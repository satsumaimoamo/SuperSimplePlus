//引用=>https://github.com/reitou-mugicha/TownOfSuper/blob/main/TownOfSuper/Main.cs
using System;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Configuration;
using HarmonyLib;

namespace SuperSimplePlus
{
    [BepInPlugin(Id, "SuperSimplePlus", Version)]
    public class SSPPlugin : BasePlugin
    {
        public const String Id = "jp.satsumaimoamo.SuperSimplePlus";
        public const String Version = "1.0.0";

        public static ConfigEntry<bool> debugTool { get; set; }
        public static ConfigEntry<string> StereotypedText { get; set; }
        public Harmony Harmony = new Harmony(Id);
        internal static BepInEx.Logging.ManualLogSource Logger;

        public override void Load()
        {
            Logger = Log;

            debugTool = Config.Bind("Client Options", "Debug Tool", false);
            StereotypedText = Config.Bind("Client Options", "StereotypedText", "SuperSimplePlus定型文");

            Harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
    public class ShowModStampPatch
    {
        public static void Postfix(ModManager __instance)
        {
            __instance.ShowModStamp();
        }
    }

    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public class VersionShowerPatch
    {
        public static void Postfix(VersionShower __instance)
        {
            __instance.text.text += " + <color=#ff0000>SuperSimplePlus</color> ver." + SSPPlugin.Version; //<color=#ffddef>AZ</color>
        }
    }
}
