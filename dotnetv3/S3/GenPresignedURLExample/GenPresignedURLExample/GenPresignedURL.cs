﻿// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier:  Apache-2.0

namespace GenPresignedURLExample
{
    using System;
    using Amazon;
    using Amazon.S3;
    using Amazon.S3.Model;

    /// <summary>
    /// This example generates a presigned URL for an object in an Amazon
    /// Amazon Simple Storage Service (Amazon S3) bucket. The generated example
    /// remains valid for the specified number of hours. This example was
    /// created using the AWS SDK for .NET version 3.7 and .NET Core 5.0.
    /// </summary>
    public class GenPresignedURL
    {
        public static void Main()
        {
            const string bucketName = "doc-example-bucket";
            const string objectKey = "sample.txt";

            // Specify how long the presigned URL lasts, in hours
            const double timeoutDuration = 12;

            // Specify your bucket region (an example region is shown).
            RegionEndpoint bucketRegion = RegionEndpoint.USWest2;

            IAmazonS3 s3Client = new AmazonS3Client();
            string urlString = GeneratePresignedURL(s3Client, bucketName, objectKey, timeoutDuration);
            Console.WriteLine($"The generated URL is: {urlString}.");
        }

        /// <summary>
        /// Gemerate a presigned URL that can be used to access the file named
        /// in the ojbectKey parameter for the amount of time specified in the
        /// duration parameter.
        /// </summary>
        /// <param name="client">An initialized S3 client object used to call
        /// the GetPresignedUrl method.</param>
        /// <param name="bucketName">The name of the bucket where the object
        /// for which to create the presigned URL resides.</param>
        /// <param name="objectKey">The name of the object which be accessed
        /// with the generated URL.</param>
        /// <param name="duration">The length of time for which the URL will be
        /// valid.</param>
        /// <returns>A string representing the generated URL.</returns>
        public static string GeneratePresignedURL(IAmazonS3 client, string bucketName, string objectKey, double duration)
        {
            string urlString = string.Empty;
            try
            {
                GetPreSignedUrlRequest request = new ()
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    Expires = DateTime.UtcNow.AddHours(duration),
                };
                urlString = client.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error:'{ex.Message}'");
            }

            return urlString;
        }
    }
}
