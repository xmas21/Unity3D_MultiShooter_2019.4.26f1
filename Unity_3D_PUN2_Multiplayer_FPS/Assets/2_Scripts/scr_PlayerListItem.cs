using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class scr_PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] 
    [Header("玩家名稱")]
    private TMP_Text text;

    private Player player;

    /// <summary>
    /// 設定角色資料
    /// </summary>
    /// <param name="_player">角色資料</param>
    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
    }

    /// <summary>
    /// 玩家離開房間
    /// </summary>
    /// <param name="otherPlayer">離開的玩家</param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer) // 假如玩家離開房間 > 刪除房間
        {
            Destroy(gameObject); 
        }
    }

    /// <summary>
    /// 離開房間
    /// </summary>
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

}
