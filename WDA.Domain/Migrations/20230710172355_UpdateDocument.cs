using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WDA.Domain.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_AspNetRoles_RoleId",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_RoleId",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Positions");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId",
                table: "Attachments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_DocumentId",
                table: "Attachments",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Documents_DocumentId",
                table: "Attachments",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "DocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Documents_DocumentId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_DocumentId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Attachments");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Positions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Positions_RoleId",
                table: "Positions",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_AspNetRoles_RoleId",
                table: "Positions",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
