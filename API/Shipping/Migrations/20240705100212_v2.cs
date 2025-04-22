using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shipping.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "374d11a2-781b-4821-8c63-f9031a2cb37f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "852ebdd5-55e7-4358-8fcc-8d275b2d575b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c14b5250-740e-4056-96ea-ede52f6209e8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ab58670-8727-4b67-85d5-4199912a70bf",
                column: "Date",
                value: "05/07/2024 01:02:07 م");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Date", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "02ce01f9-6de7-4259-aa9e-79917498cf52", null, "05/07/2024 01:02:07 م", "الموظفين", "الموظفين" },
                    { "cbff0e83-f3ba-4184-826b-df5d21636f81", null, "05/07/2024 01:02:07 م", "المناديب", "المناديب" },
                    { "e178d070-9148-4278-a68d-292a0973b929", null, "05/07/2024 01:02:07 م", "التجار", "التجار" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "76f86073-b51c-47c4-b7fa-731628055ebb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "32ab12f6-20d6-497f-940b-4a5e947e89db", "AQAAAAIAAYagAAAAEEZyZUpcwqZFvYOQuTadUbAJgFaNZADXdjzHUV6TOwhWXrwFlUAhprCpIQIHzCyCrg==", "94cead1b-436a-44c3-bb08-4991546020f6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "02ce01f9-6de7-4259-aa9e-79917498cf52");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cbff0e83-f3ba-4184-826b-df5d21636f81");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e178d070-9148-4278-a68d-292a0973b929");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ab58670-8727-4b67-85d5-4199912a70bf",
                column: "Date",
                value: "7/3/2024 12:46:53 PM");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Date", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "374d11a2-781b-4821-8c63-f9031a2cb37f", null, "7/3/2024 12:46:53 PM", "الموظفين", "الموظفين" },
                    { "852ebdd5-55e7-4358-8fcc-8d275b2d575b", null, "7/3/2024 12:46:53 PM", "المناديب", "المناديب" },
                    { "c14b5250-740e-4056-96ea-ede52f6209e8", null, "7/3/2024 12:46:53 PM", "التجار", "التجار" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "76f86073-b51c-47c4-b7fa-731628055ebb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7f7ebe57-fd13-4bbb-92aa-ef394f1cecf7", "AQAAAAIAAYagAAAAEGAb2O2QV9W4CLCM9Z2yn2aR79XLq8SKXWkZPMcKEIhsnOCXNjLwgB9hSRvKqLD7+w==", "e2152d3f-8583-4080-9ed2-763152cc8c42" });
        }
    }
}
