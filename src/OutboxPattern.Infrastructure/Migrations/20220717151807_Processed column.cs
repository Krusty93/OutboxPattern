using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutboxPattern.Infrastructure.Migrations
{
    public partial class Processedcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedDate",
                table: "OutboxMessages",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedDate",
                table: "OutboxMessages");
        }
    }
}
