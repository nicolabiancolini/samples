// See the LICENSE.TXT file in the project root for full license information.

using System.Globalization;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample.SqlServer.Design.Migrations
{
    public partial class InitialWithSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var migrationAttribute = (MigrationAttribute)this.GetType()
                .GetCustomAttributes(typeof(MigrationAttribute), false)
                .Single();

            migrationBuilder.Sql(File.ReadAllText(string.Format(
                CultureInfo.InvariantCulture,
                "{1}{0}RawMigrations{0}{2}",
                Path.DirectorySeparatorChar,
                AppContext.BaseDirectory,
                $"{migrationAttribute.Id}.sql")));
        }
    }
}
