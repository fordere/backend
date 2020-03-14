using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.News
{
    [Route("/news/{Id}", "PATCH", Summary = "Patch update IsPublished.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class UpdateIsPublishedRequest : IReturnVoid
    {
        public int Id { get; set; }
        public bool IsPublished { get; set; }
    }
}