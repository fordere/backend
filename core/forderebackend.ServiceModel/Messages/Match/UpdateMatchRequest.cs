using System;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceModel.Messages.Match
{
    [Route("/matches/{Id}", "PUT")]
    
    public class UpdateMatchRequest : IReturn<MatchViewDto>
    {
        public int Id { get; set; }

        public int HomeTeamScore { get; set; }

        public int GuestTeamScore { get; set; }

        public int TableId { get; set; }

        public DateTime PlayDate { get; set; }
    }
}