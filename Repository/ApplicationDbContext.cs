using AvocoBackend.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class ApplicationDbContext :DbContext
    {
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			:base(options)
		{
		}
		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AvocoBackend;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
		//}

		public DbSet<TestModel> TestModels;
		//public DbSet<User> Users;
		//public DbSet<Group> Groups;
		//public DbSet<GroupJoinedUser> GroupsJoinedUsers;
		//public DbSet<Message> Messages;
		//public DbSet<Event> Events;
		//public DbSet<EventJoinedUser> EventsJoinedUsers;
		//public DbSet<Friend> Friends;
		//public DbSet<Post> Posts;
		//public DbSet<PostComment> PostsComments;
		//public DbSet<Interest> Interests;
		//public DbSet<UserInterest> UsersInterests;
		//public DbSet<GroupInterest> GroupsInterests;

    }
	public class TestModel
	{
		public int TestModelId { get; set; }
		public string Name { get; set; }
	}
}
