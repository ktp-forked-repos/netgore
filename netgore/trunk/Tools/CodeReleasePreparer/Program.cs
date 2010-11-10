﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;
using MySqlHelper = InstallationValidator.MySqlHelper;

namespace CodeReleasePreparer
{
    class Program
    {
        /// <summary>
        /// If true, ONLY the database schema part will be built. Otherwise, a complete clean will be done.
        /// </summary>
        const bool _buildSchemaOnly = false;

        static readonly string[] _deleteFilePatterns = new string[]
        {
            @"\.csproj\.user$",
            @"\.resharper\.user$", 
            @"\.suo$",
            @"\.cachefile$", 
            @"\.vshost\.exe", 
            @"\.pdb$", 
            @"[\\/]bin[\\/]",
            @"[\\/]obj[\\/]", 
            @"[\\/]\.svn[\\/]", 
            @"[\\/]_ReSharper\.", 
            @"[\\/]InstallationValidator[\\/]bin[\\/]csfml.+\.dll$",
            @"[\\/]InstallationValidator[\\/]bin[\\/]libsndfile.*\.dll$", 
            @"[\\/]InstallationValidator[\\/]bin[\\/]openal.*\.dll$",
        };

        static readonly string[] _preserveFilePatterns = new string[]
        {
            @"\.bat$", 
            @"[\\/]InstallationValidator[\\/]bin[\\/][^\\/]+\.exe",
            @"[\\/]InstallationValidator[\\/]bin[\\/][^\\/]+\.dll",
            @"[\\/]CodeReleasePreparer[\\/]bin[\\/]",
        };

        static RegexCollection _delFileRegexes;
        static RegexCollection _preFileRegexes;

        /// <summary>
        /// Performs the database cleaning.
        /// </summary>
        static void CleanDatabase()
        {
            var sb = new MySqlConnectionStringBuilder
            { UserID = "root", Database = "demogame", Server = "localhost", Password = "", Logging = false, Pooling = false };

            using (var conn = new MySqlConnection(sb.ToString()))
            {
                conn.Open();

                // Truncate tables
                var truncateTables = CleanDatabaseStr(conn,
                                                      "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE" +
                                                      " `TABLE_SCHEMA`=\"demogame\" AND `TABLE_NAME` LIKE \"world_stats_%\"");

                truncateTables =
                    truncateTables.Concat(new string[]
                    {
                        "account_ban", "account_ips", "active_trade_cash", "active_trade_item", "character_quest_status",
                        "character_quest_status_kills", "character_status_effect", "guild", "guild_event", "guild_member"
                    });

                foreach (var table in truncateTables)
                {
                    CleanDatabaseNQ(conn, string.Format("TRUNCATE TABLE `{0}`", table));
                }

                // Clean out items table
                CleanDatabaseNQ(conn,
                                "DELETE FROM `item` WHERE `id` NOT IN (SELECT `item_id` FROM `character_inventory`) " +
                                "AND `id` NOT IN (SELECT `item_id` FROM `character_equipped`)");
            }
        }

        /// <summary>
        /// Sub-routine for the <see cref="CleanDatabase"/> that provides a short-hand for running a non-reader query.
        /// </summary>
        /// <param name="conn">The <see cref="MySqlConnection"/>.</param>
        /// <param name="query">The query to run.</param>
        static void CleanDatabaseNQ(MySqlConnection conn, string query)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Sub-routine for the <see cref="CleanDatabase"/> that provides a short-hand for running a query.
        /// </summary>
        /// <param name="conn">The <see cref="MySqlConnection"/>.</param>
        /// <param name="query">The query to run.</param>
        static IEnumerable<string> CleanDatabaseStr(MySqlConnection conn, string query)
        {
            var ret = new List<string>();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        var s = r.GetString(0);
                        ret.Add(s);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Checks if the one who is running this program is Spodi.
        /// </summary>
        /// <returns>True if the one who is running this program is probably Spodi; false if it is AN IMPOSTER!!!</returns>
        static bool IsSpodi()
        {
            var drives = DriveInfo.GetDrives();

            // Spodi loves his iRAM! How dare he leave home without it!?
            if (!drives.Any(x => x.Name == "E:\\" && x.VolumeLabel == "iRAM"))
                return false;

            // Dual-core ftw!
            if (Environment.ProcessorCount != 2)
                return false;

            // My PC named its self after me
            if (Environment.MachineName != "SPODI-PC")
                return false;

            // ...or did I name my self after my PC? :o
            if (Environment.UserName != "Spodi")
                return false;

            return true;
        }

        static bool IsValidRootDir()
        {
            var files = Directory.GetFiles(Paths.Root, "*", SearchOption.TopDirectoryOnly);
            return files.Any(x => x.EndsWith("\\NetGore.sln", StringComparison.InvariantCultureIgnoreCase));
        }

        static void Main()
        {
            _delFileRegexes = new RegexCollection(_deleteFilePatterns);
            _preFileRegexes = new RegexCollection(_preserveFilePatterns);

            // Hmm, spend my time programming, or making ASCII art...
            Console.WriteLine(@"             __          __     _____  _   _ _____ _   _  _____");
            Console.WriteLine(@"             \ \        / /\   |  __ \| \ | |_   _| \ | |/ ____|");
            Console.WriteLine(@"              \ \  /\  / /  \  | |__) |  \| | | | |  \| | |  __ ");
            Console.WriteLine(@"               \ \/  \/ / /\ \ |  _  /| . ` | | | | . ` | | |_ |");
            Console.WriteLine(@"                \  /\  / ____ \| | \ \| |\  |_| |_| |\  | |__| |");
            Console.WriteLine(@"                 \/  \/_/    \_\_|  \_\_| \_|_____|_| \_|\_____|");
            Console.WriteLine();
            Console.WriteLine("                          DO NOT RUN THIS PROGRAM!");
            Console.WriteLine();
            Console.WriteLine(
                "This program is intended to be run ONLY by Spodi for setting up official releases. Running this program WILL alter your database contents and DELETE many files!");
            Console.WriteLine();

            if (!IsSpodi())
            {
                // Screen of doom!
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(@"______________________");
                Console.WriteLine(@"|\/        |         /          _.--""""""""""--._          (,_    ,_,    _,)");
                Console.WriteLine(@"|/\________|_______ /         .'             '.        /|\`-._( )_.-'/|\");
                Console.WriteLine(@"| /\       |       /         /                 \      / | \`-'/ \'-`/ | \");
                Console.WriteLine(@"|/  \______|_____ /         ;                   ;    /__|.-'`-\_/-`'-.|__\");
                Console.WriteLine(@"|  / \     |     /          |                   |   `          ""          `");
                Console.WriteLine(@"| /   \____|____/           |                   |");
                Console.WriteLine(@"|/    /\   |   /            ;                   ;");
                Console.WriteLine(@"|    /  \__|__/|             \ (`'--,   ,--'`) /");
                Console.WriteLine(@"|   /   /\ | / |              ) )(')/ _ \(')( (");
                Console.WriteLine(@"|   |  /  \|/  |             (_ `""""` / \ `""""` _)");
                Console.WriteLine(@"|----------/   |              \`'-, /   \ ,-'`/");
                Console.WriteLine(@"|   |  |  /    |               `\ / `'`'` \ /`");
                Console.WriteLine(@"|   |  | / / / | \ \            | _. ; ; ._ |");
                Console.WriteLine(@"|   |  \/ / /  |  \ \           |\ '-'-'-' /|");
                Console.WriteLine(@"|\   \ /  \ \_(*)_/ /           | | _ _ _ | |    _..__.          .__.._");
                Console.WriteLine(@"| \   /    \_(~:~)_/             \ '.;_;.' /.   ^""-.._ '-(\__/)-' _..-""^");
                Console.WriteLine(@"|  \ /      /-(:)-\               \       /           '-.' oo '.-'");
                Console.WriteLine(@"|   /      / / * \ \               ',___,'               `-..-'");
                Console.WriteLine(@"|  /       \ \   / /                q___p");
                Console.WriteLine(@"| /         \     /                 q___p");
                Console.WriteLine(@"|/                                  q___p");
                Console.WriteLine(@"");
                Console.WriteLine("     IMPOSTER! You are not Spodi! Press any key to terminate program...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Root clean directory: " + Paths.Root);

            // Validate run dir
            if (!IsValidRootDir())
            {
                Console.WriteLine("This program may only be run from the project's default build path!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return;
            }

            // Save the schema file
            Console.WriteLine("Updating the database schema file...");
            var schemaFile = Paths.Root + MySqlHelper.DbSchemaFile;
            if (File.Exists(schemaFile))
                File.Delete(schemaFile);

            SchemaSaver.Save();

            if (!File.Exists(schemaFile))
            {
                Console.WriteLine("Failed to create database schema file! Path: {0}", schemaFile);
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return;
            }

#pragma warning disable 162
            if (_buildSchemaOnly)
            {
                Console.WriteLine("_buildSchemaOnly is set - will not progress any farther.");
                Console.WriteLine("Done!");
                return;
            }
#pragma warning restore 162

            // Clean out the items table in the database
            Console.WriteLine("Cleaning out database...");
            CleanDatabase();

            // Dump database file
            Console.WriteLine("Dumping database to file...");
            const string dbfile = "db.sql";
            if (File.Exists(dbfile))
                File.Delete(dbfile);

            RunBatchFile(false,
                         "mysqldump demogame --user=root --password= --host=localhost --all-tables --routines --create-options > db.sql");

            if (!File.Exists(dbfile) || new FileInfo(dbfile).Length < 1000)
            {
                Console.WriteLine("Failed to dump database to db.sql.");
                Console.ReadLine();
            }

            // Move dump file
            Console.WriteLine("Moving dump file to trunk root...");
            var dbfileRooted = Paths.Root + dbfile;
            if (File.Exists(dbfileRooted))
                File.Delete(dbfileRooted);

            File.Move(dbfile, dbfileRooted);

            // Remove version control references
            Console.WriteLine("Deleting version control references...");
            VersionControlCleaner.Run(Paths.Root);

            // Delete crap
            Console.WriteLine("Deleting unneeded files/folders...");
            Deleter.RecursiveDelete(Paths.Root, WillDeleteFile);

            var p = Path.Combine(Paths.Root, "Tools", "PngOptimizer");
            Directory.Delete(p, true);

            // Create self-destroying batch file that will delete this program's binaries
            Console.WriteLine("Creating self-destruct batch file...");
            var programPath = string.Format("{0}CodeReleasePreparer{1}", Paths.Root, Path.DirectorySeparatorChar);
            RunBatchFile(true, "CHOICE /c 1 /d 1 /t 2 > nul", "RMDIR /S /Q \"" + programPath + "bin\"",
                         "RMDIR /S /Q \"" + programPath + "obj\"", "DEL %0");

            Console.WriteLine("Done");
        }

        /// <summary>
        /// Runs a batch file.
        /// </summary>
        /// <param name="async">If false, this method will stall until the batch file finishes.</param>
        /// <param name="lines">The batch file lines to execute.</param>
        static void RunBatchFile(bool async, params string[] lines)
        {
            var filePath = Path.GetTempFileName() + ".bat";
            File.WriteAllLines(filePath, lines);
            var psi = new ProcessStartInfo(filePath)
            { CreateNoWindow = true, UseShellExecute = true, WindowStyle = ProcessWindowStyle.Hidden };

            var p = Process.Start(psi);

            if (p == null)
            {
                Console.WriteLine("Failed to run batch file!");
                Console.WriteLine("Batch file content:");

                if (lines == null || lines.Length == 0)
                    Console.WriteLine("(empty)");
                else
                {
                    for (var i = 0; i < lines.Length; i++)
                    {
                        Console.WriteLine(lines[i]);
                    }
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
                return;
            }

            if (!async)
                p.WaitForExit();
        }

        /// <summary>
        /// Checks if a file will be deleted.
        /// </summary>
        /// <param name="fileName">The path to the file to check.</param>
        /// <returns>True if the file should be deleted; otherwise false.</returns>
        static bool WillDeleteFile(string fileName)
        {
            return _delFileRegexes.Matches(fileName) && !_preFileRegexes.Matches(fileName);
        }
    }
}