using System;
using System.Diagnostics;
using System.IO;

using System.Linq;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Omegasis.SaveBackup.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace Omegasis.SaveBackup
{
    /// <summary>The mod entry point.</summary>
    public class SaveBackup : Mod
    {
        /*********
        ** Fields
        *********/
        /// <summary>The folder path containing the game's app data.</summary>
        private static readonly string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley");

        /// <summary>The folder path containing the game's saves.</summary>
        private static readonly string SavesPath = Path.Combine(SaveBackup.AppDataPath, "Saves");

        /// <summary>The folder path containing backups of the save before the player starts playing.</summary>
        private static readonly string PrePlayBackupsPath = Path.Combine(SaveBackup.AppDataPath, "Backed_Up_Saves", "Pre_Play_Saves");

        /// <summary>The folder path containing nightly backups of the save.</summary>
        private static readonly string NightlyBackupsPath = Path.Combine(SaveBackup.AppDataPath, "Backed_Up_Saves", "Nightly_InGame_Saves");


        /// <summary>The folder path containing the save data for Android.</summary>
        private static readonly string AndroidDataPath = Constants.DataPath;

        /// <summary>The folder path of the current save data for Android.</summary>
        private static string AndroidCurrentSavePath => Constants.CurrentSavePath;

        /// <summary>The folder path containing nightly backups of the save for Android.</summary>
        private static string AndroidNightlyBackupsPath => Path.Combine(SaveBackup.AndroidDataPath, "Backed_Up_Saves", Constants.SaveFolderName, "Nightly_InGame_Saves");

        /// <summary>The mod configuration.</summary>
        private static ModConfig Config;

        private static IMonitor ModMonitor;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ModMonitor = this.Monitor;
            Config = helper.ReadConfig<ModConfig>();

            if (string.IsNullOrEmpty(Config.AlternatePreplaySaveBackupPath) == false)
            {
                this.BackupSaves(Config.AlternatePreplaySaveBackupPath, false);
            }
            else if(Constants.TargetPlatform != GamePlatform.Android)
            {
                this.BackupSaves(SaveBackup.PrePlayBackupsPath, false);
            }

            helper.Events.GameLoop.Saving += this.OnSaving;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised before the game begins writes data to the save file (except the initial save creation).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnSaving(object sender, SavingEventArgs e)
        {
            if (string.IsNullOrEmpty(Config.AlternateNightlySaveBackupPath) == false)
            {
                this.BackupSaves(Config.AlternateNightlySaveBackupPath);
            }
            else if (Constants.TargetPlatform != GamePlatform.Android)
            {
                this.BackupSaves(SaveBackup.NightlyBackupsPath);
            }
            else
            {
                if (!Directory.Exists(SaveBackup.AndroidNightlyBackupsPath))
                {
                    Directory.CreateDirectory(SaveBackup.AndroidNightlyBackupsPath);
                }
                this.BackupSaves(SaveBackup.AndroidNightlyBackupsPath);
            }
        }

        public class BackgroundCopier
            {
            public ManualResetEvent DoneFlag;
            public string SourceDir;
            public string DestDir;
            public bool Recurse = true;
            public string Filter = null;
            public int SaveCount = SaveBackup.Config.SaveCount;
            public IMonitor Monitor = SaveBackup.ModMonitor;
            public void Copy(object _) {
                this.Monitor.Log($"[{nameof(BackgroundCopier)}] Copying {this.SourceDir} to {this.DestDir} recursively");
                DirectoryCopy(this.SourceDir, this.DestDir, true);
                var parent_dest_dir = new DirectoryInfo(this.DestDir).Parent;
                this.Monitor.Log($"[{nameof(BackgroundCopier)}] Removing old dirs in {parent_dest_dir} ({this.SaveCount} to keep)");
                parent_dest_dir
                    .EnumerateDirectories()
                    .OrderByDescending(f => f.CreationTime)
                    .Skip(Config.SaveCount)
                    .ToList()
                    .ForEach(dir => dir.Delete(true));
                this.DoneFlag.Set();
                }
            ~BackgroundCopier() {
                this.Monitor.Log($"[{nameof(BackgroundCopier)}] Finalized");
                }
            }

        /// <summary>
        /// A class to encapsulate FastZip operation so it can take place in the background,
        /// not blocking the main thread of the game.
        /// </summary>
        public class BackgroundZipper {
            public ManualResetEvent WaitStart;
            public string ZipDir;
            public string ZipName;
            public string SourceDir;
            public bool Recurse = true;
            public string Filter = null;
            public int SaveCount = SaveBackup.Config.SaveCount;
            public IMonitor Monitor = SaveBackup.ModMonitor;
            public void CreateZip(object _) {
                this.Monitor.Log($"[{nameof(BackgroundZipper)}] Waiting for starting flag...");
                this.WaitStart.WaitOne();
                string zip_path = Path.Combine(this.ZipDir, this.ZipName);
                this.Monitor.Log($"[{nameof(BackgroundZipper)}] Creating zip file {zip_path} from {this.SourceDir}");
                FastZip fastZip = new() { UseZip64 = UseZip64.Off };
                fastZip.CreateZip(zip_path, this.SourceDir, this.Recurse, this.Filter);
                // Delete the temporary directory
                this.Monitor.Log($"[{nameof(BackgroundZipper)}] Removing {this.SourceDir}");
                new DirectoryInfo(this.SourceDir).Delete(true);
                // Delete older files
                var to_del = new DirectoryInfo(this.ZipDir)
                    .EnumerateFiles()
                    .OrderByDescending(f => f.CreationTime)
                    .Skip(this.SaveCount)
                    .ToList()
                    ;
                this.Monitor.Log($"[{nameof(BackgroundZipper)}] Removing old zips in {this.ZipDir} ({this.SaveCount} to keep)");
                foreach (var f in to_del) {
                    this.Monitor.Log($"[{nameof(BackgroundZipper)}]   deleting {f}");
                    f.Delete();
                    }
                }
            ~BackgroundZipper() {
                this.Monitor.Log($"[{nameof(BackgroundZipper)}] Finalized");
                }
            }

        /// <summary>Back up saves to the specified folder.</summary>
        /// <param name="folderPath">The folder path in which to generate saves.</param>
        /// <param name="waitForBackgroundCopier">Whether to wait for BackgroundCopier to finish before launching BackgroundZipper</param>
        private void BackupSaves(string folderPath, bool waitForBackgroundCopier = true)
        {
            this.Monitor.Log($"Backing up to {folderPath}");

            string dtstamp = $"{DateTime.Now:yyyyMMdd'-'HHmmss}";
            string backup_path = Path.Combine(folderPath, $"backup-{dtstamp}");
            string source = Constants.TargetPlatform != GamePlatform.Android ? SaveBackup.SavesPath : SaveBackup.AndroidCurrentSavePath;

            ManualResetEvent CopierDone = new(false);

            ModMonitor.Log($"Launching BackgroundCopier");
            BackgroundCopier copier = new() {
                DoneFlag = CopierDone,
                SourceDir = source,
                DestDir = backup_path
                };
            ThreadPool.QueueUserWorkItem(copier.Copy);

            if (waitForBackgroundCopier) {
                ModMonitor.Log("Waiting for BackgroundCopier to finish...");
                CopierDone.WaitOne();
                ModMonitor.Log("BackgroundCopier finished, continuing");
                }

            if (Config.UseZipCompression)
            {
                ModMonitor.Log($"Launching BackgroundZipper");
                BackgroundZipper zipper = new() {
                    WaitStart = CopierDone,
                    ZipDir = folderPath,
                    ZipName = $"backup-{dtstamp}.zip",
                    SourceDir = backup_path
                    };
                ThreadPool.QueueUserWorkItem(zipper.CreateZip);
            }

            /*
            // back up saves This used compression but it always causes a library loading issue for OS
            Directory.CreateDirectory(folderPath);
            ZipFile.CreateFromDirectory(SaveBackup.SavesPath, Path.Combine(folderPath, $"backup-{DateTime.Now:yyyyMMdd'-'HHmmss}.zip"));

            // delete old backups

            */
        }

        /// <summary>
        /// An uncompressed output method.
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                ModMonitor.Log($"Creating directory {destDirName}");
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        /// <summary>
        /// <para><see href="https://github.com/Pathoschild/SMAPI/blob/develop/src/SMAPI.Mods.SaveBackup/ModEntry.cs"/></para>
        /// <para>This code originated from Pathoschilds SMAPI SaveBackup Mod. All rights for this code goes to them.
        /// If this code is not allowed to be here *please* contact Omegasis to discus a proper work around.
        /// Create a zip using a process command on MacOS.</para>
        /// </summary>
        /// <param name="sourcePath">The file or directory path to zip.</param>
        /// <param name="destination">The destination file to create.</param>
        private void CompressUsingMacProcess(string sourcePath, FileInfo destination)
        {
            DirectoryInfo saveFolder = new DirectoryInfo(sourcePath);
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "zip",
                Arguments = $"-rq \"{destination.FullName}\" \"{saveFolder.Name}\" -x \"*.DS_Store\" -x \"__MACOSX\"",
                WorkingDirectory = $"{saveFolder.FullName}/../",
                CreateNoWindow = true
            };
            new Process { StartInfo = startInfo }.Start();
        }
    }
}
