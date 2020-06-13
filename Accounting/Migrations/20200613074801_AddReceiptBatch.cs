using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Accounting.Migrations
{
    public partial class AddReceiptBatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceiptBatches",
                columns: table => new
                {
                    ReceiptBatchNo = table.Column<string>(type: "varchar(10)", nullable: false),
                    BatchStatus = table.Column<bool>(type: "varchar(10)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    DescriptionInVietNamese = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    ReceiptBatchDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptBatches", x => x.ReceiptBatchNo);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptBatches");
        }
    }
}
