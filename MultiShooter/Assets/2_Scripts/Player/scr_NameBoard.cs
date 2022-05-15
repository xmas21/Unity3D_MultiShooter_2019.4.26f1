using UnityEngine;
using Photon.Pun;
using TMPro;

public class scr_NameBoard : MonoBehaviour
{
    [SerializeField] PhotonView player_pv;
    [SerializeField] TMP_Text text;

    Camera cam;

    void Start()
    {
        if (player_pv.IsMine)
        {
            gameObject.SetActive(false);
        }
        text.text = player_pv.Owner.NickName;
    }

    void Update()
    {
        CheckCam();
        CamFollow();
    }

    /// <summary>
    /// 尋找攝影機
    /// </summary>
    void CheckCam()
    {
        if (cam == null)
        {
            cam = FindObjectOfType<Camera>();
        }

        if (cam == null) return;
    }

    /// <summary>
    /// 名牌追蹤攝影機
    /// </summary>
    void CamFollow()
    {
        transform.LookAt(cam.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
