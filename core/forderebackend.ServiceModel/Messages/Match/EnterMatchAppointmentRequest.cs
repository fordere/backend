using System;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Match
{
    [Route("/matches/{Id}/appointment", "PUT")]
    
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