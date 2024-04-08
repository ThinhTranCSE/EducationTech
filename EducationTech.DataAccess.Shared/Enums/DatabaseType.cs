namespace EducationTech.DataAccess.Shared.Enums
{
    public enum DatabaseType
    {
        MySql,
        SqlServer,
        PostgreSql,
        Sqlite
    }

    public static class DatabaseTypeExtensions
    {
        public static DatabaseType ToDatabaseType(this string databaseType)
        {
            return databaseType switch
            {
                "MySql" => DatabaseType.MySql,
                "SqlServer" => DatabaseType.SqlServer,
                "PostgreSql" => DatabaseType.PostgreSql,
                "Sqlite" => DatabaseType.Sqlite,
                _ => throw new ArgumentOutOfRangeException(databaseType)
            };
        }
    }
}
