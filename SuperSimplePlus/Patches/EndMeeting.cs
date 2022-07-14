using HarmonyLib;
using UnityEngine;

namespace SuperSimplePlus.Patches
{
    [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
    class DebugManager
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
                        ShipStatus.RpcEndGame(GameOverReason.HumansByTask, false);//タスクを終わらせる
                        CachedShipStatus.enabled = false;
                    }
                    //ミーティング強制終了
                    if (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift))//Sと右左シフトを押したとき
                    {
                        MeetingHud.Instance.RpcClose();//会議を爆破
                    }
                }
            }
        }
    }
}
