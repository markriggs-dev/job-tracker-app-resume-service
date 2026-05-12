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
            // Rows created before DocumentType was introduced have "DocumentType" = '' (EF Core default).
            // Backfill them to 'Resume' — all pre-existing links were resumes.
            // Column name is PascalCase ("DocumentType") — must be quoted in PostgreSQL.
            migrationBuilder.Sql(
                "UPDATE job_resume_links SET \"DocumentType\" = 'Resume' WHERE \"DocumentType\" = ''");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE job_resume_links SET \"DocumentType\" = '' WHERE \"DocumentType\" = 'Resume'");
        }
    }
}
