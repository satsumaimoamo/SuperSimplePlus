using HarmonyLib;
using UnityEngine;

namespace SuperSimplePlus.Patches
{
    [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
    class ShoutcutKeyBoard
    {
        public static ShipStatus CachedShipStatus = ShipStatus.Instance;
        public static void Postfix(ControllerManager __instance)
        {
            if (AmongUsClient.Instance.GameState == AmongUsClient.GameStates.Started)//ゲームがスタートしているとき
            {
                if (AmongUsClient.Instance.AmHost)//ホストで
                {
                    //廃村
                    if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift))//Aと右左シフトを押したとき
                    {
                        Logger.Info("廃村", "EndGame");
                        ShipStatus.RpcEndGame(GameOverReason.HumansByTask, false);//タスクを終わらせる
                        CachedShipStatus.enabled = false;
                    }
                    //ミーティング強制終了
                    if (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift))//Sと右左シフトを押したとき
                    {
                        Logger.Info("会議強制終了", "MeetingHud");
                        MeetingHud.Instance.RpcClose();//会議を爆破
                    }
                    //ミーティング強制開始
                    if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift))//Zと右左シフトを押したとき
                    {
                        Logger.Info("会議強制開始", "MeetingHud");
                        PlayerControl.LocalPlayer.NoCheckStartMeeting(PlayerControl.LocalPlayer.Data);//自分の死体をレポートする
                    }
                }
            }
        }
    }
}
