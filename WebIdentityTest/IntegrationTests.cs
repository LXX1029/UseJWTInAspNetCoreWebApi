using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppIdentity;
using Xunit;

namespace WebIdentityTest
{

    public class IntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _fixture;
        public IntegrationTests(WebApplicationFactory<Startup> fixture)
        {
            this._fixture = fixture;
        }

        [Fact]
        public async Task PingRequest_ReturnsPong()
        {
            var client = _fixture.CreateClient();
            var response = await client.GetAsync("/ping");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now.ToShortTimeString()}  {content}");
            Assert.Contains("pong", content);
        }
    }
}
