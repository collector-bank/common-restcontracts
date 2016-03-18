// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Response_Test.cs" company="Collector AB">
//   Copyright © Collector AB. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using ApprovalTests.Reporters;

namespace Collector.Common.RestContracts.ComponentTest
{
    using ApprovalTests;
    using ApprovalTests.Namers;
    using Common.ComponentTest.Helpers.ApprovalExtensions.Namers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NUnit.Framework;

    [TestFixture]
    public class Response_Test
    {
        [TestFixture]
        public class When_response_has_error : BaseComponentTest
        {
            private Response<object> _response;
            private string _jsonResponse;
            private JsonSerializerSettings _jsonSerializerSettings;

            protected override void Setup()
            {
                Approvals.RegisterDefaultNamerCreation(() => new HashCodeNamer());

                _response =
                    new Response<object>(
                        apiVersion: "1.0",
                        id: "9838FACF-30BB-42D2-90B6-3D67ADF08058",
                        context: "123",
                        error:
                            new Error(
                                code: "TEST_ERROR_CODE",
                                message: "Test error",
                                errors: new[] { new ErrorInfo("TestException", "Test error message") }));

                _jsonSerializerSettings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            }

            protected override void Act()
            {
                _jsonResponse = JsonConvert.SerializeObject(_response, _jsonSerializerSettings);
            }

            [Test]           
            [UseApprovalSubdirectory("ApprovalFiles")]
            public void The_response_is_serialized_to_the_expected_format()
            {
                Approvals.VerifyJson(_jsonResponse);
            }
        }

        [TestFixture]
        public class When_response_has_data : BaseComponentTest
        {
            private Response<string> _response;
            private string _jsonResponse;
            private JsonSerializerSettings _jsonSerializerSettings;

            protected override void Setup()
            {
                Approvals.RegisterDefaultNamerCreation(() => new HashCodeNamer());

                _response = new Response<string>(
                    apiVersion: "1.0",
                    context: "123",
                    id: "7E1589C9-8438-4CDB-8957-0001E1EEF833",
                    data: "Test successfull!");

                _jsonSerializerSettings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            }

            protected override void Act()
            {
                _jsonResponse = JsonConvert.SerializeObject(_response, _jsonSerializerSettings);
            }

            [Test]
            [UseApprovalSubdirectory("ApprovalFiles")]
            public void The_response_is_serialized_to_the_expected_format()
            {
                Approvals.VerifyJson(_jsonResponse);
            }
        }
    }
}