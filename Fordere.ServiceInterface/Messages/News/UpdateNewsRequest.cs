using System;

using Fordere.ServiceInterface.Annotations;
using Fordere.ServiceInterface.Dtos;

using ServiceStack;

namespace Fordere.ServiceInterface.Messages.News
{
    [Route("/news", "PUT", Summary = "Update a news.")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class UpdateNewsRequest : IReturn<NewsDto>
    {
        public int Id { get; set; }
        public NameDto User { get; set; }
        public DateTime PostDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
    }

    [Route("/competitions", "PUT", Summary = "Update a competition.")]
    public class UpdateCompetitionRequest : IReturn<CompetitionDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegistrationText { get; set; }
        public string Rules { get; set; }
        public string Modus { get; set; }
    }

    [Route("/competitions", "POST", Summary = "Create a new competition")]
    public class AddCompetitionRequest : IReturn<CompetitionDto>
    {
        public string Name { get; set; }
        public string RegistrationText { get; set; }
        public string Rules { get; set; }
        public string Modus { get; set; }
        public int SeasonId { get; set; }
    }
}