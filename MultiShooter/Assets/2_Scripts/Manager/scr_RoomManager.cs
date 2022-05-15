using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class scr_RoomManager : MonoBehaviourPunCallbacks
{
    public static scr_RoomManager roomManager;

    [SerializeField] GameObject scoreboardCanvas;

    void Awake()
    {
        if (roomManager)          // Check if other roomManager exists
        {
            Destroy(gameObject);  // i go bye bye
            return;
        }
        DontDestroyOnLoad(gameObject);  // only this scripts
        roomManager = this;
    }

    void Update()
    {
        Onclick();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneloaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneloaded;

    }

    /// <summary>
    /// 載入場景
    /// </summary>
    /// <param name="scene">現在的場景</param>
    /// <param name="loadSceneMode">場景模式</param>
    void OnSceneloaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1) // in the game scene
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "玩家管理器"), Vector3.zero, Quaternion.identity);
        }
    }

    /// <summary>
    /// 偵測按鈕
    /// </summary>
    void Onclick()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboardCanvas.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboardCanvas.SetActive(false);
        }
    }
}
