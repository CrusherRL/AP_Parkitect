using ArchipelagoMod.Src;
using ArchipelagoMod.Src.Config;
using System.Drawing.Printing;
using UnityEngine;

namespace Archipelago.Src.UI
{
    static class ArchipelagoSettingsUI
    {
        public static void Draw()
        {
            string configPath = ParkitectAPConfig.GetConfigFilePath();
            GUILayoutOption width = GUILayout.Width(200f);
            GUIStyle defaultStyle = ArchipelagoSettingsUI.GetTextFieldStyle();
            ArchipelagoSettings instance = ScriptableSingleton<ArchipelagoSettings>.Instance;

            // Version
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Version: {Constants.VERSION}");
            GUILayout.EndHorizontal();

            // Versioning
            GUILayout.BeginHorizontal();
            GUILayout.Label("Supported AP World versions:");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            ArchipelagoSettingsUI.DisabledTextArea(new ArchipelagoMod.Src.Version().GetSupportedVersions());
            GUILayout.EndHorizontal();

            // Config Info
            GUILayout.BeginHorizontal();
            GUILayout.Label("Path to Config:");
            if (GUILayout.Button("Copy", GUILayout.Width(80f)))
            {
                GUIUtility.systemCopyBuffer = configPath;
                GUI.FocusControl(null);
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            ArchipelagoSettingsUI.DisabledTextArea(configPath);
            GUILayout.EndHorizontal();

            ArchipelagoSettingsUI.HorizontalLine();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Archipelago Connection:");
            GUILayout.EndHorizontal();

            // Changeable Content
            // Address
            GUILayout.BeginHorizontal();
            GUILayout.Label("Address:");
            instance.ParkitectAPConfig.Address = GUILayout.TextField(instance.ParkitectAPConfig.Address, defaultStyle, width);
            GUILayout.EndHorizontal();

            // Port
            GUILayout.BeginHorizontal();
            GUILayout.Label("Port:");
            string portInput = instance.ParkitectAPConfig.Port.ToString();
            portInput = GUILayout.TextField(portInput, defaultStyle, width);
            GUILayout.EndHorizontal();
            if (portInput.Length == 0)
            {
                portInput = portInput.Substring(0, 5);
            }
            if (string.IsNullOrWhiteSpace(portInput))
            {
                instance.ParkitectAPConfig.Port = 0;
            }
            else if (int.TryParse(portInput, out int port))
            {
                instance.ParkitectAPConfig.Port = port;
            }

            // Playername
            GUILayout.BeginHorizontal();
            GUILayout.Label("Playername:");
            instance.ParkitectAPConfig.Playername = GUILayout.TextField(instance.ParkitectAPConfig.Playername, defaultStyle, width);
            GUILayout.EndHorizontal();

            // Password
            GUILayout.BeginHorizontal();
            GUILayout.Label("Password:");
            instance.ParkitectAPConfig.Password = GUILayout.TextField(instance.ParkitectAPConfig.Password, defaultStyle, width);
            GUILayout.EndHorizontal();

            // Helper Buttons
            GUILayout.BeginHorizontal();
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.margin = defaultStyle.margin;
            if (GUILayout.Button("Load", buttonStyle))
            {
                instance.Load();
                GUI.FocusControl(null);
            }
            if (GUILayout.Button("Save", buttonStyle))
            {
                instance.Save();
                GUI.FocusControl(null);
            }
            GUILayout.EndHorizontal();
        }

        public static GUIStyle GetTextFieldStyle(GUIStyle skin = null, int margin = 15, int padding = 5)
        {
            GUIStyle style = new GUIStyle(skin ?? GUI.skin.textField);

            style.wordWrap = true;
            style.margin = new RectOffset(
                margin,
                margin,
                margin,
                margin
            );
            style.padding = new RectOffset(
                padding,
                padding,
                padding,
                padding
            );

            return style;
        }

        public static void HorizontalLine()
        {
            GUIStyle style = new GUIStyle
            {
                normal =
                {
                    background = Texture2D.whiteTexture
                },
                margin = new RectOffset(0, 0, 4, 4),
                fixedHeight = 1f
            };
            Color color2 = GUI.color;
            Color white = Color.white;
            white.a = 0.3f;
            GUI.color = (white);
            GUILayout.Box(GUIContent.none, style);
            GUI.color = color2;
        }

        public static void DisabledTextArea(string text)
        {
            GUIStyle wrappedStyle = new GUIStyle(GUI.skin.textArea);
            wrappedStyle.wordWrap = true;
            GUI.enabled = false;
            GUILayout.TextArea(text, ArchipelagoSettingsUI.GetTextFieldStyle(GUI.skin.textArea), GUILayout.ExpandWidth(true));
            GUI.enabled = true;
        }
    }
}
