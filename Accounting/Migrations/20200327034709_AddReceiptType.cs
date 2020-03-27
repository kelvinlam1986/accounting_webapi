using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Accounting.Migrations
{
    public partial class AddReceiptType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceiptTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "char(3)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ReceiptTypeInSecondLanguage = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ReceiptTypeInVietnamese = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ShowReceiptTypeInVietNamese = table.Column<bool>(nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptTypes", x => x.Code);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptTypes");
        }
    }
}
