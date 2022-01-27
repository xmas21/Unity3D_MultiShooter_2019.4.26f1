using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class scr_Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;

    Dictionary<Player, scr_ScoreboardItem> scoreboardItems = new Dictionary<Player, scr_ScoreboardItem>();

    void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }
    }

    /// <summary>
    /// 進房間
    /// </summary>
    /// <param name="newPlayer">新玩家</param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreboardItem(newPlayer);
    }

    /// <summary>
    /// 離開房間
    /// </summary>
    /// <param name="otherPlayer">舊玩家</param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreboardItem(otherPlayer);
    }

    /// <summary>
    /// 增加玩家列表
    /// </summary>
    /// <param name="player">玩家</param>
    void AddScoreboardItem(Player player)
    {
        scr_ScoreboardItem item = Instantiate(scoreboardItemPrefab, container).GetComponent<scr_ScoreboardItem>();
        item.Initialize(player);
        scoreboardItems[player] = item;
    }

    /// <summary>
    /// 移除玩家列表
    /// </summary>
    /// <param name="player">出去的玩家</param>
    void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }

}
