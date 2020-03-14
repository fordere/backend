using System;

using Fordere.ServiceInterface.Annotations;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Challenge
{
    [Route("/challenges", "POST")]
    [UsedImplicitly]
    public class CreateChallengeRequest : IReturnVoid
    {
        public int TeamId { get; set; }

        public DateTime ProposedDate { get; set; }

        public int TableId { get; set; }
    }
}