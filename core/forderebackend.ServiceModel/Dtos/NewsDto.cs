using System;
using System.Collections.Generic;

namespace forderebackend.ServiceModel.Dtos
{
    public class NewsDto
    {
        public NewsDto()
        {
            User = new UserDto();
        }

        public string Id { get; set; }
        public UserDto User { get; set; }
        public DateTime PostDate { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public int? DivisionId { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1} [{2}]", PostDate.ToString("d"), Title, Id);
        }
    }

    public class StatisticsDto
    {
        public LineStatistic MatchesPerWeek { get; set; }

        public LineStatistic MatchesPerWeekday { get; set; }

        public DonoughtStatistic MatchesPerLocation { get; set; }

        public DonoughtStatistic MatchesPlayed { get; set; }
    }

    public class LineStatistic
    {
        public List<string> Labels { get; set; }

        public List<string> Series { get; set; }

        public List<List<int>> Data { get; set; }
    }

    public class BarStatistics
    {
        public List<string> Labels { get; set; }

        public List<List<int>> Data { get; set; }
    }

    public class DonoughtStatistic
    {
        public List<string> Labels { get; set; }

        public List<int> Data { get; set; }
    }
}