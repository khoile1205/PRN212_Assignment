using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    total_order_price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    customer_pay = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    reason = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders__3213E83F1BD4211A", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    stock = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    image = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Products__3213E83FDDACF27D", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    created_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__3213E83F0D81F2D2", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<int>(type: "int", nullable: true),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    created_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderPro__3213E83FECBD63FE", x => x.id);
                    table.ForeignKey(
                        name: "FK__OrderProd__order__7B5B524B",
                        column: x => x.order_id,
                        principalTable: "Orders",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__OrderProd__produ__7C4F7684",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ProductStock",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    created_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductS__3213E83FE7ACD8C1", x => x.id);
                    table.ForeignKey(
                        name: "FK__ProductSt__produ__6D0D32F4",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    avatar = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    created_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__3213E83FE9067E0A", x => x.id);
                    table.ForeignKey(
                        name: "FK__Users__role_id__6477ECF3",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "UserOrder",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    order_id = table.Column<int>(type: "int", nullable: true),
                    created_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_timestamp = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserOrde__3213E83F5A0D2241", x => x.id);
                    table.ForeignKey(
                        name: "FK__UserOrder__order__76969D2E",
                        column: x => x.order_id,
                        principalTable: "Orders",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__UserOrder__user___75A278F5",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_order_id",
                table: "OrderProduct",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_product_id",
                table: "OrderProduct",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStock_product_id",
                table: "ProductStock",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrder_order_id",
                table: "UserOrder",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrder_user_id",
                table: "UserOrder",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_role_id",
                table: "Users",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProduct");

            migrationBuilder.DropTable(
                name: "ProductStock");

            migrationBuilder.DropTable(
                name: "UserOrder");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
