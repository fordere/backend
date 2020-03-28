using System.Collections.Generic;
using forderebackend.ServiceInterface;
using forderebackend.ServiceInterface.Filters;
using forderebackend.ServiceModel.Dtos;
using forderebackend.ServiceModel.Messages;
using forderebackend.ServiceModel.Messages.Match;
using forderebackend.ServiceModel.Messages.User;
using Funq;
using ServiceStack;
using ServiceStack.Api.OpenApi;
using ServiceStack.Api.OpenApi.Specification;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using LogManager = ServiceStack.Logging.LogManager;

namespace forderebackend
{
    public class AppHostConsole : AppHostBase
    {
        public AppHostConsole()
            : base("Fordere WebService Console", typeof(UserService).Assembly)
        {
            // TODO Core Migration Logging
            //LogManager.LogFactory = new NLogFactory();
            //LogManager.GetLogger(typeof(AppHostConsole));
        }

        public override void Configure(Container container)
        {
            JsConfig.DateHandler = DateHandler.ISO8601;
            JsConfig.AssumeUtc = true;
            JsConfig.TextCase = TextCase.PascalCase;
            
            var appSettings = new AppSettings();

            ServiceExceptionHandlers.Add((httpReq, request, exception) =>
            {
                var logger = LogManager.GetLogger(GetType());
                logger.Error(exception);
                return null;
            });

            container.Register<ICacheClient>(new MemoryCacheClient {FlushOnDispose = false});
            container.RegisterAs<FordereAuthEventHandler, IAuthEvents>();
            container.Register<IDbConnectionFactory>(
                new OrmLiteConnectionFactory(
                    string.Format("Server = {0}; Port = {1}; Database = {2}; Uid = {3}; Pwd = {4}",
                        "localhost",
                        3306,
                        "fordere",
                        "root",
                        "root"),
                    MySqlDialect.Provider));

            container.Register<IUserAuthRepository>(c => new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));

            if (appSettings.Get("Debug", false))
            {
                Plugins.Add(new PostmanFeature());
            }

            Plugins.Add(new AuthFeature(() => new FordereAuthUserService(), new IAuthProvider[]
            {
                new JwtAuthProvider(AppSettings) { AuthKey = AesUtils.CreateKey() },
                new CredentialsAuthProvider()
            }));

            Plugins.Add(new RequestLogsFeature
            {
                // do not log request bodies of requests containing passwords
                HideRequestBodyForRequestDtoTypes = new[]
                    {typeof(Authenticate), typeof(Register), typeof(UpdateUserProfileRequest)},
            });

            // This is a workaround to cleanup generated api names in openapi definition
            // https://forums.servicestack.net/t/attributes-modifying-swagger-output-parameters/4787/2
            Dictionary<string, int> results = new Dictionary<string, int>();
            void UniquifyCall(string verb, OpenApiOperation op)
            {
                if (op == null) return;
                op.OperationId = op.RequestType.EndsWith("Request")
                    ? op.RequestType.Substring(0, op.RequestType.Length - 7)
                    : op.RequestType;
                if (!results.ContainsKey(op.RequestType))
                    results.Add(op.RequestType, 1);
                else
                    op.OperationId += "_" + verb;
            }

            Plugins.Add(new OpenApiFeature() {OperationFilter = UniquifyCall});

            SetConfig(new HostConfig
            {
                // TODO SSH This sets ss-pid/ss-opt to NOT HttpOnly.. is this a security issue?
                AllowNonHttpOnlyCookies = true,
                DebugMode = appSettings.Get("Debug", false)
            });

            RegisterTypedRequestFilter<EnterMatchAppointmentRequest>(Filter.EnterMatchAppointment);
            RegisterTypedRequestFilter<EnterMatchResultRequest>(Filter.EnterMatchResult);

            RegisterTypedResponseFilter<TeamDto>(Filter.TeamPlayerDetails);

            PreRequestFilters.Add((httpReq, httpRes) =>
            {
                if (httpReq.Verb.ToUpper() == "PATCH")
                {
                    httpReq.UseBufferedStream = true;
                }
            });
        }
    }
}