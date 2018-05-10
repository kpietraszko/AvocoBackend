using AvocoBackend.Data.Models;
using AvocoBackend.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvocoBackend.Repository
{
	public static class DbInitializer
	{
		public static void Seed(ApplicationDbContext context)
		{
			context.Database.EnsureCreated();
			if (!context.Interests.Any())
			{
				context.Interests.AddRange(
					new Interest { InterestName = "Filmy" },
					new Interest { InterestName = "Zwierzęta" },
					new Interest { InterestName = "Gotowanie" },
					new Interest { InterestName = "Fotografia" },
                    new Interest { InterestName = "Wojskowosc" },
                    new Interest { InterestName = "Muzyka" },
                    new Interest { InterestName = "Programiowanie" },
                    new Interest { InterestName = "Podroze" },
                    new Interest { InterestName = "Motoryzacja" },
                    new Interest { InterestName = "Zielarstwo" },
                    new Interest { InterestName = "Bieganie" });

                context.SaveChanges();
			}
			if (!context.Groups.Any())
			{
				context.Groups.AddRange(
					new Group { GroupName = "Militaria" },
					new Group { GroupName = "Gotowanie w lesie" },
                    new Group { GroupName = "Biegacze w Olsztynie" },
                    new Group { GroupName = "Poczatkujacy programisci" },
                    new Group { GroupName = "Fotografowie przyrody" });
                context.SaveChanges();
			}
			if (!context.UsersInterests.Any())
			{
				var ui = new UserInterest
				{
					Interest = context.Interests.First(i => i.Id == 1),
					User = context.Users.First(u => u.Id == 6)
				};
				context.UsersInterests.Add(ui);
				context.SaveChanges();
			}
			if (!context.Friends.Any())
			{
				context.Friends.Add(new Friend { User1Id = 6, User2Id = 1 });
				context.Friends.Add(new Friend { User1Id = 6, User2Id = 8 });
			}
			context.SaveChanges();
			if (!context.GroupsJoinedUsers.Any())
			{
                context.GroupsJoinedUsers.AddRange(
                    new GroupJoinedUser { UserId = 6, GroupId = 1 },
                    new GroupJoinedUser { UserId = 6, GroupId = 2 },
                    new GroupJoinedUser { UserId = 6, GroupId = 3 },
                    new GroupJoinedUser { UserId = 6, GroupId = 4 },
                    new GroupJoinedUser { UserId = 6, GroupId = 5 });
				context.SaveChanges();
			}
			if (!context.Events.Any())
			{
				context.Events.Add(new Event { GroupId = 1, EventName = "ASG w terenie", EventDescription = "Strzelanie z kulek do ludzi", EventDateTime = new DateTime(2018, 5, 29, 16, 0, 0), EventLocationLat = 53.777367, EventLocationLng = 20.484959 });
                context.Events.Add(new Event { GroupId = 9, EventName = "Defilada", EventDateTime = new DateTime(2018, 7, 17, 12, 30, 0), EventLocationLat = 53.677367, EventLocationLng = 20.684959 });
                context.Events.Add(new Event { GroupId = 8, EventName = "Pokaz broni", EventDateTime = new DateTime(2018, 8, 1, 8, 30, 0), EventLocationLat = 53.777367, EventLocationLng = 20.684959 });
                context.Events.Add(new Event { GroupId = 2, EventName = "Maraton filmowy", EventDateTime = new DateTime(2018, 6, 21, 22, 20, 0), EventLocationLat = 53.477367, EventLocationLng = 20.284959 });
                context.Events.Add(new Event { GroupId = 3, EventName = "Szkolenie z pythona", EventDateTime = new DateTime(2018, 7, 11, 11, 0, 0), EventLocationLat = 53.977367, EventLocationLng = 20.384959 });
                context.Events.Add(new Event { GroupId = 4, EventName = "Sesja zdjeciowa", EventDateTime = new DateTime(2018, 6, 1, 8, 0, 0), EventLocationLat = 53.577367, EventLocationLng = 20.584959 });
                context.Events.Add(new Event { GroupId = 5, EventName = "Bieg wokol jeziora", EventDateTime = new DateTime(2018, 7, 20, 14, 0, 0), EventLocationLat = 52.777367, EventLocationLng = 21.484959 });
                context.Events.Add(new Event { GroupId = 6, EventName = "Wspolne granie na stadionie", EventDateTime = new DateTime(2018, 7, 11, 20, 0, 0), EventLocationLat = 53.447367, EventLocationLng = 20.224959 });
                context.Events.Add(new Event { GroupId = 7, EventName = "Prelekcja o ziolach bagiennych", EventDateTime = new DateTime(2018, 6, 20, 13, 0, 0), EventLocationLat = 53.677367, EventLocationLng = 20.784959 });
                context.SaveChanges();
            }
            /*
            if (!context.Posts.Any()) //tudu
            {
                context.Posts.Add(new Post {GroupId = 1, UserId = 6, Content = "tesasdasdsadasdasdasd" });
                context.SaveChanges();
            }*/
		}
	}
}

// uzywac uzytkowników 1,3,6 
// 6 - jan
// 1 - krzysiek
// 3 - joanna
// 1 - militaria