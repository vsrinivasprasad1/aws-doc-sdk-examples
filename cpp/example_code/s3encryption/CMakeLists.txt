# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0

# Set the minimum required version of CMake for this project.
cmake_minimum_required(VERSION 3.13)

# Set this project's name.
project("s3-encryption-examples")

# Set the C++ standard to use to build this target.
set(CMAKE_CXX_STANDARD 11)

# Enable CTest for testing these code examples.
include(CTest)

# Build shared libraries by default.
if(NOT BUILD_SHARED_LIBS)
    set(BUILD_SHARED_LIBS ON)
endif()

# Build in Debug mode by default
if (NOT CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE Debug)
endif()

# Find the AWS SDK for C++ package.
find_package(AWSSDK REQUIRED COMPONENTS s3-encryption)

# If the compiler is some version of Microsoft Visual C++, or another compiler simulating C++,
# and building as shared libraries, then dynamically link to those shared libraries.
if(MSVC AND AWSSDK_INSTALL_AS_SHARED_LIBS)
    add_definitions(-DUSE_IMPORT_EXPORT)
    # Copy relevant AWS SDK for C++ libraries into the current binary directory for running and debugging.
    list(APPEND SERVICE_LIST s3-encryption)
    message(STATUS "CMAKE_CURRENT_BINARY_DIR: ${CMAKE_CURRENT_BINARY_DIR}")
    AWSSDK_CPY_DYN_LIBS(SERVICE_LIST "" ${CMAKE_CURRENT_BINARY_DIR}/${CMAKE_BUILD_TYPE})
endif()

# Add the code example-specific header files.
file(GLOB AWSDOC_S3_ENCRYPTION_HEADERS
    "include/awsdoc/s3-encryption/*.h"
)

# Add the code example-specific source files.
file(GLOB AWSDOC_S3_ENCRYPTION_SOURCE
    "*.cpp"
)

# Check whether the target system is Windows, including Win64.
if(WIN32)
    # Check whether the compiler is some version of Microsoft Visual C++, or another compiler simulating C++.
    if(MSVC)
        source_group("Header Files\\awsdoc\\s3-encryption" FILES ${AWSDOC_S3_ENCRYPTION_HEADERS})
        source_group("Source Files" FILES ${AWSDOC_S3_ENCRYPTION_SOURCE})
    endif(MSVC)
endif()

foreach(file ${AWSDOC_S3_ENCRYPTION_SOURCE})
    get_filename_component(EXAMPLE ${file} NAME_WE)

    # Build the code example executables.
    set(EXAMPLE_EXE run_${EXAMPLE})

    add_executable(${EXAMPLE_EXE} ${AWSDOC_S3_ENCRYPTION_HEADERS} ${file})

    if(MSVC AND BUILD_SHARED_LIBS)
        target_compile_definitions(${EXAMPLE_EXE} PUBLIC "USE_IMPORT_EXPORT")
        target_compile_definitions(${EXAMPLE_EXE} PRIVATE "AWSDOC_S3ENCRYPTION_EXPORTS")
    endif()
    if(NOT MSVC)
        list(APPEND AWS_COMPILER_WARNINGS "-Wall" "-pedantic" "-Wextra")
        target_compile_options(${EXAMPLE_EXE} PRIVATE "${AWS_COMPILER_WARNINGS}")
    endif()

    target_include_directories(${EXAMPLE_EXE} PUBLIC
        $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
        $<INSTALL_INTERFACE:include>)
    target_link_libraries(${EXAMPLE_EXE} ${AWSSDK_LINK_LIBRARIES}
        ${AWSSDK_PLATFORM_DEPS})

    if(BUILD_TESTING)
        # Enable testing for this directory and below.
        enable_testing()

        # Build the code example libraries.
        set(EXAMPLE_LIB ${EXAMPLE})

        add_library(${EXAMPLE_LIB} ${AWSDOC_S3_ENCRYPTION_HEADERS} ${file})

        if(MSVC AND BUILD_SHARED_LIBS)
            target_compile_definitions(${EXAMPLE_LIB} PUBLIC "USE_IMPORT_EXPORT")
            target_compile_definitions(${EXAMPLE_LIB} PRIVATE "AWSDOC_S3ENCRYPTION_EXPORTS")
        endif()

        target_include_directories(${EXAMPLE_LIB} PUBLIC
            $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
            $<INSTALL_INTERFACE:include>)
        target_link_libraries(${EXAMPLE_LIB} ${AWSSDK_LINK_LIBRARIES}
            ${AWSSDK_PLATFORM_DEPS})

        # Build the code example unit tests.
        set(EXAMPLE_TEST test_${EXAMPLE})
        set(EXAMPLE_TEST_FILE ${CMAKE_CURRENT_SOURCE_DIR}/tests/test_${EXAMPLE}.cpp)

        if(EXISTS ${EXAMPLE_TEST_FILE})
            add_executable(${EXAMPLE_TEST} ${EXAMPLE_TEST_FILE} )

            target_include_directories(${EXAMPLE_TEST} PUBLIC
                $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
                $<INSTALL_INTERFACE:include>)
            target_link_libraries(${EXAMPLE_TEST} ${EXAMPLE_LIB} )
            add_test(${EXAMPLE_TEST} ${EXAMPLE_TEST})
        endif()

    endif()
endforeach()
