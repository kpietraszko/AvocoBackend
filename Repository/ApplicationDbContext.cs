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
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EventJoinedUser>()
				.HasKey(e => new { e.EventId, e.UserId });

			modelBuilder.Entity<Friend>()
				.HasKey(f => new { f.User1Id, f.SecondUserId });
			//modelBuilder.Entity<Friend>()
			//	.HasOne(f => f.User1)
			//	.WithMany(u => u.FriendsInvited)
			//	.HasForeignKey(f => f.User1Id)
			//	.OnDelete(DeleteBehavior.SetNull);

			//modelBuilder.Entity<Friend>()
			//	.HasOne(f => f.User2)
			//	.WithMany(u => u.Friends)
			//	.HasForeignKey(f => f.User2Id)
			//	.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<GroupInterest>()
				.HasKey(g => new { g.GroupId, g.InterestId });

			modelBuilder.Entity<GroupJoinedUser>()
				.HasKey(g => new { g.GroupId, g.UserId });

			modelBuilder.Entity<UserInterest>()
				.HasKey(u => new { u.UserId, u.InterestId });


		}

		public DbSet<User> Users{ get; set;}
		public DbSet<Group> Groups{ get; set;}
		public DbSet<GroupJoinedUser> GroupsJoinedUsers{ get; set;}
		public DbSet<Message> Messages{ get; set;}
		public DbSet<Event> Events{ get; set;}
		public DbSet<EventJoinedUser> EventsJoinedUsers{ get; set;}
		public DbSet<Friend> Friends{ get; set;}
		public DbSet<Post> Posts{ get; set;}
		public DbSet<PostComment> PostsComments{ get; set;}
		public DbSet<Interest> Interests{ get; set;}
		public DbSet<UserInterest> UsersInterests{ get; set;}
		public DbSet<GroupInterest> GroupsInterests{ get; set;}

	}
}
