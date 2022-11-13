using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP12RazorPageEntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class SeedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            for (int i = 0; i < 150; i++)
            {
                migrationBuilder.InsertData(
                    "Users",
                    columns: new[]
                    {
                        "Id",
                        "UserName",
                        "Email" ,
                        "SecurityStamp",
                        "EmailConfirmed",
                        "PhoneNumberConfirmed",
                        "TwoFactorEnabled",
                        "LockoutEnabled",
                        "AccessFailedCount"
                    },
                    values:new object[]
                    {
                        Guid.NewGuid().ToString(),
                        "User"+i.ToString("D3"),
                        $"email{i.ToString("D3")}@example.com",
                        Guid.NewGuid().ToString(),
                        true,
                        false,
                        false,
                        false,
                        0
                    }
                    );
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

// "Id"                   text    not null
// constraint "PK_Users"
// primary key,
// "UserName"             varchar(256),
// "NormalizedUserName"   varchar(256),
// "Email"                varchar(256),
// "NormalizedEmail"      varchar(256),
// "EmailConfirmed"       boolean not null,
// "PasswordHash"         text,
// "SecurityStamp"        text,
// "ConcurrencyStamp"     text,
// "PhoneNumber"          text,
// "PhoneNumberConfirmed" boolean not null,
// "TwoFactorEnabled"     boolean not null,
// "LockoutEnd"           timestamp with time zone,
// "LockoutEnabled"       boolean not null,
// "AccessFailedCount"    integer not null