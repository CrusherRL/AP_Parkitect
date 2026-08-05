using ArchipelagoMod.Src;
using ArchipelagoMod.Src.Controller;
using ArchipelagoMod.Src.Migration;
using ArchipelagoMod.Src.UI;
using ArchipelagoMod.Src.Window;
using UnityEngine;

namespace ArchipelagoMod
{
    public class Archipelago : AbstractMod, IModSettings
    {
        public const string VERSION_NUMBER = Constants.VERSION;
        public override string getIdentifier() => Constants.IDENTIFIER;
        public override string getName() => "Archipelago Mod";
        public override string getDescription() => @"A Connector to Archipelago within Parkitect";
        public string path { get; set; }

        public override string getVersionNumber() => VERSION_NUMBER;
        public override bool isMultiplayerModeCompatible() => false;
        public override bool isRequiredByAllPlayersInMultiplayerMode() => true;
        
        public string ModName = "Archipelago_Mod";

        public GameObject GameObject;

        private SaveData SaveData = null;

        private ParkitectController ParkitectController = null;
        private ArchipelagoController ArchipelagoController = null;

        private DebuggerWindow DebuggerWindow = null;
        private ArchipelagoWindow ArchipelagoWindow = null;

        public void OnBeforeStart()
        {
            Constants.ModPath = System.IO.Path.Combine(GameController.modsPath, Constants.ParkitectAPFolder) + System.IO.Path.DirectorySeparatorChar;
            new MigrationHelper().RunMigrations();
        }

        public override void onEnabled()
        {
            Helper.Debug("=============================================");

            this.OnBeforeStart();
            ScriptableSingleton<ArchipelagoSettings>.Instance.Load();

            // Sicherheitsnetz: Falls aus irgendeinem Grund noch ein altes Objekt mit diesem Namen existiert, löschen wir es vorab
            GameObject oldInstance = GameObject.Find(this.ModName);
            if (oldInstance != null)
            {
                UnityEngine.Object.DestroyImmediate(oldInstance);
            }

            this.GameObject = new GameObject(this.ModName);
            this.SaveData = this.GameObject.AddComponent<SaveData>();
            this.ParkitectController = this.GameObject.AddComponent<ParkitectController>();
            this.DebuggerWindow = this.GameObject.AddComponent<DebuggerWindow>();
            this.ArchipelagoWindow = this.GameObject.AddComponent<ArchipelagoWindow>();
            this.ArchipelagoController = this.GameObject.AddComponent<ArchipelagoController>();

            this.ArchipelagoWindow.SetVersion(VERSION_NUMBER);
        }

        public override void onDisabled()
        {
            UnityEngine.Object.Destroy(this.GameObject);
        }
        public void onDrawSettingsUI()
        {
            ArchipelagoSettingsUI.Draw();
        }
        public void onSettingsOpened()
        {
            this.OnBeforeStart();
            ScriptableSingleton<ArchipelagoSettings>.Instance.Load();
        }

        public void onSettingsClosed()
        {
        }
    }
}