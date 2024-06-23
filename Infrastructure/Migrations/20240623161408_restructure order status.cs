using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class restructureorderstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderComutationStatuses",
                table: "OrderComutationStatuses");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderComutationStatuses",
                newName: "CompanyId");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "OrderComutationStatuses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderComputationRequestGuid",
                table: "OrderComutationStatuses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderComutationStatuses",
                table: "OrderComutationStatuses",
                column: "OrderComputationRequestGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderComutationStatuses",
                table: "OrderComutationStatuses");

            migrationBuilder.DropColumn(
                name: "OrderComputationRequestGuid",
                table: "OrderComutationStatuses");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "OrderComutationStatuses",
                newName: "OrderId");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrderComutationStatuses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderComutationStatuses",
                table: "OrderComutationStatuses",
                column: "OrderId");
        }
    }
}
