using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddUserIdToOrders : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "UserId",
            table: "Orders",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_Orders_UserId",
            table: "Orders",
            column: "UserId");

        migrationBuilder.AddForeignKey(
            name: "FK_Orders_Users_UserId",
            table: "Orders",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Orders_Users_UserId",
            table: "Orders");

        migrationBuilder.DropIndex(
            name: "IX_Orders_UserId",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "UserId",
            table: "Orders");
    }
}
