using System.IO;
using UnityEngine;
using Photon.Pun;

public class scr_PlayerManager : MonoBehaviour
{
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (pv.IsMine)
        {
            CreateController();
        }
    }

    /// <summary>
    /// 創建角色
    /// </summary>
    private void CreateController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs" , "玩家"), Vector3.zero, Quaternion.identity);
    }

}
