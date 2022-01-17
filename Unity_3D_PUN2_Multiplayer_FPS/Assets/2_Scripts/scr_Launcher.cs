using UnityEngine;
using Photon.Pun;
using TMPro;

public class scr_Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField roomNameInputField;
    [SerializeField]
    private TMP_Text errorText;
    [SerializeField]
    private TMP_Text roomNameText;

    private void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

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
    }

    /// <summary>
    /// 加入房間
    /// </summary>
    public override void OnJoinedRoom()
    {
        scr_MenuManager.menuManager.OpenMenu("房間選單");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
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

}
