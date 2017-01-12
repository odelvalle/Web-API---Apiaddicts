using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ASP.NET.ApiAddits.MessagesHandler.Handlers
{
    public class ApiKeyHandler : DelegatingHandler
    {
        public string Key { get; set; }

        public ApiKeyHandler(string key)
        {
            this.Key = key;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var query = request.RequestUri.ParseQueryString();

            if (!(query["key"] == Key))
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);

                return tsc.Task;
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}