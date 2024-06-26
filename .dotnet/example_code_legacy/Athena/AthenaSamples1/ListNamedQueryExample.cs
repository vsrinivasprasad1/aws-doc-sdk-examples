﻿// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0
﻿using System;
using Amazon;
using Amazon.Athena;
using Amazon.Athena.Model;

namespace AthenaSamples1
{
    /**
     * ListNamedQueryExample
     * -------------------------------------
     * This code shows how to obtain a list of named query IDs.
     */
    class ListNamedQueryExample
    {
        public static void Example()
        {
            // Create an Amazon Athena client
            var athenaConfig = new AmazonAthenaConfig
            {
                RegionEndpoint = RegionEndpoint.USEast1,
                Timeout = TimeSpan.FromMilliseconds(ExampleConstants.CLIENT_EXECUTION_TIMEOUT)
            };
            var athenaClient = new AmazonAthenaClient(config: athenaConfig);

            // Build the request
            var listNamedQueriesRequest = new ListNamedQueriesRequest();

            // Get the list results.
            var listNamedQueriesResponse = athenaClient.ListNamedQueries(listNamedQueriesRequest);

            // Process the results.
            bool hasMoreResults = true;

            while (hasMoreResults)
            {
                var namedQueryIds = listNamedQueriesResponse.NamedQueryIds;
                // process named query IDs

                // If nextToken is not null,  there are more results. Get the next page of results.
                if (!String.IsNullOrEmpty(listNamedQueriesResponse.NextToken))
                {
                    listNamedQueriesRequest.NextToken = listNamedQueriesResponse.NextToken;
                    listNamedQueriesResponse = athenaClient.ListNamedQueries(listNamedQueriesRequest);
                }
                else
                    hasMoreResults = false;
            }
        }
    }
}
