using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace ArchipelagoMod.Src
{
    public static class FileExplorer
    {
        public static void OpenFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    string explorerPath = path.Replace('/', '\\');
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = $"\"{explorerPath}\"",
                        UseShellExecute = true
                    });
                    break;

                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    Process.Start("open", $"\"{path}\"");
                    break;

                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                    Process.Start("xdg-open", $"\"{path}\"");
                    break;

                default:
                    break;
            }
        }
    }
}
