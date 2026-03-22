using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateWorkflowDefinition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "is_required",
                schema: "workflow",
                table: "master_data_column",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.CreateTable(
                name: "workflow_definition",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    icon = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    permissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_definition", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "workflow_field",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    version_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    label = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    data_type = table.Column<int>(type: "int", nullable: false),
                    data_source_type = table.Column<int>(type: "int", nullable: true),
                    data_source_config_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    field_formula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    settings_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    is_required = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_field", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "workflow_layout",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    version_id = table.Column<int>(type: "int", nullable: false),
                    rows_json = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    attachment_settings_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_layout", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "workflow_report",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    version_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    fields_config_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    chart_config_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_report", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "workflow_step_define",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    version_id = table.Column<int>(type: "int", nullable: false),
                    label = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    step_type = table.Column<int>(type: "int", nullable: false),
                    status_code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    assign_rule = table.Column<int>(type: "int", nullable: false),
                    assign_value_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sla_time = table.Column<int>(type: "int", nullable: true),
                    sla_unit = table.Column<int>(type: "int", nullable: true),
                    position_x = table.Column<double>(type: "float", nullable: false),
                    position_y = table.Column<double>(type: "float", nullable: false),
                    is_signature_step = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_step_define", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "workflow_version",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    workflow_id = table.Column<int>(type: "int", nullable: false),
                    version_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_version", x => x.id);
                    table.ForeignKey(
                        name: "FK_workflow_version_workflow_definition_workflow_id",
                        column: x => x.workflow_id,
                        principalSchema: "workflow",
                        principalTable: "workflow_definition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_grid_column",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    parent_field_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    label = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    data_type = table.Column<int>(type: "int", nullable: false),
                    data_source_type = table.Column<int>(type: "int", nullable: true),
                    data_source_config_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    settings_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    is_required = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_grid_column", x => x.id);
                    table.ForeignKey(
                        name: "FK_workflow_grid_column_workflow_field_parent_field_id",
                        column: x => x.parent_field_id,
                        principalSchema: "workflow",
                        principalTable: "workflow_field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_step_define_action",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    step_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    button_key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    label = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    target_step_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    notify_template = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_step_define_action", x => x.id);
                    table.ForeignKey(
                        name: "FK_workflow_step_define_action_workflow_step_define_step_id",
                        column: x => x.step_id,
                        principalSchema: "workflow",
                        principalTable: "workflow_step_define",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_step_define_document",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    step_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    doc_type_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    is_required = table.Column<bool>(type: "bit", nullable: false),
                    check_digital_signature = table.Column<bool>(type: "bit", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    modified_by = table.Column<int>(type: "int", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_step_define_document", x => x.id);
                    table.ForeignKey(
                        name: "FK_workflow_step_define_document_workflow_step_define_step_id",
                        column: x => x.step_id,
                        principalSchema: "workflow",
                        principalTable: "workflow_step_define",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_step_field_permission",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    step_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    field_id = table.Column<int>(type: "int", nullable: false),
                    permission = table.Column<int>(type: "int", nullable: false),
                    is_required = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_step_field_permission", x => x.id);
                    table.ForeignKey(
                        name: "FK_workflow_step_field_permission_workflow_step_define_step_id",
                        column: x => x.step_id,
                        principalSchema: "workflow",
                        principalTable: "workflow_step_define",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_step_hook",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    step_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    event_type = table.Column<int>(type: "int", nullable: false),
                    action_type = table.Column<int>(type: "int", nullable: false),
                    config_json = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_step_hook", x => x.id);
                    table.ForeignKey(
                        name: "FK_workflow_step_hook_workflow_step_define_step_id",
                        column: x => x.step_id,
                        principalSchema: "workflow",
                        principalTable: "workflow_step_define",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_action_rule",
                schema: "workflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    action_id = table.Column<int>(type: "int", nullable: false),
                    condition_expression = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    target_step_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_action_rule", x => x.id);
                    table.ForeignKey(
                        name: "FK_workflow_action_rule_workflow_step_define_action_action_id",
                        column: x => x.action_id,
                        principalSchema: "workflow",
                        principalTable: "workflow_step_define_action",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_workflow_action_rule_action_id",
                schema: "workflow",
                table: "workflow_action_rule",
                column: "action_id");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_definition_code",
                schema: "workflow",
                table: "workflow_definition",
                column: "code",
                unique: true,
                filter: "[is_deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_definition_is_deleted",
                schema: "workflow",
                table: "workflow_definition",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_grid_column_parent_field_id",
                schema: "workflow",
                table: "workflow_grid_column",
                column: "parent_field_id");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_step_define_action_step_id",
                schema: "workflow",
                table: "workflow_step_define_action",
                column: "step_id");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_step_define_document_step_id",
                schema: "workflow",
                table: "workflow_step_define_document",
                column: "step_id");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_step_field_permission_step_id",
                schema: "workflow",
                table: "workflow_step_field_permission",
                column: "step_id");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_step_hook_step_id",
                schema: "workflow",
                table: "workflow_step_hook",
                column: "step_id");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_version_workflow_id",
                schema: "workflow",
                table: "workflow_version",
                column: "workflow_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "workflow_action_rule",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflow_grid_column",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflow_layout",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflow_report",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflow_step_define_document",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflow_step_field_permission",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflow_step_hook",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflow_version",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflow_step_define_action",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflow_field",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflow_definition",
                schema: "workflow");

            migrationBuilder.DropTable(
                name: "workflow_step_define",
                schema: "workflow");

            migrationBuilder.AlterColumn<bool>(
                name: "is_required",
                schema: "workflow",
                table: "master_data_column",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
