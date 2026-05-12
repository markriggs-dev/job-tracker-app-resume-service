using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentTypeToJobResumeLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_job_resume_links_JobRequisitionId_UserId",
                table: "job_resume_links");

            migrationBuilder.AddColumn<string>(
                name: "DocumentType",
                table: "job_resume_links",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_job_resume_links_JobRequisitionId_UserId_DocumentType",
                table: "job_resume_links",
                columns: new[] { "JobRequisitionId", "UserId", "DocumentType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_job_resume_links_JobRequisitionId_UserId_DocumentType",
                table: "job_resume_links");

            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "job_resume_links");

            migrationBuilder.CreateIndex(
                name: "IX_job_resume_links_JobRequisitionId_UserId",
                table: "job_resume_links",
                columns: new[] { "JobRequisitionId", "UserId" },
                unique: true);
        }
    }
}
