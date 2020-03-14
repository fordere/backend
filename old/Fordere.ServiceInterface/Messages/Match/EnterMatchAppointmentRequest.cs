using System;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.Match
{
    [Route("/matches/{Id}/appointment", "PUT")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class EnterMatchAppointmentRequest : IReturn<ExtendedMatchDto>
    {
        [ApiMember(Name = "Id", Description = "Id of the match", ParameterType = "path", DataType = "int", IsRequired = true)]
        public int Id { get; set; }

        [ApiMember(Name = "Id", Description = "Id of the table", ParameterType = "model", DataType = "int", IsRequired = true)]
        public int? TableId { get; set; }

        [ApiMember(Name = "PlayDate", Description = "Play date", ParameterType = "model", DataType = "datetime", IsRequired = true)]
        public DateTime? PlayDate { get; set; }
    }
}