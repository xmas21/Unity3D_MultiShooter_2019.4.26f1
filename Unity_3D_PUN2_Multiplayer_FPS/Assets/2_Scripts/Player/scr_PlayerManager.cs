using System.IO;
using UnityEngine;
using Photon.Pun;

public class scr_PlayerManager : MonoBehaviour
{
    [Header("玩家控制器")]
    public GameObject controller;

    PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (pv.IsMine)
        {
            CreateController();
        }
    }

    /// <summary>
    /// 角色死亡
    /// </summary>
    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }

    /// <summary>
    /// 創建角色
    /// </summary>
    void CreateController()
    {
        Transform spwanPoints = scr_SpawnManager.spawnManager.GetSpawnPoint();

        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "玩家"), spwanPoints.position, spwanPoints.rotation, 0, new object[] { pv.ViewID });
    }



}
