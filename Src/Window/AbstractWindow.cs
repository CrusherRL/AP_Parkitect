using System;
using UnityEngine;
using UnityEngine.UI;
using ArchipelagoMod.Src.Window.Scripts;

namespace ArchipelagoMod.Src.Window
{
    abstract class AbstractWindow : MonoBehaviour
    {
        protected GameObject GameObject = null;
        protected GameObject Prefab = null;
        protected AssetBundle AssetBundle = null;

        public abstract KeyCode KeyCode { get; set; }
        public abstract string BundleFilename { get; set; }

        public bool IsActive = false;

        // -----------------------------
        // MonoBehaviour
        // -----------------------------
        void Awake ()
        {
            string OS = this.GetPlatformFolder();
            Helper.Debug($"[AbstractWindow::Awake] Operating System: {OS}");
            string bundlePath = System.IO.Path.Combine(Constants.ModPath, OS, this.BundleFilename);

            // Load AssetBundle from file
            this.AssetBundle = AssetBundle.LoadFromFile(bundlePath);

            if (AssetBundle == null)
            {
                Helper.Debug($"[AbstractWindow::Awake] Assetbundle not found - {OS} -> {this.BundleFilename}");
                return;
            }

            this.Prefab = AssetBundle.LoadAsset<GameObject>("Canvas");
            this.GameObject = MonoBehaviour.Instantiate(this.Prefab);
            this.SetCloseButtonListener();
            this.AddDraggableScript();

            this.SetActiveState();

            this.OnAwake();
            this.Done();
        }

        void Update ()
        {
            if (Input.GetKeyDown(this.KeyCode))
            {
                this.ToggleActiveState();
            }
        }

        // -----------------------------
        // Custom things lol
        // -----------------------------

        private void SetCloseButtonListener ()
        {
            Transform ButtonCloseChild = this.GetChild("Frame/Header/ButtonClose");
            Button ButtonClose = ButtonCloseChild.transform.GetComponent<Button>();

            if (ButtonClose == null)
            {
                Helper.Debug($"[AbstractWindow::SetCloseButtonListener] No Close Button found");
            }

            ButtonClose.onClick.AddListener(this.OnCloseButton);
        }

        private void OnCloseButton ()
        {
            this.IsActive = false;
        }

        // Do your stuff and dont forget to call this.Done() at the end :)
        public abstract void OnAwake ();

        public void Done ()
        {
            try
            {
                this.AssetBundle.Unload(false);
            } catch (Exception e)
            {
                Helper.Debug($"[AbstractWindow::SetCloseButtonListener] Unloading Assetbundle failed - {e.Message}");
            }
        }

        public void ToggleActiveState()
        {
            this.IsActive = !this.IsActive;
            this.SetActiveState();
        }

        public void SetActiveState()
        {
            this.GetChild("Frame").transform.gameObject.SetActive(this.IsActive);
        }

        // -----------------------------
        // Helpers
        // -----------------------------

        public Transform GetChild (string hierarchy)
        {
            return this.GameObject.transform.Find(hierarchy);
        }

        public void AddDraggableScript(string element = "Frame")
        {
            this.GetChild(element).gameObject.AddComponent<DragUI>();
        }

        public string GetPlatformFolder()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                    return "macOS";
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.LinuxEditor:
                    return "Linux";
                default:
                    return "Unknown";
            }
        }
    }
}
