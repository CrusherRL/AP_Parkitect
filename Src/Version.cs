using System.Collections.Generic;
using System.Linq;

namespace ArchipelagoMod.Src
{
    class Version
    {
        private static readonly Dictionary<string, string[]> SupportedVersions = new Dictionary<string, string[]>
        {
            // OUR MOD -> AP WORLD VERSION
            { "1.2.1", new[] { "v1.2.1" } },
            { "1.3.0", new[] { "v1.2.1", "v1.3.0" } },
        };

        public string ap_world_version = null;

        public Version(string ap_world_version)
        {
            this.ap_world_version = ap_world_version;
        }

        public bool IsCompatible()
        {
            if (!this.HasVersion())
            {
                return false;
            }
            return SupportedVersions[Constants.VERSION].Contains(this.ap_world_version);
        }

        public string[] Messages()
        {
            if (!this.HasVersion())
            {
                return new[] { "Version Matcher does not include current version! Please Contact the Owner of the Mod!" };
            }

            return new[] { "Your AP World version is not compatible with this mod!nAP World version: {this.ap_world_version}\nMod version: {Constants.VERSION}\n", $"Supported Versions: [{SupportedVersions[Constants.VERSION]}]" };
        }

        private bool HasVersion()
        {
            return SupportedVersions.ContainsKey(Constants.VERSION);
        }
    }
}
