// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Response_Test.cs" company="Collector AB">
//   Copyright © Collector AB. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Collector.Common.RestContracts.UnitTest
{
    using Common.UnitTest.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class Response_Test : BaseUnitTest
    {
        [Test]
        public void It_returns_null_if_Data_is_read_when_response_has_errors()
        {
            var response = new Response<object>(apiVersion: string.Empty, id: string.Empty, error: new Error("code"));

            Assert.IsNull(response.Data);
        }

        [Test]
        public void It_returns_the_data_object_when_response_has_no_errors()
        {
            var data = new object();
            var response = new Response<object>(apiVersion: string.Empty, id: string.Empty, data: data);

            Assert.AreSame(data, response.Data);
        }
    }
}