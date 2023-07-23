using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WDA.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePictureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    PasswordChangeRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    EmailTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailTemplateType = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.EmailTemplateId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_Attachments_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attachments_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTickets",
                columns: table => new
                {
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TicketCategory = table.Column<int>(type: "int", nullable: false),
                    ReopenReasons = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResolverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_EmployeeTickets_AspNetUsers_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeTickets_AspNetUsers_ResolverId",
                        column: x => x.ResolverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerTickets",
                columns: table => new
                {
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TicketCategory = table.Column<int>(type: "int", nullable: false),
                    ReopenReasons = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResolverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_CustomerTickets_AspNetUsers_ResolverId",
                        column: x => x.ResolverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerTickets_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_CustomerTickets_Customers_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), null, "Admin", "Admin" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), null, "Hr", "Hr" },
                    { new Guid("20000000-0000-0000-0000-000000000003"), null, "Sale", "Sale" },
                    { new Guid("20000000-0000-0000-0000-000000000004"), null, "HrManager", "HrManager" },
                    { new Guid("20000000-0000-0000-0000-000000000005"), null, "SaleManager", "SaleManager" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Department", "Email", "EmailConfirmed", "FullName", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordChangeRequired", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfilePictureId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("10000000-0000-0000-0000-000000000001"), 0, "6a9943f8-af5b-4231-9a8d-63f8c43c6e0c", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "admin@email.com", true, "System Admin", false, false, null, "admin@email.com", "admin", false, "AQAAAAIAAYagAAAAEMeRmOWs9W/KsBTc0NEYwk5Efsp1rjs48fPIPWSW0xhuuKWByjTRlnJXKrEmn9yPhA==", null, false, null, "3YYPM246ONSVZFAKY3TR2JSVKMX7ZM4D", false, "admin" });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "EmailTemplateId", "Body", "EmailTemplateType", "Subject" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), "<div style=\"word-spacing: normal; background-color: #eeeeee; font-family: 'IBM Plex Sans', sans-serif\"> <div style=\"background-color: #eeeeee\"> <div style=\"background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px\" > <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\" > <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0px 24px 2px 24px; text-align: center\" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \" > <div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 22px 6px 15px 10px; word-break: break-word\" > <table cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; border-spacing: 0px\" > <tbody> <tr> <td style=\"width: 248px\"> <a href=\"/\" class=\"css-5k1n1y\" style=\"text-decoration: none; display: flex; align-items: center\" > <svg width=\"30\" height=\"25\" version=\"1.1\" viewBox=\"0 0 30 23\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" > <g stroke=\"none\" stroke-width=\"1\" fill=\"none\" fill-rule=\"evenodd\" > <g id=\"Artboard\" transform=\"translate(-95.000000, -51.000000)\" > <g id=\"logo\" transform=\"translate(95.000000, 50.000000)\" > <path id=\"Combined-Shape\" fill=\"#9155FD\" d=\"M30,21.3918362 C30,21.7535219 29.9019196,22.1084381 29.7162004,22.4188007 C29.1490236,23.366632 27.9208668,23.6752135 26.9730355,23.1080366 L26.9730355,23.1080366 L23.714971,21.1584295 C23.1114106,20.7972624 22.7419355,20.1455972 22.7419355,19.4422291 L22.7419355,19.4422291 L22.741,12.7425689 L15,17.1774194 L7.258,12.7425689 L7.25806452,19.4422291 C7.25806452,20.1455972 6.88858935,20.7972624 6.28502902,21.1584295 L3.0269645,23.1080366 C2.07913318,23.6752135 0.850976404,23.366632 0.283799571,22.4188007 C0.0980803893,22.1084381 2.0190442e-15,21.7535219 0,21.3918362 L0,3.58469444 L0.00548573643,3.43543209 L0.00548573643,3.43543209 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 L15,9.19354839 L26.9548759,1.86636639 C27.2693965,1.67359571 27.6311047,1.5715689 28,1.5715689 C29.1045695,1.5715689 30,2.4669994 30,3.5715689 L30,3.5715689 Z\" ></path> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.7505183 7.25806452 16.8305646\" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.6445567 7.25806452 15.1370162\" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.7417372 30 16.9537453\" transform=\"translate(26.370968, 12.771227) scale(-1, 1) translate(-26.370968, -12.771227) \" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.6409734 30 15.2601969\" transform=\"translate(26.370968, 11.924453) scale(-1, 1) translate(-26.370968, -11.924453) \" ></polygon> <path id=\"Rectangle\" fill-opacity=\"0.15\" fill=\"#FFF\" d=\"M3.04512412,1.86636639 L15,9.19354839 L15,9.19354839 L15,17.1774194 L0,8.58649679 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 Z\" ></path> <path id=\"Rectangle\" fill-opacity=\"0.35\" fill=\"#FFF\" transform=\"translate(22.500000, 8.588710) scale(-1, 1) translate(-22.500000, -8.588710) \" d=\"M18.0451241,1.86636639 L30,9.19354839 L30,9.19354839 L30,17.1774194 L15,8.58649679 L15,3.5715689 C15,2.4669994 15.8954305,1.5715689 17,1.5715689 C17.3688953,1.5715689 17.7306035,1.67359571 18.0451241,1.86636639 Z\" ></path> </g> </g> </g> </svg> <div style=\" font-size: 21px; line-height: 18px; text-align: right; color: #9155fd; font-weight: 600; margin-left: 10px; \" > Company Active </div></a> </td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></tbody> </table> </div><div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\" > [[CreatedAt]] </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\" > Oder ID: [[TransactionId]] </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> </table> </div><div style=\" background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px; padding-bottom: 10px; \" > <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\" > <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 30px 32px 36px 32px; text-align: center\" > <div style=\"margin: 0px auto; max-width: 536px\"> <table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0; text-align: center\" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 100%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" > <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\" color: #000000; font-size: 13px; line-height: 22px; table-layout: auto; width: 100%; border: none; \" > <tbody> <tr> <td style=\"text-align: right\"> <div style=\"height: 0px; max-height: 0px; min-height: 0px\" > <img style=\"width: 140px; margin-right: 30px\" src=\"https://clipground.com/images/paid-in-full-stamp-clip-art-5.png\" alt=\"paid\" class=\"CToWUd\"/> </div></td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></tbody> </table> </div></div><div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: top; width: 100%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" > <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \" > Dear Customers <span style=\"font-weight: 700; color: #9155fd\" >[[CustomerFullName]]</span >, </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \" > Thank you for choosing to use our products! </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"height: 12px; line-height: 12px\">   </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: left; color: #3a2a2c\" > Your transaction is complete. Wishing you have a great experience! </div></td></tr></tbody> </table> </td></tr></tbody> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: left; color: #3a2a2c\" > For further support, please create a ticket by clicking the below button. </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></td></tr></tbody> </table> <div style=\"display: flex; justify-content: center\"> <button style=\"padding: 10px 30px; background-color: #9155fd; border: none\" > <a href=\"[[CreateTicketUrl]]\" style=\"text-decoration: none; color: white\"> <strong>Create Ticket</strong> </a> </button> </div></div></div></div>", 0, "Transaction No. [[TransactionId]] has been completed." },
                    { new Guid("30000000-0000-0000-0000-000000000002"), "<div style=\"word-spacing: normal; background-color: #eeeeee; font-family: 'IBM Plex Sans', sans-serif\"> <div style=\"background-color: #eeeeee\"> <div style=\"background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px\" > <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\" > <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0px 24px 2px 24px; text-align: center\" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \" > <div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 22px 6px 15px 10px; word-break: break-word\" > <table cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; border-spacing: 0px\" > <tbody> <tr> <td style=\"width: 248px\"> <a href=\"/\" class=\"css-5k1n1y\" style=\"text-decoration: none; display: flex; align-items: center\" > <svg width=\"30\" height=\"25\" version=\"1.1\" viewBox=\"0 0 30 23\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" > <g stroke=\"none\" stroke-width=\"1\" fill=\"none\" fill-rule=\"evenodd\" > <g id=\"Artboard\" transform=\"translate(-95.000000, -51.000000)\" > <g id=\"logo\" transform=\"translate(95.000000, 50.000000)\" > <path id=\"Combined-Shape\" fill=\"#9155FD\" d=\"M30,21.3918362 C30,21.7535219 29.9019196,22.1084381 29.7162004,22.4188007 C29.1490236,23.366632 27.9208668,23.6752135 26.9730355,23.1080366 L26.9730355,23.1080366 L23.714971,21.1584295 C23.1114106,20.7972624 22.7419355,20.1455972 22.7419355,19.4422291 L22.7419355,19.4422291 L22.741,12.7425689 L15,17.1774194 L7.258,12.7425689 L7.25806452,19.4422291 C7.25806452,20.1455972 6.88858935,20.7972624 6.28502902,21.1584295 L3.0269645,23.1080366 C2.07913318,23.6752135 0.850976404,23.366632 0.283799571,22.4188007 C0.0980803893,22.1084381 2.0190442e-15,21.7535219 0,21.3918362 L0,3.58469444 L0.00548573643,3.43543209 L0.00548573643,3.43543209 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 L15,9.19354839 L26.9548759,1.86636639 C27.2693965,1.67359571 27.6311047,1.5715689 28,1.5715689 C29.1045695,1.5715689 30,2.4669994 30,3.5715689 L30,3.5715689 Z\" ></path> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.7505183 7.25806452 16.8305646\" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.6445567 7.25806452 15.1370162\" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.7417372 30 16.9537453\" transform=\"translate(26.370968, 12.771227) scale(-1, 1) translate(-26.370968, -12.771227) \" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.6409734 30 15.2601969\" transform=\"translate(26.370968, 11.924453) scale(-1, 1) translate(-26.370968, -11.924453) \" ></polygon> <path id=\"Rectangle\" fill-opacity=\"0.15\" fill=\"#FFF\" d=\"M3.04512412,1.86636639 L15,9.19354839 L15,9.19354839 L15,17.1774194 L0,8.58649679 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 Z\" ></path> <path id=\"Rectangle\" fill-opacity=\"0.35\" fill=\"#FFF\" transform=\"translate(22.500000, 8.588710) scale(-1, 1) translate(-22.500000, -8.588710) \" d=\"M18.0451241,1.86636639 L30,9.19354839 L30,9.19354839 L30,17.1774194 L15,8.58649679 L15,3.5715689 C15,2.4669994 15.8954305,1.5715689 17,1.5715689 C17.3688953,1.5715689 17.7306035,1.67359571 18.0451241,1.86636639 Z\" ></path> </g> </g> </g> </svg> <div style=\" font-size: 21px; line-height: 18px; text-align: right; color: #9155fd; font-weight: 600; margin-left: 10px; \" > Company Active </div></a> </td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></tbody> </table> </div><div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\" > [[CreatedAt]] </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\" > Ticket ID: [[TicketId]] </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> </table> </div><div style=\"background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px\" > <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\" > <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 30px 32px 36px 32px; text-align: center\" > <div style=\"margin: 0px auto; max-width: 536px\"> <table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0; text-align: center\" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: top; width: 100%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" > <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \" > Dear <span style=\"font-weight: 700; color: #9155fd\" >[[RequestorFullName]]</span >, </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \" > Your ticket has been received by the system. </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \" > You'll receive a new notification when your ticket being assigned. </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"height: 12px; line-height: 12px\">   </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: left; color: #3a2a2c\" > This is an auto-generated email. Please do not reply. </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></td></tr></tbody> </table> </div></div></div>", 1, "Ticket No. [[TicketId]] has been opened." },
                    { new Guid("30000000-0000-0000-0000-000000000003"), "<div style=\"word-spacing: normal; background-color: #eeeeee; font-family: 'IBM Plex Sans', sans-serif\"> <div style=\"background-color: #eeeeee\"> <div style=\"background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px\" > <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\" > <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0px 24px 2px 24px; text-align: center\" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \" > <div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 22px 6px 15px 10px; word-break: break-word\" > <table cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; border-spacing: 0px\" > <tbody> <tr> <td style=\"width: 248px\"> <a href=\"/\" class=\"css-5k1n1y\" style=\"text-decoration: none; display: flex; align-items: center\" > <svg width=\"30\" height=\"25\" version=\"1.1\" viewBox=\"0 0 30 23\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" > <g stroke=\"none\" stroke-width=\"1\" fill=\"none\" fill-rule=\"evenodd\" > <g id=\"Artboard\" transform=\"translate(-95.000000, -51.000000)\" > <g id=\"logo\" transform=\"translate(95.000000, 50.000000)\" > <path id=\"Combined-Shape\" fill=\"#9155FD\" d=\"M30,21.3918362 C30,21.7535219 29.9019196,22.1084381 29.7162004,22.4188007 C29.1490236,23.366632 27.9208668,23.6752135 26.9730355,23.1080366 L26.9730355,23.1080366 L23.714971,21.1584295 C23.1114106,20.7972624 22.7419355,20.1455972 22.7419355,19.4422291 L22.7419355,19.4422291 L22.741,12.7425689 L15,17.1774194 L7.258,12.7425689 L7.25806452,19.4422291 C7.25806452,20.1455972 6.88858935,20.7972624 6.28502902,21.1584295 L3.0269645,23.1080366 C2.07913318,23.6752135 0.850976404,23.366632 0.283799571,22.4188007 C0.0980803893,22.1084381 2.0190442e-15,21.7535219 0,21.3918362 L0,3.58469444 L0.00548573643,3.43543209 L0.00548573643,3.43543209 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 L15,9.19354839 L26.9548759,1.86636639 C27.2693965,1.67359571 27.6311047,1.5715689 28,1.5715689 C29.1045695,1.5715689 30,2.4669994 30,3.5715689 L30,3.5715689 Z\" ></path> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.7505183 7.25806452 16.8305646\" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.6445567 7.25806452 15.1370162\" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.7417372 30 16.9537453\" transform=\"translate(26.370968, 12.771227) scale(-1, 1) translate(-26.370968, -12.771227) \" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.6409734 30 15.2601969\" transform=\"translate(26.370968, 11.924453) scale(-1, 1) translate(-26.370968, -11.924453) \" ></polygon> <path id=\"Rectangle\" fill-opacity=\"0.15\" fill=\"#FFF\" d=\"M3.04512412,1.86636639 L15,9.19354839 L15,9.19354839 L15,17.1774194 L0,8.58649679 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 Z\" ></path> <path id=\"Rectangle\" fill-opacity=\"0.35\" fill=\"#FFF\" transform=\"translate(22.500000, 8.588710) scale(-1, 1) translate(-22.500000, -8.588710) \" d=\"M18.0451241,1.86636639 L30,9.19354839 L30,9.19354839 L30,17.1774194 L15,8.58649679 L15,3.5715689 C15,2.4669994 15.8954305,1.5715689 17,1.5715689 C17.3688953,1.5715689 17.7306035,1.67359571 18.0451241,1.86636639 Z\" ></path> </g> </g> </g> </svg> <div style=\" font-size: 21px; line-height: 18px; text-align: right; color: #9155fd; font-weight: 600; margin-left: 10px; \" > Company Active </div></a> </td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></tbody> </table> </div><div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\" > [[CreatedAt]] </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\" > Ticket ID: [[TicketId]] </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> </table> </div><div style=\"background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px\" > <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\" > <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 30px 32px 36px 32px; text-align: center\" > <div style=\"margin: 0px auto; max-width: 536px\"> <table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0; text-align: center\" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: top; width: 100%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" > <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \" > Dear <span style=\"font-weight: 700; color: #9155fd\" >[[RequestorFullName]]</span >, </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \" > Your ticket has been assigned to staff <strong style=\"color: #9155fd\" >[[ResolverFullName]]</strong >. We will send an email notification when your ticket is being processed. </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> </table> </div></td></tr></tbody> </table> </div></div></div>", 2, "Ticket No. [[TicketId]] has been assigned." },
                    { new Guid("30000000-0000-0000-0000-000000000004"), "<div style=\"word-spacing: normal; background-color: #eeeeee; font-family: 'IBM Plex Sans', sans-serif\"> <div style=\"background-color: #eeeeee\"> <div style=\"background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px\" > <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\" > <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0px 24px 2px 24px; text-align: center\" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \" > <div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 22px 6px 15px 10px; word-break: break-word\" > <table cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; border-spacing: 0px\" > <tbody> <tr> <td style=\"width: 248px\"> <a href=\"/\" class=\"css-5k1n1y\" style=\"text-decoration: none; display: flex; align-items: center\" > <svg width=\"30\" height=\"25\" version=\"1.1\" viewBox=\"0 0 30 23\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" > <g stroke=\"none\" stroke-width=\"1\" fill=\"none\" fill-rule=\"evenodd\" > <g id=\"Artboard\" transform=\"translate(-95.000000, -51.000000)\" > <g id=\"logo\" transform=\"translate(95.000000, 50.000000)\" > <path id=\"Combined-Shape\" fill=\"#9155FD\" d=\"M30,21.3918362 C30,21.7535219 29.9019196,22.1084381 29.7162004,22.4188007 C29.1490236,23.366632 27.9208668,23.6752135 26.9730355,23.1080366 L26.9730355,23.1080366 L23.714971,21.1584295 C23.1114106,20.7972624 22.7419355,20.1455972 22.7419355,19.4422291 L22.7419355,19.4422291 L22.741,12.7425689 L15,17.1774194 L7.258,12.7425689 L7.25806452,19.4422291 C7.25806452,20.1455972 6.88858935,20.7972624 6.28502902,21.1584295 L3.0269645,23.1080366 C2.07913318,23.6752135 0.850976404,23.366632 0.283799571,22.4188007 C0.0980803893,22.1084381 2.0190442e-15,21.7535219 0,21.3918362 L0,3.58469444 L0.00548573643,3.43543209 L0.00548573643,3.43543209 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 L15,9.19354839 L26.9548759,1.86636639 C27.2693965,1.67359571 27.6311047,1.5715689 28,1.5715689 C29.1045695,1.5715689 30,2.4669994 30,3.5715689 L30,3.5715689 Z\" ></path> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.7505183 7.25806452 16.8305646\" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.6445567 7.25806452 15.1370162\" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.7417372 30 16.9537453\" transform=\"translate(26.370968, 12.771227) scale(-1, 1) translate(-26.370968, -12.771227) \" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.6409734 30 15.2601969\" transform=\"translate(26.370968, 11.924453) scale(-1, 1) translate(-26.370968, -11.924453) \" ></polygon> <path id=\"Rectangle\" fill-opacity=\"0.15\" fill=\"#FFF\" d=\"M3.04512412,1.86636639 L15,9.19354839 L15,9.19354839 L15,17.1774194 L0,8.58649679 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 Z\" ></path> <path id=\"Rectangle\" fill-opacity=\"0.35\" fill=\"#FFF\" transform=\"translate(22.500000, 8.588710) scale(-1, 1) translate(-22.500000, -8.588710) \" d=\"M18.0451241,1.86636639 L30,9.19354839 L30,9.19354839 L30,17.1774194 L15,8.58649679 L15,3.5715689 C15,2.4669994 15.8954305,1.5715689 17,1.5715689 C17.3688953,1.5715689 17.7306035,1.67359571 18.0451241,1.86636639 Z\" ></path> </g> </g> </g> </svg> <div style=\" font-size: 21px; line-height: 18px; text-align: right; color: #9155fd; font-weight: 600; margin-left: 10px; \" > Company Active </div></a> </td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></tbody> </table> </div><div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\" > [[CreatedAt]] </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\" > Ticket ID: [[TicketId]] </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> </table> </div><div style=\" background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px; padding-bottom: 10px; \" > <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\" > <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 30px 32px 36px 32px; text-align: center\" > <div style=\"margin: 0px auto; max-width: 536px\"> <table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0; text-align: center\" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: top; width: 100%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" > <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \" > Dear <span style=\"font-weight: 700; color: #9155fd\" >[[RequestorFullName]]</span >, </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \" > Your ticket is now being processed by <span style=\"font-weight: 700; color: #9155fd\" >[[ResolverFullName]]</span >. </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"height: 12px; line-height: 12px\">   </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: left; color: #3a2a2c\" > This is an auto-generated email. Please do not reply. </div></td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></tbody> </table> </div></td></tr></tbody> </table> </div></div></div>", 3, "Ticket No. [[TicketId]] is being processed." },
                    { new Guid("30000000-0000-0000-0000-000000000005"), "<div style=\"word-spacing: normal; background-color: #eeeeee; font-family: 'IBM Plex Sans', sans-serif\"> <div style=\"background-color: #eeeeee\"> <div style=\"background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px\"> <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0px 24px 2px 24px; text-align: center\"> <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \"> <div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 22px 6px 15px 10px; word-break: break-word\"> <table cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; border-spacing: 0px\"> <tbody> <tr> <td style=\"width: 248px\"> <a href=\"/\" class=\"css-5k1n1y\" style=\"text-decoration: none; display: flex; align-items: center\"> <svg width=\"30\" height=\"25\" version=\"1.1\" viewBox=\"0 0 30 23\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\"> <g stroke=\"none\" stroke-width=\"1\" fill=\"none\" fill-rule=\"evenodd\"> <g id=\"Artboard\" transform=\"translate(-95.000000, -51.000000)\"> <g id=\"logo\" transform=\"translate(95.000000, 50.000000)\"> <path id=\"Combined-Shape\" fill=\"#9155FD\" d=\"M30,21.3918362 C30,21.7535219 29.9019196,22.1084381 29.7162004,22.4188007 C29.1490236,23.366632 27.9208668,23.6752135 26.9730355,23.1080366 L26.9730355,23.1080366 L23.714971,21.1584295 C23.1114106,20.7972624 22.7419355,20.1455972 22.7419355,19.4422291 L22.7419355,19.4422291 L22.741,12.7425689 L15,17.1774194 L7.258,12.7425689 L7.25806452,19.4422291 C7.25806452,20.1455972 6.88858935,20.7972624 6.28502902,21.1584295 L3.0269645,23.1080366 C2.07913318,23.6752135 0.850976404,23.366632 0.283799571,22.4188007 C0.0980803893,22.1084381 2.0190442e-15,21.7535219 0,21.3918362 L0,3.58469444 L0.00548573643,3.43543209 L0.00548573643,3.43543209 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 L15,9.19354839 L26.9548759,1.86636639 C27.2693965,1.67359571 27.6311047,1.5715689 28,1.5715689 C29.1045695,1.5715689 30,2.4669994 30,3.5715689 L30,3.5715689 Z\"></path> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.7505183 7.25806452 16.8305646\"></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.6445567 7.25806452 15.1370162\"></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.7417372 30 16.9537453\" transform=\"translate(26.370968, 12.771227) scale(-1, 1) translate(-26.370968, -12.771227) \"></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.6409734 30 15.2601969\" transform=\"translate(26.370968, 11.924453) scale(-1, 1) translate(-26.370968, -11.924453) \"></polygon> <path id=\"Rectangle\" fill-opacity=\"0.15\" fill=\"#FFF\" d=\"M3.04512412,1.86636639 L15,9.19354839 L15,9.19354839 L15,17.1774194 L0,8.58649679 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 Z\"></path> <path id=\"Rectangle\" fill-opacity=\"0.35\" fill=\"#FFF\" transform=\"translate(22.500000, 8.588710) scale(-1, 1) translate(-22.500000, -8.588710) \" d=\"M18.0451241,1.86636639 L30,9.19354839 L30,9.19354839 L30,17.1774194 L15,8.58649679 L15,3.5715689 C15,2.4669994 15.8954305,1.5715689 17,1.5715689 C17.3688953,1.5715689 17.7306035,1.67359571 18.0451241,1.86636639 Z\"></path> </g> </g> </g> </svg> <div style=\" font-size: 21px; line-height: 18px; text-align: right; color: #9155fd; font-weight: 600; margin-left: 10px; \"> Company Active </div></a> </td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></tbody> </table> </div><div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\"> [[CreatedAt]] </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\"> Oder ID: [[TransactionId]] </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> </table> </div><div style=\" background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px; padding-bottom: 10px; \"> <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 30px 32px 36px 32px; text-align: center\"> <div style=\"margin: 0px auto; max-width: 536px\"> <table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0; text-align: center\"> <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \"> <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: top; width: 100%; \"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \"> Dear <span style=\"font-weight: 700; color: #9155fd\">[[RequestorFullName]]</span>, </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \"> Your support ticket has been processed. </div><div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \"> Please review and close your ticket by clicking on the button below. </div><div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \"> Please remind that your ticket will be automatically closed after 3 days if there is no more further action. </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"height: 12px; line-height: 12px\">   </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\"> <div style=\"font-size: 12px; line-height: 18px; text-align: left; color: #3a2a2c\"> This is a auto-generated email. Please do not reply. </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></td></tr></tbody> </table> <div style=\"display: flex; justify-content: center\"> <button style=\"padding: 10px 30px; background-color: #9155fd; border: none; margin: 0px 15px\"> <a href=\"[[ReviewTicketUrl]]\" style=\"text-decoration: none; color: white\"> <strong>View Ticket</strong> </a> </button> </div></div></div></div>", 4, "Ticket No. [[TicketId]] has been completed." },
                    { new Guid("30000000-0000-0000-0000-000000000006"), "<div style=\"word-spacing: normal; background-color: #eeeeee; font-family: 'IBM Plex Sans', sans-serif\"> <div style=\"background-color: #eeeeee\"> <div style=\"background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px\" > <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\" > <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0px 24px 2px 24px; text-align: center\" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \" > <div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 22px 6px 15px 10px; word-break: break-word\" > <table cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse: collapse; border-spacing: 0px\" > <tbody> <tr> <td style=\"width: 248px\"> <a href=\"/\" class=\"css-5k1n1y\" style=\"text-decoration: none; display: flex; align-items: center\" > <svg width=\"30\" height=\"25\" version=\"1.1\" viewBox=\"0 0 30 23\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" > <g stroke=\"none\" stroke-width=\"1\" fill=\"none\" fill-rule=\"evenodd\" > <g id=\"Artboard\" transform=\"translate(-95.000000, -51.000000)\" > <g id=\"logo\" transform=\"translate(95.000000, 50.000000)\" > <path id=\"Combined-Shape\" fill=\"#9155FD\" d=\"M30,21.3918362 C30,21.7535219 29.9019196,22.1084381 29.7162004,22.4188007 C29.1490236,23.366632 27.9208668,23.6752135 26.9730355,23.1080366 L26.9730355,23.1080366 L23.714971,21.1584295 C23.1114106,20.7972624 22.7419355,20.1455972 22.7419355,19.4422291 L22.7419355,19.4422291 L22.741,12.7425689 L15,17.1774194 L7.258,12.7425689 L7.25806452,19.4422291 C7.25806452,20.1455972 6.88858935,20.7972624 6.28502902,21.1584295 L3.0269645,23.1080366 C2.07913318,23.6752135 0.850976404,23.366632 0.283799571,22.4188007 C0.0980803893,22.1084381 2.0190442e-15,21.7535219 0,21.3918362 L0,3.58469444 L0.00548573643,3.43543209 L0.00548573643,3.43543209 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 L15,9.19354839 L26.9548759,1.86636639 C27.2693965,1.67359571 27.6311047,1.5715689 28,1.5715689 C29.1045695,1.5715689 30,2.4669994 30,3.5715689 L30,3.5715689 Z\" ></path> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.7505183 7.25806452 16.8305646\" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"0 8.58870968 7.25806452 12.6445567 7.25806452 15.1370162\" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.7417372 30 16.9537453\" transform=\"translate(26.370968, 12.771227) scale(-1, 1) translate(-26.370968, -12.771227) \" ></polygon> <polygon id=\"Rectangle\" opacity=\"0.077704\" fill=\"#000\" points=\"22.7419355 8.58870968 30 12.6409734 30 15.2601969\" transform=\"translate(26.370968, 11.924453) scale(-1, 1) translate(-26.370968, -11.924453) \" ></polygon> <path id=\"Rectangle\" fill-opacity=\"0.15\" fill=\"#FFF\" d=\"M3.04512412,1.86636639 L15,9.19354839 L15,9.19354839 L15,17.1774194 L0,8.58649679 L0,3.5715689 C3.0881846e-16,2.4669994 0.8954305,1.5715689 2,1.5715689 C2.36889529,1.5715689 2.73060353,1.67359571 3.04512412,1.86636639 Z\" ></path> <path id=\"Rectangle\" fill-opacity=\"0.35\" fill=\"#FFF\" transform=\"translate(22.500000, 8.588710) scale(-1, 1) translate(-22.500000, -8.588710) \" d=\"M18.0451241,1.86636639 L30,9.19354839 L30,9.19354839 L30,17.1774194 L15,8.58649679 L15,3.5715689 C15,2.4669994 15.8954305,1.5715689 17,1.5715689 C17.3688953,1.5715689 17.7306035,1.67359571 18.0451241,1.86636639 Z\" ></path> </g> </g> </g> </svg> <div style=\" font-size: 21px; line-height: 18px; text-align: right; color: #9155fd; font-weight: 600; margin-left: 10px; \" > Company Active </div></a > </td></tr></tbody> </table> </td></tr></tbody> </table> </td></tr></tbody> </table> </div><div class=\"m_515302627557219584mj-column-per-50\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: middle; width: 50%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: middle; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\" > [[CreatedAt]] </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: right; color: #3a2a2c\" > Oder ID: [[TransactionId]] </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> </table> </div><div style=\" background: #ffe2cc; background-color: #ffe2cc; margin: 0px auto; max-width: 600px; padding-bottom: 10px; \" > <table style=\"background: #ffe2cc; background-color: #ffe2cc; width: 100%\" > <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 30px 32px 36px 32px; text-align: center\" > <div style=\"margin: 0px auto; max-width: 536px\"> <table cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%\"> <tbody> <tr> <td style=\"direction: ltr; font-size: 0px; padding: 0; text-align: center\" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0; line-height: 0; text-align: left; display: inline-block; width: 100%; direction: ltr; \" > <div class=\"m_515302627557219584mj-column-per-100\" style=\" font-size: 0px; text-align: left; direction: ltr; display: inline-block; vertical-align: top; width: 100%; \" > <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" > <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \" > Dear <span style=\"font-weight: 700; color: #9155fd\" >[[CustomerFullName]]</span >, </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \" > Your support ticket has been completed and closed. </div><div style=\" font-size: 14px; line-height: 20px; text-align: left; color: #3a2a2c; \" > You can click on the link below to review the ticket or create a new one </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></div></td></tr></tbody> <tbody> <tr> <td style=\"vertical-align: top; padding: 0\"> <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tbody> <tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"height: 12px; line-height: 12px\">   </div></td></tr><tr> <td style=\"font-size: 0px; padding: 0; word-break: break-word\" > <div style=\"font-size: 12px; line-height: 18px; text-align: left; color: #3a2a2c\" > This is a auto-generated email. Please do not reply. </div></td></tr></tbody> </table> </td></tr></tbody> </table> </div></td></tr></tbody> </table> <div style=\"display: flex; justify-content: center\"> <button style=\"padding: 10px 30px; background-color: #9155fd; border: none; margin: 0px 15px\" > <a href=\"[[ViewTicketUrl]]\" style=\"text-decoration: none; color: white\" > <strong>View Ticket</strong> </a> </button> <button style=\"padding: 10px 30px; background-color: #9155fd; border: none; margin: 0px 15px\" > <a href=\"[[CreateTicketUrl]]\" style=\"text-decoration: none; color: white\" > <strong>Create Ticket</strong> </a> </button> </div></div></div></div>", 5, "Ticket No. [[TicketId]] has been closed." },
                    { new Guid("30000000-0000-0000-0000-000000000007"), "NotImplemented.", 6, "Ticket No. [[TicketId]] has been assigned to you." },
                    { new Guid("30000000-0000-0000-0000-000000000008"), "NotImplemented.", 7, "Ticket No. [[TicketId]] has been reopened." },
                    { new Guid("30000000-0000-0000-0000-000000000009"), "NotImplemented.", 8, "Ticket No. [[TicketId]] has been reopened by Requestor." }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("10000000-0000-0000-0000-000000000001") });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CreatedById",
                table: "Attachments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ModifiedById",
                table: "Attachments",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedById",
                table: "Customers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ModifiedById",
                table: "Customers",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTickets_CustomerId",
                table: "CustomerTickets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTickets_RequestorId",
                table: "CustomerTickets",
                column: "RequestorId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTickets_ResolverId",
                table: "CustomerTickets",
                column: "ResolverId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTickets_RequestorId",
                table: "EmployeeTickets",
                column: "RequestorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTickets_ResolverId",
                table: "EmployeeTickets",
                column: "ResolverId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedById",
                table: "Transactions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CustomerId",
                table: "Transactions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ModifiedById",
                table: "Transactions",
                column: "ModifiedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "CustomerTickets");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "EmployeeTickets");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
