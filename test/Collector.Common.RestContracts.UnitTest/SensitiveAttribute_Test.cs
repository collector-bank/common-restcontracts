namespace Collector.Common.RestContracts.UnitTest
{
    using Collector.Common.RestContracts.Interfaces;

    using Newtonsoft.Json;

    using NUnit.Framework;

    public class SensitiveAttribute_Test
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void When_request_has_sensitive_property_then_it_is_masked_for_content_logging()
        {
            var request = new MyRequest
                          {
                              SensitiveRequestString = "secret",
                              SensitiveRequestInteger = 123456789,
                              NonSensitiveRequestString = "info",
                              NonSensitiveRequestInteger = 123456789
                          };

            var result = request.GetRequestContentForLogging(JsonConvert.SerializeObject(request));

            const string expected = @"{
  ""SensitiveRequestString"": ""***"",
  ""SensitiveRequestInteger"": ""*****6789"",
  ""NonSensitiveRequestString"": ""info"",
  ""NonSensitiveRequestInteger"": 123456789,
  ""Context"": null
}";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void When_request_has_sensitive_property_nothing_is_returned_for_raw_content_logging()
        {
            var request = new MyRequest();

            var result = request.GetRawRequestContentForLogging(JsonConvert.SerializeObject(request));

            Assert.AreEqual("Request contains sensitive information", result);
        }

        [Test]
        public void When_response_has_sensitive_property_then_it_is_masked_for_content_logging()
        {
            var request = new MyRequest();

            var response = new Response<MyResponse>
                           {
                               Data = new MyResponse
                                      {
                                          SensitiveResponseString = "secret",
                                          SensitiveResponseLong = 12341234,
                                          NonSensitiveResponseString = "info",
                                          NonSensitiveResponseLong = 123456789
                                      }
                           };

            var result = request.GetResponseContentForLogging(JsonConvert.SerializeObject(response), "application/json");

            const string expected = @"{
  ""ApiVersion"": null,
  ""Context"": null,
  ""Data"": {
    ""SensitiveResponseString"": ""-"",
    ""SensitiveResponseLong"": ""12##HIDDEN##"",
    ""NonSensitiveResponseString"": ""info"",
    ""NonSensitiveResponseLong"": 123456789
  },
  ""Error"": null,
  ""CorrelationId"": null
}";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void When_a_response_that_does_not_follow_Data_property_pattern_has_sensitive_property_then_it_is_masked_for_content_logging()
        {
            var request = new MyRequest();

            var response = new MyResponse
                           {
                               SensitiveResponseString = "secret",
                               SensitiveResponseLong = 12341234,
                               NonSensitiveResponseString = "info",
                               NonSensitiveResponseLong = 123456789
                           };

            var result = request.GetResponseContentForLogging(JsonConvert.SerializeObject(response), "application/json");

            const string expected = @"{
  ""SensitiveResponseString"": ""-"",
  ""SensitiveResponseLong"": ""12##HIDDEN##"",
  ""NonSensitiveResponseString"": ""info"",
  ""NonSensitiveResponseLong"": 123456789
}";

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void When_response_has_sensitive_property_then_nothing_is_returned_for_raw_content_logging()
        {
            var request = new MyRequest();

            var response = new Response<MyResponse> { Data = new MyResponse() };

            var result = request.GetRawResponseContentForLogging(JsonConvert.SerializeObject(response), "application/json");

            Assert.AreEqual("Response contains sensitive information", result);
        }

        private class MyRequest : RequestBase<MyResourceIdentifier, MyResponse>
        {
            public MyRequest()
                : base(new MyResourceIdentifier())
            {
            }

            [Sensitive]
            public string SensitiveRequestString { get; set; }

            [Sensitive(PreserveLength = true, ShowLast = 4)]
            public int SensitiveRequestInteger { get; set; }

            public string NonSensitiveRequestString { get; set; }

            public int NonSensitiveRequestInteger { get; set; }

            public override HttpMethod GetHttpMethod()
            {
                return HttpMethod.POST;
            }

            public override string GetConfigurationKey()
            {
                return "Test";
            }
        }

        private class MyResourceIdentifier : IResourceIdentifier
        {
            public string Uri { get; }
        }

        private class MyResponse
        {
            [Sensitive(Text = "-")]
            public string SensitiveResponseString { get; set; }

            [Sensitive(Text = "##HIDDEN##", ShowFirst = 2)]
            public long SensitiveResponseLong { get; set; }

            public string NonSensitiveResponseString { get; set; }

            public long NonSensitiveResponseLong { get; set; }
        }
    }
}