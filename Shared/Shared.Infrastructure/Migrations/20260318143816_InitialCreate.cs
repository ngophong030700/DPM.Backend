using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "department",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    index = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    level = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    path_code = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_department", x => x.id);
                    table.ForeignKey(
                        name: "FK_department_department_parent_id",
                        column: x => x.parent_id,
                        principalTable: "department",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "position",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_position", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    department_id = table.Column<int>(type: "int", nullable: true),
                    position_id = table.Column<int>(type: "int", nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    distinguished_name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    sid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    last_login_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_sync_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    gender = table.Column<bool>(type: "bit", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_department_department_id",
                        column: x => x.department_id,
                        principalTable: "department",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_user_position_position_id",
                        column: x => x.position_id,
                        principalTable: "position",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "user_group",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    group_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_group", x => new { x.user_id, x.group_id });
                    table.ForeignKey(
                        name: "FK_user_group_group_group_id",
                        column: x => x.group_id,
                        principalTable: "group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_group_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_department_is_deleted",
                table: "department",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_department_name",
                table: "department",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_department_parent_id",
                table: "department",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_department_path_code",
                table: "department",
                column: "path_code");

            migrationBuilder.CreateIndex(
                name: "IX_group_is_deleted",
                table: "group",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_group_name",
                table: "group",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_position_is_deleted",
                table: "position",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_position_name",
                table: "position",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_user_department_id",
                table: "user",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_email",
                table: "user",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_user_is_deleted",
                table: "user",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_user_position_id",
                table: "user",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_sid",
                table: "user",
                column: "sid");

            migrationBuilder.CreateIndex(
                name: "IX_user_username",
                table: "user",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "IX_user_group_group_id",
                table: "user_group",
                column: "group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_group");

            migrationBuilder.DropTable(
                name: "group");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "department");

            migrationBuilder.DropTable(
                name: "position");
        }
    }
}
