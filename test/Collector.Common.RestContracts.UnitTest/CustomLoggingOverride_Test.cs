namespace Collector.Common.RestContracts.UnitTest
{
    using System.Linq;

    using Collector.Common.RestContracts.Interfaces;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    public class CustomLoggingOverride_Test
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
                              RequestProperty = "hej"
                          };

            var result = request.GetRequestContentForLogging(JsonConvert.SerializeObject(request));

            const string expected = @"{
  ""RequestProperty"": ""HEJ"",
  ""Context"": null
}";

            Assert.AreEqual(expected, result);
        }

        private class MyRequest : RequestBase<MyResourceIdentifier>
        {
            public MyRequest()
                : base(new MyResourceIdentifier())
            {
            }

            public string RequestProperty { get; set; }

            public override HttpMethod GetHttpMethod()
            {
                return HttpMethod.POST;
            }

            public override string GetConfigurationKey()
            {
                return "Test";
            }

            public override string GetRequestContentForLogging(string rawContent)
            {
                var jObject = (JObject)JsonConvert.DeserializeObject(rawContent);
                if (jObject.Properties().Any(p => p.Name == nameof(RequestProperty)))
                {
                    jObject[nameof(RequestProperty)] = jObject[nameof(RequestProperty)]?.ToString()?.ToUpper();
                }

                return JsonConvert.SerializeObject(jObject, Formatting.Indented);
            }
        }

        private class MyResourceIdentifier : IResourceIdentifier
        {
            public string Uri { get; }
        }
    }
}