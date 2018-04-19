using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AvocoBackend.Repository.Migrations
{
    public partial class groupImagePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImageSmallPath",
                table: "Users",
                newName: "ImageSmallPath");

            migrationBuilder.RenameColumn(
                name: "ProfileImagePath",
                table: "Users",
                newName: "ImagePath");

            migrationBuilder.AlterColumn<string>(
                name: "GroupPicture",
                table: "Groups",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageSmallPath",
                table: "Users",
                newName: "ProfileImageSmallPath");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Users",
                newName: "ProfileImagePath");

            migrationBuilder.AlterColumn<byte[]>(
                name: "GroupPicture",
                table: "Groups",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
