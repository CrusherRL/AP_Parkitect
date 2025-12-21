using ArchipelagoMod.Src;
using ArchipelagoMod.Src.Controller;
using ArchipelagoMod.Src.Window;
using UnityEngine;

namespace ArchipelagoMod
{
    public class Archipelago : AbstractMod
    {
        public const string VERSION_NUMBER = "1.0.0";
        public override string getIdentifier() => "com.parkitectCommunity.Archipelago";
        public override string getName() => "Archipelago Mod";
        public override string getDescription() => @"A Connector to Archipelago within Parkitect";
        public string path { get; set; }

        public override string getVersionNumber() => VERSION_NUMBER;
        public override bool isMultiplayerModeCompatible() => false;
        public override bool isRequiredByAllPlayersInMultiplayerMode() => true;

        public GameObject GameObject;

        private SaveData SaveData = null;

        private ParkitectController ParkitectController = null;
        private ArchipelagoController ArchipelagoController = null;

        private DebuggerWindow DebuggerWindow = null;
        private ArchipelagoWindow ArchipelagoWindow = null;

        public override void onEnabled()
        {
            Constants.ModPath = GameController.modsPath + System.IO.Path.Combine("Archipelago") + System.IO.Path.DirectorySeparatorChar;
            Helper.Debug("=============================================");

            this.GameObject = new GameObject();

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
    }
}