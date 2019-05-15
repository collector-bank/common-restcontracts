namespace Collector.Common.RestContracts.UnitTest
{
    using Collector.Common.RestContracts.Interfaces;

    using NUnit.Framework;

    public class RequestBase_Test
    {
        [Test]
        public void When_creating_any_request_then_headers_is_not_null()
        {
            var sut = new MyRequest();

            Assert.NotNull(sut.Headers);
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
