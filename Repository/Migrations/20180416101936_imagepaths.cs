using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AvocoBackend.Repository.Migrations
{
    public partial class imagepaths : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageSmallPath",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfileImageSmallPath",
                table: "Users");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImage",
                table: "Users",
                nullable: true);
        }
    }
}
