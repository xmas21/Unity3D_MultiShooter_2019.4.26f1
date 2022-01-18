using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;

public class scr_Launcher : MonoBehaviourPunCallbacks
{
    [Header("房間名稱輸入欄位")]
    [SerializeField]
    private TMP_InputField roomNameInputField;
    [Header("錯誤訊息")]
    [SerializeField]
    private TMP_Text errorText;
    [Header("房間名稱")]
    [SerializeField]
    private TMP_Text roomNameText;
    [Header("房間列表")]
    [SerializeField]
    private Transform roomListContent;
    [Header("房間預置物")]
    [SerializeField]
    private GameObject roomListItemPrefab;
    [Header("玩家列表")]
    [SerializeField]
    private Transform playerListContent;
    [Header("玩家預置物")]
    [SerializeField]
    private GameObject playerListItemPrefab;

    public static scr_Launcher launcher;

    #region - 方法 - 

    private void Awake()
    {
        launcher = this;
    }

    private void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    #region - PUN 2 連接伺服器 - 

    /// <summary>
    /// 連接到主伺服器
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    /// <summary>
    /// 連接至大廳
    /// </summary>
    public override void OnJoinedLobby()
    {
        scr_MenuManager.menuManager.OpenMenu("大廳選單");

        Debug.Log("Join Lobby");

        PhotonNetwork.NickName = "Player " + Random.Range(0, 2000).ToString("0000");
    }

    #endregion

    #region - PUN 2 繼承功能 -

    /// <summary>
    /// 加入創建好的房間
    /// </summary>
    public override void OnJoinedRoom()
    {
        scr_MenuManager.menuManager.OpenMenu("房間選單");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<scr_PlayerListItem>().SetUp(players[i]);

        }
    }

    /// <summary>
    /// 創建房間失敗
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message">失敗原因</param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        scr_MenuManager.menuManager.OpenMenu("錯誤選單");
    }

    /// <summary>
    /// 離開房間
    /// </summary>
    public override void OnLeftRoom()
    {
        scr_MenuManager.menuManager.OpenMenu("大廳選單");
    }

    /// <summary>
    /// 更新房間列表
    /// </summary>
    /// <param name="roomList">房間列表</param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform transform in roomListContent)
        {
            Destroy(transform.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<scr_RoomListItem>().Setup(roomList[i]);
        }
    }

    /// <summary>
    /// 新玩家進入房間
    /// </summary>
    /// <param name="newPlayer">新玩家</param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<scr_PlayerListItem>().SetUp(newPlayer);
    }

    #endregion

    #region - 新建功能 - 

    /// <summary>
    /// 創建房間
    /// </summary>
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        scr_MenuManager.menuManager.OpenMenu("載入選單");
    }

    /// <summary>
    /// 離開房間
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        scr_MenuManager.menuManager.OpenMenu("載入選單");
    }

    /// <summary>
    /// 進入房間
    /// </summary>
    /// <param name="info">房間資訊</param>
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        scr_MenuManager.menuManager.OpenMenu("載入選單");
    }

    #endregion
}
