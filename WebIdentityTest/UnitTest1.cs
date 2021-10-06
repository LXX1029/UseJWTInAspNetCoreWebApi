using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using WebAppIdentity.Middleware;
using Xunit;

namespace WebIdentityTest
{
    public class UnitTest1
    {
        //[Fact]
        [Theory]
        [InlineData(1,1)]
        [InlineData(1, 3)]
        public void Test1(int value1,int value2)
        {
            Assert.Equal(value1, value2);
        }

        [Fact]
        public async void Test2()
        {
            var bodyStream = new MemoryStream();
            var context = new DefaultHttpContext();
            context.Response.Body = bodyStream;
            context.Request.Path = "/ping";
            var wasExecuted = false;
             RequestDelegate next = (HttpContext ctx) => {
                 wasExecuted = true;
                return Task.CompletedTask;
            };
            var middleware = new PingPongMiddleware(next);
            await middleware.Invoke(context);

            // 从body 中读取到返回内容
            string response;
            bodyStream.Seek(0, SeekOrigin.Begin);
            using(var stringReader = new StreamReader(bodyStream))
            {
                response = await stringReader.ReadToEndAsync();
            }

            //Assert.True(wasExecuted);
            Assert.Contains("pong", response);
        }

   
    }
}
