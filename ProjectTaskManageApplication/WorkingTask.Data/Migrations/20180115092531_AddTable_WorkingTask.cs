using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WorkingTask.Data.Migrations
{
    public partial class AddTable_WorkingTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateUser = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkingTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClosingDate = table.Column<DateTime>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateUser = table.Column<string>(nullable: true),
                    TaskRemark = table.Column<string>(nullable: true),
                    TaskState = table.Column<string>(nullable: true),
                    WorkingTaskName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkTaskSubmieOperations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Executor = table.Column<string>(nullable: true),
                    ItemCreateTime = table.Column<DateTime>(nullable: false),
                    ItemRemark = table.Column<string>(nullable: true),
                    ItemStyle = table.Column<string>(nullable: true),
                    TaskState = table.Column<string>(nullable: true),
                    WorkTaskId = table.Column<int>(nullable: false),
                    WorkingTaskItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTaskSubmieOperations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkingTaskItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateUser = table.Column<string>(nullable: true),
                    Executor = table.Column<string>(nullable: true),
                    ItemClosingDate = table.Column<DateTime>(nullable: true),
                    ItemCreateTime = table.Column<DateTime>(nullable: false),
                    ItemName = table.Column<string>(nullable: true),
                    ItemRemark = table.Column<string>(nullable: true),
                    ItemStyle = table.Column<string>(nullable: true),
                    WorkingTaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingTaskItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingTaskItems_WorkingTasks_WorkingTaskId",
                        column: x => x.WorkingTaskId,
                        principalTable: "WorkingTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingTaskItems_WorkingTaskId",
                table: "WorkingTaskItems",
                column: "WorkingTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentInfos");

            migrationBuilder.DropTable(
                name: "WorkingTaskItems");

            migrationBuilder.DropTable(
                name: "WorkTaskSubmieOperations");

            migrationBuilder.DropTable(
                name: "WorkingTasks");
        }
    }
}
