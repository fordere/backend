// Decompiled with JetBrains decompiler
// Type: ServiceStack.Stripe.StripeGateway
// Assembly: Stripe, Version=4.5.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 3404E53C-9FC9-4DB9-A22C-1FED9D7D5618
// Assembly location: D:\Projects\fordere\src\packages\ServiceStack.Stripe.4.5.4\lib\net45\Stripe.dll

using System;
using System.Net;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Stripe.Types;
using ServiceStack.Text;
using TaskExtensions = ServiceStack.TaskExtensions;

namespace forderebackend.ServiceInterface
{
    public class FordereStripeGateway : IRestGateway
    {
        private const string BaseUrl = "https://api.stripe.com/v1";
        private const string APIVersion = "2015-10-16";
        private string apiKey;
        private string publishableKey;

        public TimeSpan Timeout { get; set; }

        public string Currency { get; set; }

        public ICredentials Credentials { get; set; }

        private string UserAgent { get; set; }

        public FordereStripeGateway(string apiKey, string publishableKey = null)
        {
            this.apiKey = apiKey;
            this.publishableKey = publishableKey;
            Credentials = (ICredentials) new NetworkCredential(apiKey, "");
            Timeout = TimeSpan.FromSeconds(60.0);
            UserAgent = "servicestack .net stripe v1";
            Currency = "USD";
            JsConfig.InitStatics();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        protected virtual void InitRequest(HttpWebRequest req, string method, string idempotencyKey)
        {
            req.Accept = "application/json";
            req.Credentials = Credentials;
            if (method == "POST" || method == "PUT")
            {
                req.ContentType = "application/x-www-form-urlencoded";
            }

            if (!string.IsNullOrWhiteSpace(idempotencyKey))
            {
                req.Headers["Idempotency-Key"] = idempotencyKey;
            }

            req.Headers["Stripe-Version"] = "2015-10-16";
            var pclExport = PclExport.Instance;
            var req1 = req;
            var allowAutoRedirect = new bool?();
            var userAgent1 = UserAgent;
            var timeout = new TimeSpan?(Timeout);
            var readWriteTimeout = new TimeSpan?();
            var userAgent2 = userAgent1;
            var preAuthenticate = new bool?(true);
            pclExport.Config(req1, allowAutoRedirect, timeout, readWriteTimeout, userAgent2, preAuthenticate);
        }

        protected virtual void HandleStripeException(WebException ex)
        {
            var responseBody = HttpUtils.GetResponseBody((Exception) ex);
            var status = HttpUtils.GetStatus(ex);
            var httpStatusCode = status.HasValue ? status.GetValueOrDefault() : HttpStatusCode.BadRequest;
            if (HttpUtils.IsAny400((Exception) ex))
            {
                throw new StripeException(StringExtensions.FromJson<StripeErrors>(responseBody).Error)
                {
                    StatusCode = httpStatusCode
                };
            }
        }

        protected virtual string Send(string relativeUrl, string method, string body, string idempotencyKey)
        {
            try
            {
                return HttpUtils.SendStringToUrl(PathUtils.CombineWith("https://api.stripe.com/v1", new string[1]
                    {
                        relativeUrl
                    }), method, body, (string) null, "*/*",
                    (Action<HttpWebRequest>) (req => InitRequest(req, method, idempotencyKey)),
                    (Action<HttpWebResponse>) null);
            }
            catch (WebException ex)
            {
                var responseBody = HttpUtils.GetResponseBody((Exception) ex);
                var status = HttpUtils.GetStatus(ex);
                var httpStatusCode = status.HasValue ? status.GetValueOrDefault() : HttpStatusCode.BadRequest;
                if (HttpUtils.IsAny400((Exception) ex))
                {
                    throw new StripeException(StringExtensions.FromJson<StripeErrors>(responseBody).Error)
                    {
                        StatusCode = httpStatusCode
                    };
                }

                throw;
            }
        }

        protected virtual async Task<string> SendAsync(string relativeUrl, string method, string body,
            string idempotencyKey)
        {
            string str;
            try
            {
                str = await HttpUtils.SendStringToUrlAsync(PathUtils.CombineWith("https://api.stripe.com/v1",
                        new string[1]
                        {
                            relativeUrl
                        }), method, body, (string) null, "*/*",
                    (Action<HttpWebRequest>) (req => InitRequest(req, method, idempotencyKey)),
                    (Action<HttpWebResponse>) null);
            }
            catch (Exception ex1)
            {
                var ex2 = TaskExtensions.UnwrapIfSingleException(ex1) as WebException;
                if (ex2 != null)
                {
                    HandleStripeException(ex2);
                }

                throw;
            }

            return str;
        }

        public T Send<T>(IReturn<T> request, string method, bool sendRequestBody = true, string idempotencyKey = null)
        {
            using (new ConfigScope())
            {
                var relativeUrl = UrlExtensions.ToUrl((IReturn) request, method, (string) null);
                var body = sendRequestBody
                    ? QueryStringSerializer.SerializeToString<IReturn<T>>(request)
                    : (string) null;
                return StringExtensions.FromJson<T>(Send(relativeUrl, method, body, idempotencyKey));
            }
        }

        public async Task<T> SendAsync<T>(IReturn<T> request, string method, bool sendRequestBody = true,
            string idempotencyKey = null)
        {
            string relativeUrl;
            string body;
            using (new ConfigScope())
            {
                relativeUrl = UrlExtensions.ToUrl((IReturn) request, method, (string) null);
                body = sendRequestBody ? QueryStringSerializer.SerializeToString<IReturn<T>>(request) : (string) null;
            }

            var json = await SendAsync(relativeUrl, method, body, idempotencyKey);
            T obj;
            using (new ConfigScope())
            {
                obj = StringExtensions.FromJson<T>(json);
            }

            return obj;
        }

        private static string GetMethod<T>(IReturn<T> request)
        {
            if (request is IPost)
            {
                return "POST";
            }

            if (request is IPut)
            {
                return "PUT";
            }

            return !(request is IDelete) ? "GET" : "DELETE";
        }

        public T Send<T>(IReturn<T> request)
        {
            var method = GetMethod<T>(request);
            return Send<T>(request, method, method == "POST" || method == "PUT", (string) null);
        }

        public Task<T> SendAsync<T>(IReturn<T> request)
        {
            var method = GetMethod<T>(request);
            return SendAsync<T>(request, method, method == "POST" || method == "PUT", (string) null);
        }

        public T Get<T>(IReturn<T> request)
        {
            return Send<T>(request, "GET", false, (string) null);
        }

        public Task<T> GetAsync<T>(IReturn<T> request)
        {
            return SendAsync<T>(request, "GET", false, (string) null);
        }

        public T Post<T>(IReturn<T> request)
        {
            return Send<T>(request, "POST", true, (string) null);
        }

        public Task<T> PostAsync<T>(IReturn<T> request)
        {
            return SendAsync<T>(request, "POST", true, (string) null);
        }

        public T Post<T>(IReturn<T> request, string idempotencyKey)
        {
            return Send<T>(request, "POST", true, idempotencyKey);
        }

        public Task<T> PostAsync<T>(IReturn<T> request, string idempotencyKey)
        {
            return SendAsync<T>(request, "POST", true, idempotencyKey);
        }

        public T Put<T>(IReturn<T> request)
        {
            return Send<T>(request, "PUT", true, (string) null);
        }

        public Task<T> PutAsync<T>(IReturn<T> request)
        {
            return SendAsync<T>(request, "PUT", true, (string) null);
        }

        public T Delete<T>(IReturn<T> request)
        {
            return Send<T>(request, "DELETE", false, (string) null);
        }

        public Task<T> DeleteAsync<T>(IReturn<T> request)
        {
            return SendAsync<T>(request, "DELETE", false, (string) null);
        }

        public class ConfigScope : IDisposable
        {
            private readonly WriteComplexTypeDelegate holdQsStrategy;
            private readonly JsConfigScope jsConfigScope;

            public ConfigScope()
            {
                var convertObjectTypesIntoStringDictionary = new bool?();
                var tryToParsePrimitiveTypeValues = new bool?();
                var tryToParseNumericType = new bool?();
                var parsePrimitiveFloatingPointTypes = new ParseAsType?();
                var parsePrimitiveIntegerTypes = new ParseAsType?();
                var excludeDefaultValues = new bool?();
                var includeNullValues = new bool?();
                var includeNullValuesInDictionaries = new bool?();
                var includeDefaultEnums = new bool?();
                var excludeTypeInfo = new bool?();
                var includeTypeInfo = new bool?();
                var nullable1 = new DateHandler?(DateHandler.UnixTime);
                var nullable2 = new PropertyConvention?(PropertyConvention.Lenient);
                var nullable3 = new bool?(true);
                var emitCamelCaseNames = new bool?(false);
                var emitLowercaseUnderscoreNames = nullable3;
                var dateHandler = nullable1;
                var timeSpanHandler = new TimeSpanHandler?();
                var propertyConvention = nullable2;
                var preferInterfaces = new bool?();
                var throwOnDeserializationError = new bool?();
                // ISSUE: variable of the null type
                // ISSUE: variable of the null type
                // ISSUE: variable of the null type
                var treatEnumAsInteger = new bool?();
                var skipDateTimeConversion = new bool?();
                var alwaysUseUtc = new bool?();
                var assumeUtc = new bool?();
                var appendUtcOffset = new bool?();
                var escapeUnicode = new bool?();
                var includePublicFields = new bool?();
                var maxDepth = new int?();
                // ISSUE: variable of the null type
                // ISSUE: variable of the null type
                jsConfigScope = JsConfig.With(convertObjectTypesIntoStringDictionary, tryToParsePrimitiveTypeValues,
                    tryToParseNumericType, parsePrimitiveFloatingPointTypes, parsePrimitiveIntegerTypes,
                    excludeDefaultValues, includeNullValues, includeNullValuesInDictionaries, includeDefaultEnums,
                    excludeTypeInfo, includeTypeInfo, emitCamelCaseNames, emitLowercaseUnderscoreNames, dateHandler,
                    timeSpanHandler, propertyConvention, preferInterfaces, throwOnDeserializationError, (string) null,
                    null, (Func<Type, string>) null, (Func<string, Type>) null, treatEnumAsInteger,
                    skipDateTimeConversion, alwaysUseUtc, assumeUtc, appendUtcOffset, escapeUnicode,
                    includePublicFields, maxDepth, (EmptyCtorFactoryDelegate) null, (string[]) null);
                holdQsStrategy = QueryStringSerializer.ComplexTypeStrategy;
                QueryStringSerializer.ComplexTypeStrategy =
                    new WriteComplexTypeDelegate(QueryStringStrategy.FormUrlEncoded);
            }

            public void Dispose()
            {
                QueryStringSerializer.ComplexTypeStrategy = holdQsStrategy;
                jsConfigScope.Dispose();
            }
        }
    }
}