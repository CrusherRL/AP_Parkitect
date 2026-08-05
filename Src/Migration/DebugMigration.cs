using UnityEngine;

namespace ArchipelagoMod.Src.Migration
{
    public class DebugMigration
    {
        protected string OldFilePath = System.IO.Path.Combine(Constants.ModPath, Constants.ParkitectDebugLogFilename);
        protected string NewFilePath = System.IO.Path.Combine(Constants.ConfigPath, Constants.ParkitectDebugLogFilename);

        public void Execute()
        {
            // Old Folder alive? rename it
            if (System.IO.File.Exists(this.OldFilePath))
            {
                System.IO.File.Move(this.OldFilePath, this.NewFilePath);
            }
        }
    }
}
