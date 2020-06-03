/*
 * Copyright 2019 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.Lambda.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Amazon.Lambda.RuntimeSupport.IntegrationTests
{
    public class WrraperCustomRuntimeTests : CustomRuntimeBase {
#if SKIP_RUNTIME_SUPPORT_INTEG_TESTS
        [Fact(Skip = "Skipped intentionally by setting the SkipRuntimeSupportIntegTests build parameter.")]
#else
        [Fact]
#endif
        public async Task TestAllHandlersAsync()
        {
            // run all test cases in one test to ensure they run serially
            using (var lambdaClient = new AmazonLambdaClient(TestRegion))
            using (var s3Client = new AmazonS3Client(TestRegion))
            using (var iamClient = new AmazonIdentityManagementServiceClient(TestRegion))
            {
                var roleAlreadyExisted = false;

                try
                {
                    roleAlreadyExisted = await PrepareTestResources(s3Client, lambdaClient, iamClient);

                    await RunTestSuccessAsync(lambdaClient, "ToUpperAsync", "message", "ToUpperAsync-MESSAGE");
                    await RunTestSuccessAsync(lambdaClient, "PingAsync", "ping", "PingAsync-pong");
                    await RunTestSuccessAsync(lambdaClient, "HttpsWorksAsync", "", "HttpsWorksAsync-SUCCESS");
                    await RunTestSuccessAsync(lambdaClient, "CertificateCallbackWorksAsync", "", "CertificateCallbackWorksAsync-SUCCESS");
                    await RunTestSuccessAsync(lambdaClient, "NetworkingProtocolsAsync", "", "NetworkingProtocolsAsync-SUCCESS");
                    await RunTestSuccessAsync(lambdaClient, "HandlerEnvVarAsync", "", "HandlerEnvVarAsync-HandlerEnvVarAsync");
                    await RunTestExceptionAsync(lambdaClient, "AggregateExceptionUnwrappedAsync", "", "Exception", "Exception thrown from an async handler.");
                    await RunTestExceptionAsync(lambdaClient, "AggregateExceptionUnwrapped", "", "Exception", "Exception thrown from a synchronous handler.");
                    await RunTestExceptionAsync(lambdaClient, "AggregateExceptionNotUnwrappedAsync", "", "AggregateException", "AggregateException thrown from an async handler.");
                    await RunTestExceptionAsync(lambdaClient, "AggregateExceptionNotUnwrapped", "", "AggregateException", "AggregateException thrown from a synchronous handler.");
                    await RunTestExceptionAsync(lambdaClient, "TooLargeResponseBodyAsync", "", "Function.ResponseSizeTooLarge", "Response payload size (7340060 bytes) exceeded maximum allowed payload size (6291556 bytes).");
                    await RunTestSuccessAsync(lambdaClient, "LambdaEnvironmentAsync", "", "LambdaEnvironmentAsync-SUCCESS");
                    await RunTestSuccessAsync(lambdaClient, "LambdaContextBasicAsync", "", "LambdaContextBasicAsync-SUCCESS");
                    await RunTestSuccessAsync(lambdaClient, "GetPidDllImportAsync", "", "GetPidDllImportAsync-SUCCESS");
                    await RunTestSuccessAsync(lambdaClient, "GetTimezoneNameAsync", "", "GetTimezoneNameAsync-UTC");
                }
                finally
                {
                    await CleanUpTestResources(s3Client, lambdaClient, iamClient, roleAlreadyExisted);
                }
            }
        }
    }
}
