using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AvocoBackend.Repository.Migrations
{
    public partial class eventcomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventComment_Events_EventId",
                table: "EventComment");

            migrationBuilder.DropForeignKey(
                name: "FK_EventComment_Users_UserId",
                table: "EventComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventComment",
                table: "EventComment");

            migrationBuilder.RenameTable(
                name: "EventComment",
                newName: "EventComments");

            migrationBuilder.RenameIndex(
                name: "IX_EventComment_UserId",
                table: "EventComments",
                newName: "IX_EventComments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EventComment_EventId",
                table: "EventComments",
                newName: "IX_EventComments_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventComments",
                table: "EventComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventComments_Events_EventId",
                table: "EventComments",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventComments_Users_UserId",
                table: "EventComments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventComments_Events_EventId",
                table: "EventComments");

            migrationBuilder.DropForeignKey(
                name: "FK_EventComments_Users_UserId",
                table: "EventComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventComments",
                table: "EventComments");

            migrationBuilder.RenameTable(
                name: "EventComments",
                newName: "EventComment");

            migrationBuilder.RenameIndex(
                name: "IX_EventComments_UserId",
                table: "EventComment",
                newName: "IX_EventComment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EventComments_EventId",
                table: "EventComment",
                newName: "IX_EventComment_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventComment",
                table: "EventComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventComment_Events_EventId",
                table: "EventComment",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventComment_Users_UserId",
                table: "EventComment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
