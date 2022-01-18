using UnityEngine;
using Photon.Realtime;
using TMPro;

public class scr_RoomListItem : MonoBehaviour
{
    [Header("房間名稱")]
    [SerializeField]
    private TMP_Text text;

    private RoomInfo info;

    /// <summary>
    /// 設定房間資訊
    /// </summary>
    /// <param name="_info">房間資訊</param>
    public void Setup(RoomInfo _info)
    {
        info = _info;
        text.text = _info.Name;
    }

    /// <summary>
    /// 按鈕觸發器
    /// </summary>
    public void Onclick()
    {
        scr_Launcher.launcher.JoinRoom(info);
    }
}
