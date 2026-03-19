using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateMasterDatasource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "master_data_source",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_data_source", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "master_data_column",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    source_id = table.Column<int>(type: "int", nullable: false),
                    column_key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    column_label = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    data_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_required = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    sort_order = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_data_column", x => x.id);
                    table.ForeignKey(
                        name: "FK_master_data_column_master_data_source_source_id",
                        column: x => x.source_id,
                        principalSchema: "workflow",
                        principalTable: "master_data_source",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "master_data_value",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    source_id = table.Column<int>(type: "int", nullable: false),
                    display_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    value_code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_data_value", x => x.id);
                    table.ForeignKey(
                        name: "FK_master_data_value_master_data_source_source_id",
                        column: x => x.source_id,
                        principalSchema: "workflow",
                        principalTable: "master_data_source",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "master_data_cell",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value_id = table.Column<int>(type: "int", nullable: false),
                    column_id = table.Column<int>(type: "int", nullable: false),
                    cell_value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_data_cell", x => x.id);
                    table.ForeignKey(
                        name: "FK_master_data_cell_master_data_value_value_id",
                        column: x => x.value_id,
                        principalSchema: "workflow",
                        principalTable: "master_data_value",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_master_data_cell_value_id",
                schema: "workflow",
                table: "master_data_cell",
                column: "value_id");

            migrationBuilder.CreateIndex(
                name: "IX_master_data_column_is_deleted",
                schema: "workflow",
                table: "master_data_column",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_master_data_column_source_id",
                schema: "workflow",
                table: "master_data_column",
                column: "source_id");

            migrationBuilder.CreateIndex(
                name: "IX_master_data_source_code",
                schema: "workflow",
                table: "master_data_source",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_master_data_source_is_deleted",
                schema: "workflow",
                table: "master_data_source",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_master_data_value_is_deleted",
                schema: "workflow",
                table: "master_data_value",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_master_data_value_source_id",
                schema: "workflow",
                table: "master_data_value",
                column: "source_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "master_data_cell",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "master_data_column",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "master_data_value",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "master_data_source",
                schema: "workflow");
        }
    }
}
