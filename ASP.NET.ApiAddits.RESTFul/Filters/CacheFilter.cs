using ASP.NET.ApiAddits.RESTFul.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ASP.NET.ApiAddits.RESTFul.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CacheFilter : ActionFilterAttribute
    {
        private const string CacheNameKey = "X-Demo-Cache";
        private const string CacheManager = "Demo-Manager";

        public string QueryStringParamsInCacheKey { get; set; }
        public string HeaderParamsInCacheKey { get; set; }

        private string key;


        public override async Task OnActionExecutedAsync(HttpActionExecutedContext context, CancellationToken cancellationToken)
        {
            if (key == null || context.Response == null) return;

            var content = new ContentCache
            {
                Content = await context.Response.Content.ReadAsByteArrayAsync().ConfigureAwait(false),
                ContentType = context.Response.Content.Headers.ContentType,
                ExpirationPolicy = DateTime.Now.AddMinutes(20)
            };

            MemoryCacher.Add(this.key, content, new DateTimeOffset(content.ExpirationPolicy));
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(HttpActionContext context)
        {
            if (context.Request.Headers.Pragma != null && context.Request.Headers.Pragma.Any(header=> header.Name.ToLower() == "no-cache"))
            {
                base.OnActionExecuting(context);
                return;
            }

            this.key = GetObjectKey(HttpContext.Current.Request, context.ActionArguments);
            var result = MemoryCacher.GetValue<ContentCache>(key);
            if (result != null)
            {
                context.Response = context.Request.CreateResponse();
                context.Response.Content = new ByteArrayContent(result.Content);
                context.Response.Content.Headers.ContentType = result.ContentType;

                var expiration = result.ExpirationPolicy - DateTime.Now;

                context.Response.Headers.Add(CacheNameKey, CacheManager);
                context.Response.Headers.Age = expiration;
                context.Response.Headers.CacheControl = new CacheControlHeaderValue { MaxAge = expiration, MustRevalidate = DateTime.Now > result.ExpirationPolicy };

                #region - Quick response -
                //var quickResponse = context.Request.CreateResponse(HttpStatusCode.NotModified);
                //quickResponse.Headers.Add(CacheNameKey, CacheManager);

                //context.Response = quickResponse;
                #endregion

                return;
            }

            base.OnActionExecuting(context);
        }

        private string GetObjectKey(HttpRequest request, IDictionary<string, object> arguments)
        {
            var key = new CacheKey
            {
                Path = request.Path,
                Arguments = arguments
            };

            if (this.QueryStringParamsInCacheKey != null)
            {
                key.QueryString = (QueryStringParamsInCacheKey == "*") ? request.QueryString.ToString() 
                    : string.Join("&", request.QueryString.Cast<string>().Where(p => QueryStringParamsInCacheKey.Split(",".ToCharArray()).Select(pkey => pkey.Trim()).Contains(p)).Select(r => $"{r}={request.QueryString[r]}"));
            }

            if (this.HeaderParamsInCacheKey != null)
            {
                key.Header = (HeaderParamsInCacheKey == "*") ? string.Join("&", request.Headers.Cast<string>().Select(h => $"{h}={request.Headers[h]}"))
                    : string.Join("&", request.Headers.Cast<string>().Where(p => HeaderParamsInCacheKey.Split(",".ToCharArray()).Select(pkey => pkey.Trim()).Contains(p)).Select(r => $"{r}={request.Headers[r]}"));
            }

            return key.GetMD5HashKey();
        }
    }

    public class CacheKey
    {
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string Header { get; set; }

        public IDictionary<string, object> Arguments { get; set; }
        public string Body { get; set; }
    }

    public class ContentCache
    {
        public DateTime ExpirationPolicy { get; set; }
        public byte[] Content { get; set; }
        public MediaTypeHeaderValue ContentType { get; set; }
    }

    internal static class ObjectExtensions
    {
        public static string GetMD5HashKey(this object instance)
        {
            var json = JsonConvert.SerializeObject(instance, new JsonSerializerSettings
            {
                Error = (object sender, ErrorEventArgs args) =>
                {
                    args.ErrorContext.Handled = true;
                }
            });

            var md5 = MD5.Create();
            var result = md5.ComputeHash(Encoding.UTF8.GetBytes(json));

            return BitConverter.ToString(result);
        }
    }
}


