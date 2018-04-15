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
					new Interest
					{
						InterestName = "Filmy"
					},
					new Interest { InterestName = "Zwierzęta" },
					new Interest { InterestName = "Gotowanie" },
					new Interest { InterestName = "Fotografia" });
				context.SaveChanges();
			}
			if(!context.Groups.Any())
			{
				context.Groups.AddRange(
					new Group { GroupName = "Militaria"},
					new Group { GroupName = "Gotowanie w lesie"});
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
			if(!context.GroupsJoinedUsers.Any())
			{
				context.GroupsJoinedUsers.AddRange(new GroupJoinedUser { UserId = 6, GroupId = 1 },
					new GroupJoinedUser { UserId = 6, GroupId = 2});
				context.SaveChanges();
			}
		}
}
}
