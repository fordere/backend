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

namespace Fordere.RestService
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
            this.Credentials = (ICredentials)new NetworkCredential(apiKey, "");
            this.Timeout = TimeSpan.FromSeconds(60.0);
            this.UserAgent = "servicestack .net stripe v1";
            this.Currency = "USD";
            JsConfig.InitStatics();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        protected virtual void InitRequest(HttpWebRequest req, string method, string idempotencyKey)
        {
            req.Accept = "application/json";
            req.Credentials = this.Credentials;
            if (method == "POST" || method == "PUT")
                req.ContentType = "application/x-www-form-urlencoded";
            if (!string.IsNullOrWhiteSpace(idempotencyKey))
                req.Headers["Idempotency-Key"] = idempotencyKey;
            req.Headers["Stripe-Version"] = "2015-10-16";
            PclExport pclExport = PclExport.Instance;
            HttpWebRequest req1 = req;
            bool? allowAutoRedirect = new bool?();
            string userAgent1 = this.UserAgent;
            TimeSpan? timeout = new TimeSpan?(this.Timeout);
            TimeSpan? readWriteTimeout = new TimeSpan?();
            string userAgent2 = userAgent1;
            bool? preAuthenticate = new bool?(true);
            pclExport.Config(req1, allowAutoRedirect, timeout, readWriteTimeout, userAgent2, preAuthenticate);
        }

        protected virtual void HandleStripeException(WebException ex)
        {
            string responseBody = HttpUtils.GetResponseBody((Exception)ex);
            HttpStatusCode? status = HttpUtils.GetStatus(ex);
            HttpStatusCode httpStatusCode = status.HasValue ? status.GetValueOrDefault() : HttpStatusCode.BadRequest;
            if (HttpUtils.IsAny400((Exception)ex))
                throw new StripeException(StringExtensions.FromJson<StripeErrors>(responseBody).Error)
                {
                    StatusCode = httpStatusCode
                };
        }

        protected virtual string Send(string relativeUrl, string method, string body, string idempotencyKey)
        {
            try
            {
                return HttpUtils.SendStringToUrl(PathUtils.CombineWith("https://api.stripe.com/v1", new string[1]
                {
          relativeUrl
                }), method, body, (string)null, "*/*", (Action<HttpWebRequest>)(req => this.InitRequest(req, method, idempotencyKey)), (Action<HttpWebResponse>)null);
            }
            catch (WebException ex)
            {
                string responseBody = HttpUtils.GetResponseBody((Exception)ex);
                HttpStatusCode? status = HttpUtils.GetStatus(ex);
                HttpStatusCode httpStatusCode = status.HasValue ? status.GetValueOrDefault() : HttpStatusCode.BadRequest;
                if (HttpUtils.IsAny400((Exception)ex))
                    throw new StripeException(StringExtensions.FromJson<StripeErrors>(responseBody).Error)
                    {
                        StatusCode = httpStatusCode
                    };
                throw;
            }
        }

        protected virtual async Task<string> SendAsync(string relativeUrl, string method, string body, string idempotencyKey)
        {
            string str;
            try
            {
                str = await HttpUtils.SendStringToUrlAsync(PathUtils.CombineWith("https://api.stripe.com/v1", new string[1]
                {
          relativeUrl
                }), method, body, (string)null, "*/*", (Action<HttpWebRequest>)(req => this.InitRequest(req, method, idempotencyKey)), (Action<HttpWebResponse>)null);
            }
            catch (Exception ex1)
            {
                WebException ex2 = TaskExtensions.UnwrapIfSingleException(ex1) as WebException;
                if (ex2 != null)
                    this.HandleStripeException(ex2);
                throw;
            }
            return str;
        }

        public T Send<T>(IReturn<T> request, string method, bool sendRequestBody = true, string idempotencyKey = null)
        {
            using (new FordereStripeGateway.ConfigScope())
            {
                string relativeUrl = ServiceStack.UrlExtensions.ToUrl((IReturn)request, method, (string)null);
                string body = sendRequestBody ? QueryStringSerializer.SerializeToString<IReturn<T>>(request) : (string)null;
                return StringExtensions.FromJson<T>(this.Send(relativeUrl, method, body, idempotencyKey));
            }
        }

        public async Task<T> SendAsync<T>(IReturn<T> request, string method, bool sendRequestBody = true, string idempotencyKey = null)
        {
            string relativeUrl;
            string body;
            using (new FordereStripeGateway.ConfigScope())
            {
                relativeUrl = ServiceStack.UrlExtensions.ToUrl((IReturn)request, method, (string)null);
                body = sendRequestBody ? QueryStringSerializer.SerializeToString<IReturn<T>>(request) : (string)null;
            }
            string json = await this.SendAsync(relativeUrl, method, body, idempotencyKey);
            T obj;
            using (new FordereStripeGateway.ConfigScope())
                obj = StringExtensions.FromJson<T>(json);
            return obj;
        }

        private static string GetMethod<T>(IReturn<T> request)
        {
            if (request is IPost)
                return "POST";
            if (request is IPut)
                return "PUT";
            return !(request is IDelete) ? "GET" : "DELETE";
        }

        public T Send<T>(IReturn<T> request)
        {
            string method = FordereStripeGateway.GetMethod<T>(request);
            return this.Send<T>(request, method, method == "POST" || method == "PUT", (string)null);
        }

        public Task<T> SendAsync<T>(IReturn<T> request)
        {
            string method = FordereStripeGateway.GetMethod<T>(request);
            return this.SendAsync<T>(request, method, method == "POST" || method == "PUT", (string)null);
        }

        public T Get<T>(IReturn<T> request)
        {
            return this.Send<T>(request, "GET", false, (string)null);
        }

        public Task<T> GetAsync<T>(IReturn<T> request)
        {
            return this.SendAsync<T>(request, "GET", false, (string)null);
        }

        public T Post<T>(IReturn<T> request)
        {
            return this.Send<T>(request, "POST", true, (string)null);
        }

        public Task<T> PostAsync<T>(IReturn<T> request)
        {
            return this.SendAsync<T>(request, "POST", true, (string)null);
        }

        public T Post<T>(IReturn<T> request, string idempotencyKey)
        {
            return this.Send<T>(request, "POST", true, idempotencyKey);
        }

        public Task<T> PostAsync<T>(IReturn<T> request, string idempotencyKey)
        {
            return this.SendAsync<T>(request, "POST", true, idempotencyKey);
        }

        public T Put<T>(IReturn<T> request)
        {
            return this.Send<T>(request, "PUT", true, (string)null);
        }

        public Task<T> PutAsync<T>(IReturn<T> request)
        {
            return this.SendAsync<T>(request, "PUT", true, (string)null);
        }

        public T Delete<T>(IReturn<T> request)
        {
            return this.Send<T>(request, "DELETE", false, (string)null);
        }

        public Task<T> DeleteAsync<T>(IReturn<T> request)
        {
            return this.SendAsync<T>(request, "DELETE", false, (string)null);
        }

        public class ConfigScope : IDisposable
        {
            private readonly WriteComplexTypeDelegate holdQsStrategy;
            private readonly JsConfigScope jsConfigScope;

            public ConfigScope()
            {
                bool? convertObjectTypesIntoStringDictionary = new bool?();
                bool? tryToParsePrimitiveTypeValues = new bool?();
                bool? tryToParseNumericType = new bool?();
                ParseAsType? parsePrimitiveFloatingPointTypes = new ParseAsType?();
                ParseAsType? parsePrimitiveIntegerTypes = new ParseAsType?();
                bool? excludeDefaultValues = new bool?();
                bool? includeNullValues = new bool?();
                bool? includeNullValuesInDictionaries = new bool?();
                bool? includeDefaultEnums = new bool?();
                bool? excludeTypeInfo = new bool?();
                bool? includeTypeInfo = new bool?();
                DateHandler? nullable1 = new DateHandler?(DateHandler.UnixTime);
                PropertyConvention? nullable2 = new PropertyConvention?(PropertyConvention.Lenient);
                bool? nullable3 = new bool?(true);
                bool? emitCamelCaseNames = new bool?(false);
                bool? emitLowercaseUnderscoreNames = nullable3;
                DateHandler? dateHandler = nullable1;
                TimeSpanHandler? timeSpanHandler = new TimeSpanHandler?();
                PropertyConvention? propertyConvention = nullable2;
                bool? preferInterfaces = new bool?();
                bool? throwOnDeserializationError = new bool?();
                // ISSUE: variable of the null type
                // ISSUE: variable of the null type
                // ISSUE: variable of the null type
                bool? treatEnumAsInteger = new bool?();
                bool? skipDateTimeConversion = new bool?();
                bool? alwaysUseUtc = new bool?();
                bool? assumeUtc = new bool?();
                bool? appendUtcOffset = new bool?();
                bool? escapeUnicode = new bool?();
                bool? includePublicFields = new bool?();
                int? maxDepth = new int?();
                // ISSUE: variable of the null type
                // ISSUE: variable of the null type
                this.jsConfigScope = JsConfig.With(convertObjectTypesIntoStringDictionary, tryToParsePrimitiveTypeValues, tryToParseNumericType, parsePrimitiveFloatingPointTypes, parsePrimitiveIntegerTypes, excludeDefaultValues, includeNullValues, includeNullValuesInDictionaries, includeDefaultEnums, excludeTypeInfo, includeTypeInfo, emitCamelCaseNames, emitLowercaseUnderscoreNames, dateHandler, timeSpanHandler, propertyConvention, preferInterfaces, throwOnDeserializationError, (string)null, null, (Func<Type, string>)null, (Func<string, Type>)null, treatEnumAsInteger, skipDateTimeConversion, alwaysUseUtc, assumeUtc, appendUtcOffset, escapeUnicode, includePublicFields, maxDepth, (EmptyCtorFactoryDelegate)null, (string[])null);
                this.holdQsStrategy = QueryStringSerializer.ComplexTypeStrategy;
                QueryStringSerializer.ComplexTypeStrategy = new WriteComplexTypeDelegate(QueryStringStrategy.FormUrlEncoded);
            }

            public void Dispose()
            {
                QueryStringSerializer.ComplexTypeStrategy = this.holdQsStrategy;
                this.jsConfigScope.Dispose();
            }
        }
    }
}
