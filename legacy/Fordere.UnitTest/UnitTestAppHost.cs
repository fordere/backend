using Fordere.RestService;

using Funq;

using ServiceStack;
using ServiceStack.Caching;

namespace Fordere.UnitTest
{
    public class UnitTestAppHost : AppHostHttpListenerBase
    {
        public UnitTestAppHost()
            : base("Fordere UnitTest WebService", typeof(UserService).Assembly)
        {
        }

        public override void Configure(Container container)
        {
            container.Register<ICacheClient>(
                                             new MemoryCacheClient
                                             {
                                                 FlushOnDispose = false
                                             });

            this.Plugins.Add(new SessionFeature());

            SetConfig(new HostConfig { DebugMode = true });
        }
    }
}
