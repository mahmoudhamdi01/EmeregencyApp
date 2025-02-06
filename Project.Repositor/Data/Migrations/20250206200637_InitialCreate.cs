using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Repositor.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "emergencyServices",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emergencyServices", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "uploadVideos",
                columns: table => new
                {
                    UploadVideoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmergencyServiceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploadVideos", x => x.UploadVideoId);
                    table.ForeignKey(
                        name: "FK_uploadVideos_emergencyServices_EmergencyServiceId",
                        column: x => x.EmergencyServiceId,
                        principalTable: "emergencyServices",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_uploadVideos_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "videos",
                columns: table => new
                {
                    VideoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmergencyServiceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_videos", x => x.VideoId);
                    table.ForeignKey(
                        name: "FK_videos_emergencyServices_EmergencyServiceId",
                        column: x => x.EmergencyServiceId,
                        principalTable: "emergencyServices",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_videos_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_uploadVideos_EmergencyServiceId",
                table: "uploadVideos",
                column: "EmergencyServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_uploadVideos_UserId",
                table: "uploadVideos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_videos_EmergencyServiceId",
                table: "videos",
                column: "EmergencyServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_videos_UserId",
                table: "videos",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "uploadVideos");

            migrationBuilder.DropTable(
                name: "videos");

            migrationBuilder.DropTable(
                name: "emergencyServices");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
