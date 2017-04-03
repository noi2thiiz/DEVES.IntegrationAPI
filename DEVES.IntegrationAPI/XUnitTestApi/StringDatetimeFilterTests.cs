using System;
using System.Net;
using ExtensionMethods;
using Newtonsoft.Json.Linq;
using TestApi;
using Xunit;
using Xunit.Abstractions;

namespace TestApi
{
    public class StringDatetimeFilterTests : ControllerIntegrationTests
    {

        protected string _endPoint = "http://localhost:5001/api/Test/Format";

        public StringDatetimeFilterTests(ITestOutputHelper output) : base(output)
        {


        }

        protected void AssertDateFormat(string value,HttpStatusCode ExpectStatusCode)
        {
            var data = new
            {
                datetime = value

            };


            var response = postData(_endPoint, data.ToJSON());


            Assert.NotNull(response);
            Assert.Equal(ExpectStatusCode,response.statusCode);

        }



        [Fact]
        public async  void it_should_return_success_if_given_the_valid_datetime()
        {
            AssertDateFormat("2017-03-16 00:08:36", HttpStatusCode.OK);
        }

        [Fact]
        public async  void it_should_return_success_if_given_the_valid_datetime2()
        {
            AssertDateFormat("2017-03-16", HttpStatusCode.OK);
        }

        [Fact]
        public async  void it_should_return_success_if_given_the_valid_datetime3()
        {
            AssertDateFormat("2017-03-16 23:59:59", HttpStatusCode.OK);
        }

        [Fact]
        public async  void it_should_return_bad_request_if_given_the_valid_datetime()
        {
            AssertDateFormat("xxxxx-03-16 00:08:36", HttpStatusCode.BadRequest);
        }
        [Fact]
        public async  void it_should_return_bad_request_if_given_the_valid_datetime2()
        {
            AssertDateFormat("2017-0", HttpStatusCode.BadRequest);
        }
        [Fact]
        public async  void it_should_return_bad_request_if_given_the_valid_datetime3()
        {
            AssertDateFormat("2017-30-16", HttpStatusCode.BadRequest);
        }
        [Fact]
        public async  void it_should_return_bad_request_if_given_the_valid_datetime4()
        {
            AssertDateFormat("17-30-16", HttpStatusCode.BadRequest);
        }
        [Fact]
        public async  void it_should_return_bad_request_if_given_the_valid_datetime5()
        {
            AssertDateFormat("2017-3-6", HttpStatusCode.BadRequest);

        }

        [Fact]
        public async  void it_should_return_bad_request_if_given_the_valid_datetime6()
        {
            AssertDateFormat("2007-03-16 30:08:36", HttpStatusCode.BadRequest);
        }

        [Fact]
        public async  void it_should_return_bad_request_if_given_the_valid_datetime7()
        {
            AssertDateFormat("2007-03-16 24:00:00", HttpStatusCode.BadRequest);
        }
    }
}