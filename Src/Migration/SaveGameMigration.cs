namespace ArchipelagoMod.Src.Migration
{
    public class SaveGameMigration
    {
        protected string OldFolderPath = System.IO.Path.Combine(Constants.ModPath, Constants.ParkitectSavegamesFolder) + System.IO.Path.DirectorySeparatorChar;
        protected string NewFolderPath = System.IO.Path.Combine(Constants.ConfigPath, Constants.ParkitectSavegamesFolder) + System.IO.Path.DirectorySeparatorChar;

        public void Execute()
        {
            if (System.IO.Directory.Exists(this.OldFolderPath))
            {
                System.IO.Directory.Move(this.OldFolderPath, this.NewFolderPath);
            }

            if (!System.IO.Directory.Exists(this.NewFolderPath))
            {
                System.IO.Directory.CreateDirectory(this.NewFolderPath);
            }
       
            Constants.SaveGamesPath = this.NewFolderPath;
        }
    }
}
