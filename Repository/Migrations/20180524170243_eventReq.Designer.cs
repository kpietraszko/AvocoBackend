﻿// <auto-generated />
using AvocoBackend.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace AvocoBackend.Repository.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180524170243_eventReq")]
    partial class eventReq
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AvocoBackend.Data.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EventDateTime");

                    b.Property<string>("EventDescription");

                    b.Property<double?>("EventLocationLat");

                    b.Property<double?>("EventLocationLng");

                    b.Property<string>("EventName")
                        .IsRequired();

                    b.Property<int>("GroupId");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.EventComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<int>("EventId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("EventComment");
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.EventJoinedUser", b =>
                {
                    b.Property<int>("EventId");

                    b.Property<int>("UserId");

                    b.HasKey("EventId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("EventsJoinedUsers");
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.Friend", b =>
                {
                    b.Property<int>("User1Id");

                    b.Property<int>("User2Id");

                    b.HasKey("User1Id", "User2Id");

                    b.HasIndex("User2Id");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GroupDescription");

                    b.Property<string>("GroupName");

                    b.Property<string>("GroupPicture");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.GroupInterest", b =>
                {
                    b.Property<int>("GroupId");

                    b.Property<int>("InterestId");

                    b.HasKey("GroupId", "InterestId");

                    b.HasIndex("InterestId");

                    b.ToTable("GroupsInterests");
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.GroupJoinedUser", b =>
                {
                    b.Property<int>("GroupId");

                    b.Property<int>("UserId");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupsJoinedUsers");
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.Interest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("InterestName");

                    b.HasKey("Id");

                    b.ToTable("Interests");
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MessageContent");

                    b.Property<int>("RecipientUserId");

                    b.Property<int>("SenderUserId");

                    b.HasKey("Id");

                    b.HasIndex("RecipientUserId");

                    b.HasIndex("SenderUserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<int>("GroupId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.PostComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<int>("PostId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("PostsComments");
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("ImagePath");

                    b.Property<string>("ImageSmallPath");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("PasswordHash")
                        .IsRequired();

                    b.Property<int?>("Region");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.UserInterest", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("InterestId");

                    b.HasKey("UserId", "InterestId");

                    b.HasIndex("InterestId");

                    b.ToTable("UsersInterests");
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.Event", b =>
                {
                    b.HasOne("AvocoBackend.Data.Models.Group", "Group")
                        .WithMany("Events")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.EventComment", b =>
                {
                    b.HasOne("AvocoBackend.Data.Models.Event", "Event")
                        .WithMany("EventComments")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AvocoBackend.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.EventJoinedUser", b =>
                {
                    b.HasOne("AvocoBackend.Data.Models.Event", "Event")
                        .WithMany("EventJoinedUsers")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AvocoBackend.Data.Models.User", "User")
                        .WithMany("EventsJoinedUser")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.Friend", b =>
                {
                    b.HasOne("AvocoBackend.Data.Models.User", "User1")
                        .WithMany("FriendsInvited")
                        .HasForeignKey("User1Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AvocoBackend.Data.Models.User", "User2")
                        .WithMany("FriendsInvitedBy")
                        .HasForeignKey("User2Id")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.GroupInterest", b =>
                {
                    b.HasOne("AvocoBackend.Data.Models.Group", "Group")
                        .WithMany("GroupInterests")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AvocoBackend.Data.Models.Interest", "Interest")
                        .WithMany("GroupsInterest")
                        .HasForeignKey("InterestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.GroupJoinedUser", b =>
                {
                    b.HasOne("AvocoBackend.Data.Models.Group", "Group")
                        .WithMany("GroupJoinedUsers")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AvocoBackend.Data.Models.User", "User")
                        .WithMany("GroupsJoinedUser")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.Message", b =>
                {
                    b.HasOne("AvocoBackend.Data.Models.User", "RecipientUser")
                        .WithMany("ReceivedMessages")
                        .HasForeignKey("RecipientUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AvocoBackend.Data.Models.User", "SenderUser")
                        .WithMany("SentMessages")
                        .HasForeignKey("SenderUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.Post", b =>
                {
                    b.HasOne("AvocoBackend.Data.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AvocoBackend.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.PostComment", b =>
                {
                    b.HasOne("AvocoBackend.Data.Models.Post", "Post")
                        .WithMany("PostComments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AvocoBackend.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AvocoBackend.Data.Models.UserInterest", b =>
                {
                    b.HasOne("AvocoBackend.Data.Models.Interest", "Interest")
                        .WithMany("UsersInterest")
                        .HasForeignKey("InterestId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AvocoBackend.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
