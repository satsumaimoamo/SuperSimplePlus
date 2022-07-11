//引用=>https://github.com/reitou-mugicha/TownOfSuper/blob/main/TownOfSuper/Main.cs
using System;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Configuration;
using HarmonyLib;

namespace TownOfSuper
{
    [BepInPlugin(Id, "SuperSimplePlus", Version)]
    public class SSPPlugin : BasePlugin
    {
        public const String Id = "jp.satsumaimoamo.SuperSimplePlus";
        public const String Version = "1.0.0";

        public static ConfigEntry<bool> debugTool { get; set; }
        public static ConfigEntry<string> StereotypedText { get; set; }
        public Harmony Harmony = new Harmony(Id);

        public override void Load()
        {

            debugTool = Config.Bind("Client Options", "Debug Tool", false);
            StereotypedText = Config.Bind("Client Options", "StereotypedText", "TownOfSuper定型文");

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
            __instance.text.text += " & <color=#ffa500>Super</color><color=#ff0000>Simple</color><color=#00ff00>Plus</color> ver." + SSPPlugin.Version; //<color=#ffddef>AZ</color>
        }
    }
}
