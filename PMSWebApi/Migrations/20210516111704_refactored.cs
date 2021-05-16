using Microsoft.EntityFrameworkCore.Migrations;

namespace PMSWebApi.Migrations
{
    public partial class refactored : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubProjects_Projects_ProjectDTOId",
                table: "SubProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTasks_Tasks_TaskDTOId",
                table: "SubTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectDTOId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_SubProjects_SubProjectDTOId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ProjectDTOId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_SubTasks_TaskDTOId",
                table: "SubTasks");

            migrationBuilder.DropIndex(
                name: "IX_SubProjects_ProjectDTOId",
                table: "SubProjects");

            migrationBuilder.DropColumn(
                name: "ProjectDTOId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskDTOId",
                table: "SubTasks");

            migrationBuilder.DropColumn(
                name: "ProjectDTOId",
                table: "SubProjects");

            migrationBuilder.RenameColumn(
                name: "SubProjectDTOId",
                table: "Tasks",
                newName: "SubProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_SubProjectDTOId",
                table: "Tasks",
                newName: "IX_Tasks_SubProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_TaskId",
                table: "SubTasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_SubProjects_ProjectId",
                table: "SubProjects",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubProjects_Projects_ProjectId",
                table: "SubProjects",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTasks_Tasks_TaskId",
                table: "SubTasks",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_SubProjects_SubProjectId",
                table: "Tasks",
                column: "SubProjectId",
                principalTable: "SubProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubProjects_Projects_ProjectId",
                table: "SubProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTasks_Tasks_TaskId",
                table: "SubTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Projects_ProjectId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_SubProjects_SubProjectId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_SubTasks_TaskId",
                table: "SubTasks");

            migrationBuilder.DropIndex(
                name: "IX_SubProjects_ProjectId",
                table: "SubProjects");

            migrationBuilder.RenameColumn(
                name: "SubProjectId",
                table: "Tasks",
                newName: "SubProjectDTOId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_SubProjectId",
                table: "Tasks",
                newName: "IX_Tasks_SubProjectDTOId");

            migrationBuilder.AddColumn<int>(
                name: "ProjectDTOId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskDTOId",
                table: "SubTasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectDTOId",
                table: "SubProjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectDTOId",
                table: "Tasks",
                column: "ProjectDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_TaskDTOId",
                table: "SubTasks",
                column: "TaskDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_SubProjects_ProjectDTOId",
                table: "SubProjects",
                column: "ProjectDTOId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubProjects_Projects_ProjectDTOId",
                table: "SubProjects",
                column: "ProjectDTOId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTasks_Tasks_TaskDTOId",
                table: "SubTasks",
                column: "TaskDTOId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Projects_ProjectDTOId",
                table: "Tasks",
                column: "ProjectDTOId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_SubProjects_SubProjectDTOId",
                table: "Tasks",
                column: "SubProjectDTOId",
                principalTable: "SubProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
