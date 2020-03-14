//using System;
//using System.Collections.Generic;
//using System.Linq;

//using Fordere.RestService.Entities;

//using Raven.Abstractions.Extensions;
//using Raven.Client;
//using Raven.Client.Document;

//using ServiceStack.Auth;

//namespace Fordere.UnitTest.Data
//{
//    public class DbSeeder
//    {
//        private readonly DocumentStore documentStore;

//        public DbSeeder(DocumentStore documentStore)
//        {
//            this.documentStore = documentStore;
//        }

//        public void Seed()
//        {
//            using (var bulkInsert = this.documentStore.BulkInsert())
//            {
//                this.GetUsers().ForEach(e => bulkInsert.Store(e));
//                this.GetTeams().ForEach(e => bulkInsert.Store(e));
//                this.GetLeagues().ForEach(e => bulkInsert.Store(e));
//                this.GetMatches().ForEach(e => bulkInsert.Store(e));
//                this.GetNews().ForEach(e => bulkInsert.Store(e));
//                this.GetBars().ForEach(e => bulkInsert.Store(e));
//                this.GetLeagueRegistrations().ForEach(e => bulkInsert.Store(e));
//            }

//            using (var session = this.documentStore.OpenSession())
//            {
//                RavenQueryStatistics stats;

//                session.Query<Bar>().Statistics(out stats).Customize(x => x.WaitForNonStaleResults()).ToArray();
//                session.Query<Cup>().Statistics(out stats).Customize(x => x.WaitForNonStaleResults()).ToArray();
//                session.Query<League>().Statistics(out stats).Customize(x => x.WaitForNonStaleResults()).ToArray();
//                session.Query<News>().Statistics(out stats).Customize(x => x.WaitForNonStaleResults()).ToArray();
//                session.Query<LeagueRegistration>().Statistics(out stats).Customize(x => x.WaitForNonStaleResults()).ToArray();
//                session.Query<Match>().Statistics(out stats).Customize(x => x.WaitForNonStaleResults()).ToArray();
//                session.Query<News>().Statistics(out stats).Customize(x => x.WaitForNonStaleResults()).ToArray();
//                session.Query<Season>().Statistics(out stats).Customize(x => x.WaitForNonStaleResults()).ToArray();
//                session.Query<Team>().Statistics(out stats).Customize(x => x.WaitForNonStaleResults()).ToArray();
//                session.Query<TeamRegistration>().Statistics(out stats).Customize(x => x.WaitForNonStaleResults()).ToArray();
//                session.Query<User>().Statistics(out stats).Customize(x => x.WaitForNonStaleResults()).ToArray();
//            }
//        }

//        private IEnumerable<Bar> GetBars()
//        {
//            return new[]
//            {
//                new Bar
//                {
//                    Name = "Bar 1"
//                },
//                new Bar
//                {
//                    Name = "Bar 2"
//                },
//                new Bar
//                {
//                    Name = "Bar 3"
//                }
//            };
//        }

//        private const string user1Id = "users/1";
//        private const string user2Id = "users/2";
//        private const string user3Id = "users/3";
//        private const string user4Id = "users/4";
//        private const string user5Id = "users/5";
//        private const string user6Id = "users/6";

//        private IEnumerable<User> GetUsers()
//        {
//            string hash;
//            string salt;
//            new SaltedHash().GetHashAndSaltString("password", out hash, out salt);

//            return new[]
//            {
//                new User
//                {
//                    FirstName = "Oliver",
//                    LastName = "Zürcher",
//                    EMail = "oliver.zuercher@gmail.com",
//                    Password = hash,
//                    Salt = salt,
//                    Roles = new List<string>
//                    {
//                        "Admin",
//                    }
//                },
//                new User
//                {
//                    FirstName = "Stefan",
//                    LastName = "Schöb",
//                    EMail = "opendix@gmail.com",
//                    Password = hash,
//                    Salt = salt,
//                    Roles = new List<string>
//                    {
//                        "Admin",
//                    }
//                },
//                new User
//                {
//                    FirstName = "Marcel",
//                    LastName = "Frischknecht",
//                    EMail = "marcel.frischknecht@gmx.ch",
//                    Password = hash,
//                    Salt = salt,
//                },
//                new User
//                {
//                    FirstName = "Test",
//                    LastName = "User 4",
//                    EMail = "test@test.com",
//                    Password = hash,
//                    Salt = salt,
//                },
//                new User
//                {
//                    FirstName = "Test",
//                    LastName = "User 5",
//                    EMail = "test@test.com",
//                    Password = hash,
//                    Salt = salt,
//                },
//                new User
//                {
//                    FirstName = "Test",
//                    LastName = "User 6",
//                    EMail = "test@test.com",
//                    Password = hash,
//                    Salt = salt,
//                },
//            };
//        }

//        private const string team1Id = "teams/1";
//        private const string team2Id = "teams/2";
//        private const string team3Id = "teams/3";

//        private IEnumerable<Team> GetTeams()
//        {
//            return new[]
//            {
//                new Team
//                {
//                    Name = "Team1",
//                    UserIds = new List<string>
//                    {
//                        user1Id,
//                        user2Id
//                    }
//                },
//                new Team
//                {
//                    Name = "Team2",
//                    UserIds = new List<string>
//                    {
//                        user3Id,
//                        user4Id
//                    }
//                },
//                new Team
//                {
//                    Name = "Team 3",
//                    UserIds = new List<string>
//                    {
//                        user5Id,
//                        user6Id
//                    }
//                }
//            };
//        }

//        private const string league1Id = "leagues/1";
//        private const string league2Id = "leagues/2";
//        private const string league3Id = "leagues/3";

//        private IEnumerable<League> GetLeagues()
//        {
//            return new[]
//            {
//                new League
//                {
//                    Id = league1Id,
//                    Name = "Season1"
//                },
//                new League
//                {
//                    Id = league2Id,
//                    Name = "Season2",
//                },
//                new League
//                {
//                    Id = league3Id,
//                    Name = "Season3",
//                }
//            };
//        }

//        private IEnumerable<Match> GetMatches()
//        {
//            return new[]
//            {
//                new Match
//                {
//                    PlayDate = new DateTime(2014, 1, 1, 20, 0, 0),
//                    GuestTeamId = team1Id,
//                    HomeTeamId = team2Id,
//                    HomeTeamScore = 4,
//                    GuestTeamScore = 0,
//                },
//                new Match
//                {
//                    PlayDate = new DateTime(2014, 1, 1, 20, 0, 0),
//                    GuestTeamId = team1Id,
//                    HomeTeamId = team3Id,
//                    HomeTeamScore = 2,
//                    GuestTeamScore = 3,
//                },
//                new Match
//                {
//                    PlayDate = new DateTime(2014, 1, 1, 20, 0, 0),
//                    GuestTeamId = team2Id,
//                    HomeTeamId = team3Id,
//                    HomeTeamScore = 1,
//                    GuestTeamScore = 3,
//                },
//                new Match
//                {
//                    PlayDate = new DateTime(2014, 1, 1, 20, 0, 0),
//                    GuestTeamId = team3Id,
//                    HomeTeamId = team1Id,
//                    HomeTeamScore = 0,
//                    GuestTeamScore = 4,
//                },
//            };
//        }

//        private IEnumerable<News> GetNews()
//        {
//            return new[]
//            {
//                new News
//                {
//                    Title = "some title",
//                    Content = "some content",
//                    IsPublished = true,
//                    PostDate = new DateTime(2014, 1, 1),
//                    UserId = "users/1"
//                },
//                new News
//                {
//                    Title = "some title2",
//                    Content = "some content2",
//                    IsPublished = true,
//                    PostDate = new DateTime(2014, 2, 1),
//                    UserId = "users/1"
//                },
//            };
//        }

//        private IEnumerable<LeagueRegistration> GetLeagueRegistrations()
//        {
//            return new[]
//                   {
//                       new LeagueRegistration
//                       {
//                           Id = "leagueregistrations/1",
//                           CloseDate = DateTime.Now.AddDays(7),
//                           CountSubLeagues = 5,
//                           IsVisible = false,
//                           Name = "Test Liga",
//                       },
//                       new LeagueRegistration
//                       {
//                           Id = "leagueregistrations/2",
//                           CloseDate = DateTime.Now.AddDays(-7),
//                           CountSubLeagues = 3,
//                           IsVisible = false,
//                           Name = "Test Registration expired"
//                       },
//                       new LeagueRegistration
//                       {
//                           Id = "leagueregistrations/3",
//                           CloseDate = DateTime.Now.AddDays(7),
//                           CountSubLeagues = 6,
//                           IsVisible = true,
//                           Name = "Test Liga 2"
//                       },
//                       new LeagueRegistration
//                       {
//                           Id = "leagueregistrations/4",
//                           CloseDate = DateTime.Now.AddDays(7),
//                           CountSubLeagues = 3,
//                           IsVisible = true,
//                           Name = "Test Liga 3",
//                           TeamRegistrations = new List<TeamRegistration>
//                                               {
//                                                   new TeamRegistration
//                                                   {
//                                                       Name = "Tätsch Bäng Mereng",
//                                                       LeagueWish = 2,
//                                                       UserIds = new List<string> { "users/1", "users/2" }
//                                                   }
//                                               }
//                       },
//                       new LeagueRegistration
//                       {
//                           Id = "leagueregistrations/5",
//                           CloseDate = DateTime.Now.AddDays(-7),
//                           CountSubLeagues = 3,
//                           IsVisible = true,
//                           Name = "Visible but expired"
//                       },
//                       new LeagueRegistration
//                       {
//                           Id = "leagueregistrations/6",
//                           CloseDate = DateTime.Now.AddDays(-7),
//                           CountSubLeagues = 3,
//                           IsVisible = true,
//                           Name = "Not visible but open"
//                       }
//                   };
//        }
//    }
//}
