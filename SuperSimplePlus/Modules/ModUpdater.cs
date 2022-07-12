using System.Collections;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json.Linq;
using HarmonyLib;
using UnityEngine.UI;
using Version = SemanticVersioning.Version;
using BepInEx;

namespace SuperSimplePlus.Modules
{
    public class ModUpdater
    {
        public static string Tag;
        public static JObject data;

        public async static Task<bool> IsNewer()//最新版かどうか判定
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "SuperSimplePlus Updater");

            var req = await client.GetAsync("https://api.github.com/repos/satsumaimoamo/SuperSimplePlus/releases/latest", HttpCompletionOption.ResponseContentRead);
            if (!req.IsSuccessStatusCode) return false;

            var dataString = await req.Content.ReadAsStringAsync();
            data = JObject.Parse(dataString);

            Tag = data["tag_name"]?.ToString().TrimStart('v');
            SSPPlugin.Logger.LogInfo($"最新版かどうか判定\ngithub:{Tag}\n入ってるバージョン:{SSPPlugin.Version}");

            if (!Version.TryParse(Tag, out var myVersion)) return false;
            return myVersion.BaseVersion() > Version.Parse(SSPPlugin.Version);
        }
        public async static Task<bool> DownloadUpdate()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "SuperSimplePlus Updater");

            JToken assets = data["assets"];
            string downloadURI = "";
            string browser_download_url = assets.First["browser_download_url"]?.ToString();
            if (browser_download_url != null && assets.First["content_type"] != null)
            {
                if (assets.First["content_type"].ToString().Equals("application/x-msdownload") &&
                    browser_download_url.EndsWith(".dll"))
                {
                    downloadURI = browser_download_url;
                }
            }
            if (downloadURI.Length == 0) return false;

            var res = await client.GetAsync(downloadURI, HttpCompletionOption.ResponseContentRead);
            string filePath = Path.Combine(Paths.PluginPath, "SuperSimplePlus.dll");
            if (File.Exists(filePath + ".old")) File.Delete(filePath + ".old");
            if (File.Exists(filePath)) File.Move(filePath, filePath + ".old");

            await using var responseStream = await res.Content.ReadAsStreamAsync();
            await using var fileStream = File.Create(filePath);
            await responseStream.CopyToAsync(fileStream);

            return true;
        }

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
        public class MainMenuStartPatch
        {
            public static void Postfix(MainMenuManager __instance)
            {
                Task<bool> SSPUpdateCheck = Task.Run(IsNewer);
                if (SSPUpdateCheck.Result) //最新版じゃなかったらボタンを作る
                {
                    SSPPlugin.Logger.LogInfo("古いバージョンです");
                    var template = GameObject.Find("ExitGameButton");
                    if (template == null) return;

                    var buttonSSPUpdate = UnityEngine.Object.Instantiate(template, null);
                    buttonSSPUpdate.transform.localPosition = new Vector3(buttonSSPUpdate.transform.localPosition.x, buttonSSPUpdate.transform.localPosition.y + 0.6f, buttonSSPUpdate.transform.localPosition.z);

                    var textSSPUpdateButton = buttonSSPUpdate.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                    __instance.StartCoroutine(Effects.Lerp(0.1f, new System.Action<float>((p) =>
                    {
                        textSSPUpdateButton.SetText("<color=#ff0000>SuperSimplePlus</color>をアップデートする");
                    })));

                    PassiveButton passiveButtonSSPUpdate = buttonSSPUpdate.GetComponent<PassiveButton>();

                    passiveButtonSSPUpdate.OnClick = new Button.ButtonClickedEvent();
                    passiveButtonSSPUpdate.OnClick.AddListener((System.Action)(() => Task.Run(DownloadUpdate)));
                }
            }
        }
    }
}
