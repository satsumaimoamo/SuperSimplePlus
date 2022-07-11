using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;

namespace SuperSimplePlus
{
    [BepInPlugin(Id, "SuperSimplePlus", VersionString)]
    [BepInProcess("Among Us.exe")]
    public class SuperSimplePlusPlugin : BasePlugin
    {
        public const string Id = "com.satsumaimoamo.SuperSimplePlus";

        public const string VersionString = "1.0.0";

        public static System.Version Version = System.Version.Parse(VersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;
        public static Sprite ModStamp;
        public static int optionsPage = 1;
        public Harmony Harmony { get; } = new Harmony(Id);
        public static SuperSimplePlusPlugin Instance;
        public static Dictionary<string, Dictionary<int, string>> StringDATE;
        public static bool IsUpdate = false;
        public static string NewVersion = "";
        public static string thisname;

        public override void Load()
        {
            Logger = Log;
            Instance = this;
            // All Load() Start
            // All Load() End

            // Old Delete Start

            try
            {
                DirectoryInfo d = new(Path.GetDirectoryName(Application.dataPath) + @"\BepInEx\plugins");
                string[] files = d.GetFiles("*.dll.old").Select(x => x.FullName).ToArray(); // Getting old versions
                foreach (string f in files)
                    File.Delete(f);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception occured when clearing old versions:\n" + e);
            }


            var assembly = Assembly.GetExecutingAssembly();

            StringDATE = new Dictionary<string, Dictionary<int, string>>();
            Harmony.PatchAll();
        }
    }
}

