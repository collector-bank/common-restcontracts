namespace Collector.Common.RestContracts.UnitTest
{
    using System.Linq;

    using Collector.Common.RestContracts.Interfaces;

    using NUnit.Framework;

    public class RequestBase_Test
    {
        [Test]
        public void When_creating_any_request_then_get_headers_never_returns_null()
        {
            var sut = new MyRequest();
            
            Assert.NotNull(sut.GetHeaders());
        }

        [Test]
        public void When_creating_any_request_adding_a_header_then_get_headers_contains_added_header()
        {
            var sut = new MyRequest();

            sut.AddHeader("test", "test");

            Assert.NotNull(sut.GetHeaders().FirstOrDefault(h => h.Key == "test"));
        }

        private class MyRequest : RequestBase<MyResourceIdentifier>
        {
            public MyRequest()
                : base(new MyResourceIdentifier())
            {
            }

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
            public string Uri => "uri";
        }
    }
}
