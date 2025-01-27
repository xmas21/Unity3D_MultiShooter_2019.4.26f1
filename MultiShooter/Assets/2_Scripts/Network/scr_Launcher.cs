﻿using UnityEngine;
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
    [Header("開始遊戲按鈕")]
    [SerializeField]
    private GameObject startGameButton;
    [SerializeField] [Header("玩家名稱文字")] TMP_InputField userNameInput;

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

        if (PlayerPrefs.HasKey("username"))
        {
            userNameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = userNameInput.text;
        }
        else
        {
            userNameInput.text = "Player " + Random.Range(0, 10000).ToString("0000");
            userNameInputValueChanged();
        }
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
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    /// <summary>
    /// 連接至大廳
    /// </summary>
    public override void OnJoinedLobby()
    {
        scr_MenuManager.menuManager.OpenMenu("大廳選單");
        Debug.Log("Join Lobby");
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

        foreach (Transform trans in playerListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<scr_PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);  // 只有房主可以開始遊戲
    }

    /// <summary>
    /// 切換房長
    /// </summary>
    /// <param name="newMasterClient">房長</param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);  // 只有房主可以開始遊戲
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
            if (roomList[i].RemovedFromList)
            {
                continue;
            }

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
    /// 進入房間
    /// </summary>
    /// <param name="info">房間資訊</param>
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        scr_MenuManager.menuManager.OpenMenu("載入選單");
    }

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
    /// 開始遊戲
    /// </summary>
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("遊戲場景");
    }

    /// <summary>
    /// 玩家更改姓名
    /// </summary>
    public void userNameInputValueChanged()
    {
        PhotonNetwork.NickName = userNameInput.text;
        PlayerPrefs.SetString("username", userNameInput.text);
    }

    #endregion
}
