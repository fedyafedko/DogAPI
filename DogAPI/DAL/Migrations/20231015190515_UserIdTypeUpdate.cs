using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassLibrary1.Migrations
{
    /// <inheritdoc />
    public partial class UserIdTypeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename Id to OldId
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "OldId");

            // Add new Guid column
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");            
            
            migrationBuilder.AddPrimaryKey(
                name: "PK_UserId", 
                table: "Users", 
                column: "Id");
            
            // Drop old Id
            migrationBuilder.DropColumn(
                name: "OldId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Id", "Users");
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false);
        }
    }
}
