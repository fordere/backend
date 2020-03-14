using System.Linq;
using forderebackend.ServiceInterface.Entities;
using forderebackend.ServiceInterface.Entities.Forum;
using forderebackend.ServiceModel.Dtos;
using ServiceStack;

namespace forderebackend.ServiceInterface
{
    public static class Mapping
    {
        public static TableDto ToDto(this Table table)
        {
            return new TableDto
            {
                Id = table.Id,
                TableType = table.TableType.ToString(),
                Name = table.Name,
                BarName = table.Bar != null ? table.Bar.Name : string.Empty
            };
        }

        public static TableAvailabilityDto ToDto(this TableAvailability availability)
        {
            return new TableAvailabilityDto
            {
                FirstTimeSlot = availability.FirstTimeSlot,
                FirstTimeSlotDayOfWeek = availability.FirstTimeSlotDayOfWeek,
                LastTimeSlot = availability.LastTimeSlot,
                LastTimeSlotDayOfWeek = availability.LastTimeSlotDayOfWeek,
                Id = availability.Id
            };
        }

        public static TeamDto ToDto(this Team team)
        {
            return new TeamDto
            {
                Id = team.Id,
                BarId = team.BarId,
                Player1 = team.Player1.ToDto(),
                Player2 = team.Player2.ToDto(),
                Player1Id = team.Player1Id,
                Player2Id = team.Player2Id,
                Name = team.Name,
                SeasonAmbition = team.SeasonAmbition,
                IsForfaitOut = team.IsForfaitOut,
                WishPlayDay = team.WishPlayDay,
                League = team.LeagueId.HasValue ? new LeagueDto
                {
                    Id = team.LeagueId.Value
                } : null
            };
        }

        public static ForumThreadDto ToDto(this ForumThread thread)
        {
            var dto = thread.ConvertTo<ForumThreadDto>();

            var firstPost = thread.ForumPosts.OrderBy(x => x.Date).First();
            dto.NumberOfPosts = thread.ForumPosts.Count;
            dto.ThreadStartDate = firstPost.Date;
            dto.ThreadStartUser = firstPost.UserAuth?.FullName;

            var lastPost = thread.ForumPosts.OrderBy(x => x.Date).Last();
            dto.LastActivityDate = lastPost.Date;
            dto.LastActivityUser = string.Format("{0} {1}", lastPost.UserAuth?.FirstName, lastPost.UserAuth?.LastName);

            return dto;
        }
    }
}
