using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Umbrella.Infrastructure.Api;

namespace Umbrella.Infrastructure.Tests.Api
{
    public class ApiCollerTests
    {
        ApiCallRequestDTO _Request;
        ApiCaller _Client;

        [SetUp]
        public void Setup()
        {
            this._Request = new ApiCallRequestDTO()
            {
                Url = "https://xxxxxxxxxxxxx-njkmy6ao7q-ey.a.run.app",
                MethodName = "v1/appsettings/version",
                RequestHeader = { 
                    ["channel"] = "CHNL",
                    ["trxId"] = "123",
                    ["UmbrellaAuth"] = "123|123",
                }
            };
        }

        ApiCaller InstanceRestClient()
        {
            return new ApiCaller(new Mock<ILogger>().Object);
        }


        public async Task InvokeGetApi_Is_Succesfull()
        {
            //**************** GIVEN
            this._Client = InstanceRestClient();

            //**************** WHEN
            var response = await this._Client.GetAsync<string>(this._Request);

            //**************** ASSERT
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSucceded, Is.True);
            Assert.That(String.IsNullOrEmpty(response.Body), Is.False);
            Assert.Pass();
            await Task.CompletedTask;
        }

        [Test]
        public async Task InvokeGetApi_Returns_NotSucessIFAnyErrorOccurs()
        {
            //**************** GIVEN
            this._Client = InstanceRestClient();
            this._Request = new ApiCallRequestDTO()
            {
                Url = "https://xxxxxxxxxxxxx-njkmy6ao7q-ey.a.run.app",
                MethodName = "v1/appsettings/version",
                RequestHeader = {
                    ["channel"] = "CHNL",
                    ["trxId"] = "123",
                    ["UmbrellaAuth"] = "123|123",
                }
            };

            //**************** WHEN
            var response = await this._Client.GetAsync<string>(this._Request);

            //**************** ASSERT
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSucceded, Is.False);
            Assert.That(response.StatusCode, Is.EqualTo(500));
            Assert.That(String.IsNullOrEmpty(response.Error), Is.False);
            Assert.Pass();
            await Task.CompletedTask;
        }
    }
}