// ----------------------------------------------------------------------------------------------
// <copyright file="DatabaseUtility.cs" company="Tasty Codes">
//      Copyright (c) 2012 Chad Burggraf.
// </copyright>
// ----------------------------------------------------------------------------------------------

namespace System.ApplicationServices.SQLite
{
    using System;
    using System.Configuration;
    using System.Data.SQLite;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides database utilities.
    /// </summary>
    internal static class DatabaseUtility
    {
        private static readonly object locker = new object();

        /// <summary>
        /// Ensures that the database identified in the given connection string exists.
        /// If it does not exist, it is created and the membership schema is created.
        /// </summary>
        /// <param name="connectionString">The connection string to ensure the database for.</param>
        public static void EnsureDatabase(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString", "connectionString cannot be null.");
            }

            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder(connectionString);
            string path = builder.DataSource;

            lock (locker)
            {
                if (!File.Exists(path))
                {
                    using (SQLiteConnection connection = new SQLiteConnection(string.Format(CultureInfo.InvariantCulture, @"data source={0};synchronous=Off;journal mode=Off;version=3", path)))
                    {
                        connection.Open();

                        using (SQLiteCommand command = connection.CreateCommand())
                        {
                            command.CommandType = Data.CommandType.Text;
                            command.CommandText = GetInstallSql();
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Normalizes the given connection string by resolving the database path into
        /// a fully qualified path.
        /// </summary>
        /// <param name="connectionString">The connection string to normalize.</param>
        /// <returns>The normalized connection string.</returns>
        public static string NormalizeConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString", "connectionString cannot be null.");
            }

            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder(connectionString);
            builder.DataSource = ResolveDatabasePath(builder.DataSource);
            return builder.ToString();
        }

        /// <summary>
        /// Gets the install SQL script.
        /// </summary>
        /// <returns>A string of SQL.</returns>
        private static string GetInstallSql()
        {
            Stream stream = null;

            try
            {
                stream = typeof(DatabaseUtility).Assembly.GetManifestResourceStream("System.ApplicationServices.SQLite.InstallMembership.sql");

                using (StreamReader reader = new StreamReader(stream))
                {
                    stream = null;
                    return reader.ReadToEnd();
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// Resolves the given database path into a fully qualified path.
        /// </summary>
        /// <param name="path">The path to resolve..</param>
        /// <returns>The resolved path.</returns>
        private static string ResolveDatabasePath(string path)
        {
            const string DataDirectory = "|DataDirectory|";
            path = (path ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(path))
            {
                int dataDirectoryIndex = path.IndexOf(DataDirectory, StringComparison.OrdinalIgnoreCase);

                if (dataDirectoryIndex > -1)
                {
                    path = path.Substring(dataDirectoryIndex + DataDirectory.Length);
                    path = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data"), path);
                }
                else
                {
                    Regex exp = new Regex(string.Concat("[", Regex.Escape(new string(Path.GetInvalidPathChars())), "]"));

                    if (!exp.IsMatch(path) && !Path.IsPathRooted(path))
                    {
                        string dir = ConfigurationManager.ConnectionStrings.ElementInformation.Source;
                        dir = !string.IsNullOrEmpty(dir) ? Path.GetDirectoryName(dir) : null;

                        if (!string.IsNullOrEmpty(dir))
                        {
                            path = Path.Combine(dir, path);
                        }
                        else
                        {
                            path = Path.GetFullPath(path);
                        }
                    }
                }
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }

            return path;
        }
    }
}
