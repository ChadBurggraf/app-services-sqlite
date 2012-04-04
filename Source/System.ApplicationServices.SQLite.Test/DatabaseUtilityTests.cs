// ----------------------------------------------------------------------------------------------
// <copyright file="DatabaseUtilityTests.cs" company="Tasty Codes">
//      Copyright (c) 2012 Chad Burggraf.
// </copyright>
// ----------------------------------------------------------------------------------------------

namespace System.ApplicationServices.SQLite.Test
{
    using System;
    using System.Data.SQLite;
    using System.Globalization;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Database utility tests.
    /// </summary>
    [TestClass]
    public sealed class DatabaseUtilityTests
    {
        /// <summary>
        /// Ensure database tests.
        /// </summary>
        [TestMethod]
        public void DatabaseUtilityEnsureDatabase()
        {
            string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".sqlite";
            string cs = DatabaseUtility.NormalizeConnectionString(string.Format(CultureInfo.InvariantCulture, "data source={0};version=3", fileName));
            string path = new SQLiteConnectionStringBuilder(cs).DataSource;
            Assert.IsFalse(File.Exists(path));
            DatabaseUtility.EnsureDatabase(cs);
            Assert.IsTrue(File.Exists(path));
        }

        /// <summary>
        /// Normalize connection string tests.
        /// </summary>
        [TestMethod]
        public void DatabaseUtilityNormalizeConnectionString()
        {
            string cs = DatabaseUtility.NormalizeConnectionString("data source=Membership.sqlite;version=3");
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, "data source=\"{0}\";version=3", Path.GetFullPath("Membership.sqlite")), cs);

            cs = DatabaseUtility.NormalizeConnectionString("data source=|DataDirectory|Membership.sqlite;version=3");
            Assert.AreEqual(string.Format(CultureInfo.InvariantCulture, "data source={0};version=3", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, (Path.Combine("App_Data", "Membership.sqlite")))), cs);
        }
    }
}
