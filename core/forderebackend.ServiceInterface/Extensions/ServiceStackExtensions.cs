using ServiceStack;

namespace Fordere.RestService.Extensions
{
    public static class ServiceStackExtensions
    {
        public static void Throw404NotFoundIfNull(this object target, string message)
        {
            if (target == null)
            {
                throw HttpError.NotFound(message);
            }
        }
    }
}
