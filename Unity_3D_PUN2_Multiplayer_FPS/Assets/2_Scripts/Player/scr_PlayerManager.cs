using System.IO;
using UnityEngine;
using Photon.Pun;

public class scr_PlayerManager : MonoBehaviour
{
    public GameObject controller;

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
        Transform spwanPoints = scr_SpawnManager.spawnManager.GetSpawnPoint();

        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "玩家"), spwanPoints.position, spwanPoints.rotation, 0, new object[] { pv.ViewID });
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }

}
