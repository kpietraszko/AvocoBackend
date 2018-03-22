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

		public DbSet<User> Users;
		public DbSet<Group> Groups;
		public DbSet<GroupJoinedUser> GroupsJoinedUsers;
		public DbSet<Message> Messages;
		public DbSet<Event> Events;
		public DbSet<EventJoinedUser> EventsJoinedUsers;
		public DbSet<Friend> Friends;
		public DbSet<Post> Posts;
		public DbSet<PostComment> PostsComments;
		public DbSet<Interest> Interests;
		public DbSet<UserInterest> UsersInterests;
		public DbSet<GroupInterest> GroupsInterests;

    }
}
