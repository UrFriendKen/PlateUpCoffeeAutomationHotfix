using Kitchen;
using KitchenLib;
using System.Reflection;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using System.Linq;
using System.Threading;

// Namespace should have "Kitchen" in the beginning
namespace KitchenCoffeeAutomationHotfix
{
    public class Main : BaseMod
    {
        // guid must be unique and is recommended to be in reverse domain name notation
        // mod name that is displayed to the player and listed in the mods menu
        // mod version must follow semver e.g. "1.2.3"
        public const string MOD_GUID = "IcedMilo.PlateUp.CoffeeAutomationHotfix";
        public const string MOD_NAME = "CoffeeAutomationHotfix";
        public const string MOD_VERSION = "0.2.0";
        public const string MOD_AUTHOR = "IcedMilo";
        public const string MOD_GAMEVERSION = ">=1.1.1";

        public static string PlateUpVersion;

        // Game version this mod is designed for in semver
        // e.g. ">=1.1.1" current and all future
        // e.g. ">=1.1.1 <=1.2.3" for all from/until

        public Main() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }
        protected override void Initialise()
        {
            base.Initialise();
            // For log file output so the official plateup support staff can identify if/which a mod is being used
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
            PlateUpVersion = Application.version.Split(' ')[0];
        }
        #region Logging
        // You can remove this, I just prefer a more standardized logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }

}
