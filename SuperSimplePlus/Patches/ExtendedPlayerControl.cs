//引用=>https://github.com/tukasa0001/TownOfHost/blob/main/Modules/ExtendedPlayerControl.cs
namespace SuperSimplePlus.Patches
{
    public static class ExtendedPlayerControl
    {
        public static void NoCheckStartMeeting(this PlayerControl reporter, GameData.PlayerInfo target)
        { /*サボタージュ中でも関係なしに会議を起こせるメソッド
            targetがnullの場合はボタンとなる*/
            MeetingRoomManager.Instance.AssignSelf(reporter, target);
            DestroyableSingleton<HudManager>.Instance.OpenMeetingRoom(reporter);
            reporter.RpcStartMeeting(target);
        }
    }
}
