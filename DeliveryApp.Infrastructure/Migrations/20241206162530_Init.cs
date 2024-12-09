using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeliveryApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "courier_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courier_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transport",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    speed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transport", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "couriers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    transport_id = table.Column<int>(type: "integer", nullable: false),
                    location_x = table.Column<int>(type: "integer", nullable: true),
                    location_y = table.Column<int>(type: "integer", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_couriers", x => x.id);
                    table.ForeignKey(
                        name: "FK_couriers_courier_status_status_id",
                        column: x => x.status_id,
                        principalTable: "courier_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_couriers_transport_transport_id",
                        column: x => x.transport_id,
                        principalTable: "transport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    location_x = table.Column<int>(type: "integer", nullable: true),
                    location_y = table.Column<int>(type: "integer", nullable: true),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    courier_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_couriers_courier_id",
                        column: x => x.courier_id,
                        principalTable: "couriers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_orders_order_status_status_id",
                        column: x => x.status_id,
                        principalTable: "order_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "courier_status",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "free" },
                    { 2, "busy" }
                });

            migrationBuilder.InsertData(
                table: "order_status",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "created" },
                    { 2, "assigned" },
                    { 3, "completed" }
                });

            migrationBuilder.InsertData(
                table: "transport",
                columns: new[] { "id", "name", "speed" },
                values: new object[,]
                {
                    { 1, "Pedestrian", 1 },
                    { 2, "Bicycle", 2 },
                    { 3, "Car", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_courier_status_name",
                table: "courier_status",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_couriers_status_id",
                table: "couriers",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_couriers_transport_id",
                table: "couriers",
                column: "transport_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_status_name",
                table: "order_status",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_orders_courier_id",
                table: "orders",
                column: "courier_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_status_id",
                table: "orders",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_transport_name",
                table: "transport",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "couriers");

            migrationBuilder.DropTable(
                name: "order_status");

            migrationBuilder.DropTable(
                name: "courier_status");

            migrationBuilder.DropTable(
                name: "transport");
        }
    }
}
