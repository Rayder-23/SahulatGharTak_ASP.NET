using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeServicesPortal.Migrations
{
    /// <inheritdoc />
    public partial class DropServiceTitleEstiBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drops a duplicate column: a colleague independently built the same "estimated
            // budget per service title" feature and applied esti_budget to the live DB via a raw
            // SQL script (scripts/add-servicetitles-esti-budget.sql, now removed), bypassing EF
            // migrations entirely — so this repo's model never tracked it. This session's own
            // implementation (ServiceTitles.BasePrice, see 20260916082544_AddServiceTitleBasePrice)
            // was kept as the canonical column; esti_budget is dropped to avoid two columns for
            // the same concept. Guarded with IF EXISTS since not every environment ran that script.
            migrationBuilder.Sql(@"
IF COL_LENGTH(N'dbo.ServiceTitles', N'esti_budget') IS NOT NULL
BEGIN
    ALTER TABLE dbo.ServiceTitles DROP COLUMN esti_budget;
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH(N'dbo.ServiceTitles', N'esti_budget') IS NULL
BEGIN
    ALTER TABLE dbo.ServiceTitles ADD esti_budget decimal(12,2) NULL;
END");
        }
    }
}
