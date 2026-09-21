using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeServicesPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderMobileNo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MobileNo",
                table: "Providers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileNo",
                table: "ProviderDocuments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE p
                SET p.MobileNo = u.MobileNo
                FROM Providers p
                INNER JOIN UsersLogin u ON u.UID = p.UserUID;
                """);

            migrationBuilder.Sql("""
                UPDATE d
                SET d.MobileNo = p.MobileNo
                FROM ProviderDocuments d
                INNER JOIN Providers p ON p.UID = d.ProviderUID;
                """);

            // Any orphan rows (should not exist) get a unique placeholder so NOT NULL can apply.
            migrationBuilder.Sql("""
                UPDATE Providers
                SET MobileNo = CONCAT('UNKNOWN-', UID)
                WHERE MobileNo IS NULL OR LTRIM(RTRIM(MobileNo)) = '';
                """);

            migrationBuilder.Sql("""
                UPDATE d
                SET d.MobileNo = p.MobileNo
                FROM ProviderDocuments d
                INNER JOIN Providers p ON p.UID = d.ProviderUID
                WHERE d.MobileNo IS NULL OR LTRIM(RTRIM(d.MobileNo)) = '';
                """);

            migrationBuilder.AlterColumn<string>(
                name: "MobileNo",
                table: "Providers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MobileNo",
                table: "ProviderDocuments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Providers_MobileNo",
                table: "Providers",
                column: "MobileNo");

            migrationBuilder.CreateIndex(
                name: "UQ_ProviderDocuments_MobileNo",
                table: "ProviderDocuments",
                column: "MobileNo",
                unique: true);

            // ON UPDATE CASCADE keeps ProviderDocuments.MobileNo in sync when Providers.MobileNo changes.
            migrationBuilder.Sql("""
                ALTER TABLE [ProviderDocuments] WITH CHECK
                ADD CONSTRAINT [FK_ProviderDocuments_Providers_MobileNo]
                FOREIGN KEY ([MobileNo]) REFERENCES [Providers] ([MobileNo])
                ON UPDATE CASCADE
                ON DELETE NO ACTION;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderDocuments_Providers_MobileNo",
                table: "ProviderDocuments");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Providers_MobileNo",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "UQ_ProviderDocuments_MobileNo",
                table: "ProviderDocuments");

            migrationBuilder.DropColumn(
                name: "MobileNo",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "MobileNo",
                table: "ProviderDocuments");
        }
    }
}
