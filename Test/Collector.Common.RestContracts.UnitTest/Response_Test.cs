namespace Collector.Common.RestContracts.UnitTest
{
    using Xunit;

    public class Response_Test
    {
        [Fact]
        public void It_returns_null_if_Data_is_read_when_response_has_errors()
        {
            var response = new Response<object>(apiVersion: string.Empty, context: string.Empty, correlationId: string.Empty, error: new Error("code"));

            Assert.Null(response.Data);
        }

        [Fact]
        public void It_returns_the_data_object_when_response_has_no_errors()
        {
            var data = new object();
            var response = new Response<object>(apiVersion: string.Empty, context: string.Empty, correlationId: string.Empty, data: data);

            Assert.Same(data, response.Data);
        }
    }
}