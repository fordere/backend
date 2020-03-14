using System.Linq;

using Fordere.RestService;
using Fordere.RestService.Filters;
using Fordere.ServiceInterface.Dtos;
using Fordere.ServiceInterface.Messages;
using Fordere.ServiceInterface.Messages.Match;
using Fordere.ServiceInterface.Messages.User;

using Funq;

using ServiceStack;
using ServiceStack.Api.OpenApi;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.Logging.NLogger;
using ServiceStack.MiniProfiler;
using ServiceStack.MiniProfiler.Data;
using ServiceStack.OrmLite;
using ServiceStack.Redis;
using ServiceStack.Text;

using LogManager = ServiceStack.Logging.LogManager;

namespace Fordere.WebConsole
{
    public class AppHostConsole : AppSelfHostBase
    {
        public AppHostConsole()
            : base("Fordere WebService Console", typeof(UserService).Assembly)
        {
            Licensing.RegisterLicense("TODO");

            LogManager.LogFactory = new NLogFactory();
            LogManager.GetLogger(typeof(AppHostConsole));
        }

        public override void Configure(Container container)
        {
            JsConfig.DateHandler = DateHandler.ISO8601;

            var appSettings = new AppSettings();

            ServiceExceptionHandlers.Add((httpReq, request, exception) =>
            {
                var logger = LogManager.GetLogger(GetType());
                logger.Error(exception);
                return null;
            });

            if (appSettings.Get("Redis.Enabled", false))
            {
                container.Register<IRedisClientsManager>(c => new PooledRedisClientManager("{0}:{1}".Fmt(appSettings.Get("Redis.Ip"), appSettings.Get("Redis.Port", 6379))));
                container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient());
            }
            else
            {
                container.Register<ICacheClient>(new MemoryCacheClient { FlushOnDispose = false });
            }

            JsConfig.AssumeUtc = true;

            container.RegisterAs<FordereAuthEventHandler, IAuthEvents>();

            container.Register<IDbConnectionFactory>(
                new OrmLiteConnectionFactory(
                    "Server = {0}; Database = {1}; Uid = {2}; Pwd = {3}".Fmt(
                        appSettings.Get("DB.Host"),
                        appSettings.Get("DB.Name"),
                        appSettings.Get("DB.User"),
                        appSettings.Get("DB.Pass")),
                    MySqlDialect.Provider));

            container.Register<IUserAuthRepository>(c => new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));

            var authProvider = new IAuthProvider[]
            {
                new CredentialsAuthProvider(),
                new JwtAuthProvider(appSettings),
            }.ToList();

            if (appSettings.Get("Debug", false))
            {
                authProvider.Add(new BasicAuthProvider());
            }

            var authFeature = new AuthFeature(() => new FordereAuthUserService(), authProvider.ToArray());


            //authFeature.AuthEvents.Add(new WebSudoFeature());

            this.Plugins.Add(new RegistrationFeature());
            this.Plugins.Add(authFeature);


            this.Plugins.Add(new RequestLogsFeature
            {
                // do not log request bodies of requests containing passwords
                HideRequestBodyForRequestDtoTypes = new[] { typeof(Authenticate), typeof(Register), typeof(UpdateUserProfileRequest) },
            });

            if (appSettings.Get("CORS.Enabled", false))
            {
                this.Plugins.Add(
                    new CorsFeature(
                        allowedOrigins: appSettings.GetString("CORS.AllowedOrigins"),
                        allowedMethods: "OPTIONS,GET,POST,PUT,DELETE,PATCH",
                        allowedHeaders: "Content-Type,Authorization,division_id",
                        allowCredentials: true));
            }

            if (appSettings.Get("Debug", false))
            {
                this.Plugins.Add(new PostmanFeature());
                this.Plugins.Add(new OpenApiFeature());
            }

            if (appSettings.Get("Debug", false) == false)
            {
                this.Plugins.RemoveAll(x => x is MetadataFeature);
            }

            this.SetConfig(new HostConfig
            {
                // TODO SSH This sets ss-pid/ss-opt to NOT HttpOnly.. is this a security issue?
                AllowNonHttpOnlyCookies = true,
                DebugMode = appSettings.Get("Debug", false)
            });

            this.RegisterTypedRequestFilter<ICaptchaRequest>(Filter.Captcha);
            this.RegisterTypedRequestFilter<EnterMatchAppointmentRequest>(Filter.EnterMatchAppointment);
            this.RegisterTypedRequestFilter<EnterMatchResultRequest>(Filter.EnterMatchResult);

            this.RegisterTypedResponseFilter<TeamDto>(Filter.TeamPlayerDetails);

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