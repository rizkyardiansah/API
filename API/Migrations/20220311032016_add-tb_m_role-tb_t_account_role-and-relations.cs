using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class addtb_m_roletb_t_account_roleandrelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_m_role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_t_account_role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_t_account_role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_t_account_role_tb_m_account_NIK",
                        column: x => x.NIK,
                        principalTable: "tb_m_account",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_t_account_role_tb_m_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tb_m_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_t_account_role_NIK",
                table: "tb_t_account_role",
                column: "NIK");

            migrationBuilder.CreateIndex(
                name: "IX_tb_t_account_role_RoleId",
                table: "tb_t_account_role",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_t_account_role");

            migrationBuilder.DropTable(
                name: "tb_m_role");
        }
    }
}
