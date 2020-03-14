using System;

using Fordere.ServiceInterface.Messages.Match;

using ServiceStack;

namespace Fordere.RestService
{
    public class KozoomService : BaseService
    {
        public async void Post(UpdateKozoomMatchRequest request)
        {
            string urlparams = QueryStringSerializer.SerializeToString<object>(request);
            string result = await "http://h2620130.stratoserver.net/livescore".PostToUrlAsync(urlparams);
            Console.WriteLine(result);
        }
    }
}