using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebAppIdentity
{
    /// <summary>
    /// 自定义HttpClientMessageHandler 类，额外处理请求信息
    /// </summary>
    public class CustomMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("message-handler", "handler");
            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
