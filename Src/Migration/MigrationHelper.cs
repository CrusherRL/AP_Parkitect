namespace ArchipelagoMod.Src.Migration
{
    public class MigrationHelper
    {
        public void RunMigrations()
        {
            new APConfigMigration().Execute();
            new SaveGameMigration().Execute();
            new DebugMigration().Execute();
        }
    }
}
