using UnityEngine;

namespace ArchipelagoMod.Src.Migration
{
    public class APConfigMigration
    {
        protected string OldFolder = System.IO.Path.Combine(Application.persistentDataPath, "Parkitect_Archipelago");
        protected string NewFolder = System.IO.Path.Combine(Application.persistentDataPath, Constants.ParkitectAPFolder);

        protected string OldFileName = "config_parkitect.json";
        protected string NewFileName = Constants.ParkitectAPFilename;

        public void Execute()
        {
            Constants.ConfigPath = this.NewFolder;

            // Old Folder alive? rename it
            if (System.IO.Directory.Exists(this.OldFolder))
            {
                System.IO.Directory.Move(this.OldFolder, this.NewFolder);
            }

            // New Folder not exist? create
            if (!System.IO.Directory.Exists(this.NewFolder))
            {
                System.IO.Directory.CreateDirectory(this.NewFolder);
            }

            string OldFile = System.IO.Path.Combine(this.NewFolder, this.OldFileName);
            string NewFile = System.IO.Path.Combine(this.NewFolder, this.NewFileName);

            // rename old file
            if (System.IO.File.Exists(OldFile))
            {
                System.IO.Directory.Move(OldFile, NewFile);
            }
        }
    }
}
