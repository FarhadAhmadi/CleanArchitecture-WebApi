using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableNames2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: "69DB714F-9576-45BA-B5B7-F00649BE01DE",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "ac71d6ab-d97a-4998-a83c-f9cb3a3baa24", "AQAAAAIAAYagAAAAEG7q/AhL/p+7gM0zZkG+EoSi4+j5e9rXrx8vkzLHdi8GbMiJk3wTc709e2y1eAwFrg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: "69DB714F-9576-45BA-B5B7-F00649BE01DE",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "824fcaac-cdef-424f-872e-d169f95f4901", "AQAAAAIAAYagAAAAEH5hQuUhTv23Gx7U0Qx6qJ16QEy/9mRU2sbb9kK+zLXO7IncYas7/spsjWQ/u0emiA==" });
        }
    }
}
