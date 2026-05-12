using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BackfillDocumentTypeForExistingLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rows created before DocumentType was introduced have document_type = '' (EF Core default).
            // Backfill them to 'Resume' — all pre-existing links were resumes.
            migrationBuilder.Sql(
                "UPDATE job_resume_links SET document_type = 'Resume' WHERE document_type = ''");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE job_resume_links SET document_type = '' WHERE document_type = 'Resume'");
        }
    }
}
