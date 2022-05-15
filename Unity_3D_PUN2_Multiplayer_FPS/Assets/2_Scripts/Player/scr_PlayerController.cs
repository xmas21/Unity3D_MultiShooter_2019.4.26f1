using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;

public class scr_PlayerController : MonoBehaviourPunCallbacks, scr_IDamagable
{
    [SerializeField] [Header("水平靈敏度")] float mouseSensitivity_X;
    [SerializeField] [Header("垂直靈敏度")] float mouseSensitivity_Y;
    [SerializeField] [Header("跑步速度")] float sprintSpeed;
    [SerializeField] [Header("走路速度")] float walkSpeed;
    [SerializeField] [Header("跳躍力道")] float jumpForce;
    [SerializeField] [Header("移動滑順時間")] float moveSmoothTime;

    [SerializeField] [Header("攝影機座標")] GameObject cameraHolder;
    [SerializeField] [Header("角色UI")] GameObject ui;
    [SerializeField] [Header("武器列表")] scr_Item[] items;
    [SerializeField] [Header("血條")] Image healthBarImage;

    int itemIndex;
    int previousItemIndex = -1;
    float verticalLookRotation;  // 上下視角旋轉值
    float currentHp = maxHp;
    bool isGrounded;  // 是否在地板

    Vector3 moveSmoothVelocity;
    Vector3 moveAmount;
    scr_PlayerManager playerManager;
    Rigidbody rig;
    PhotonView pv;

    private const float maxHp = 100;

    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<scr_PlayerManager>();
    }

    void Start()
    {
        if (pv.IsMine)
        {
            EquipItem(0);
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rig);
            Destroy(ui);
        }
    }

    void Update()
    {
        if (!pv.IsMine) return;

        CalculateView();
        CalculateMove();
        CalculateJump();

        ChangeItem();
        ChangeItem_Wheel();

        FallOutMap();
        UseItem();
    }

    void FixedUpdate()
    {
        if (!pv.IsMine) return;

        rig.MovePosition(rig.position + transform.TransformDirection(moveAmount) * Time.deltaTime);
    }

    /// <summary>
    /// 玩家道具更新
    /// </summary>
    /// <param name="targetPlayer">目標玩家</param>
    /// <param name="changedProps">更換的道具</param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!pv.IsMine && targetPlayer == pv.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }
    }

    /// <summary>
    /// 設定著地狀態
    /// </summary>
    /// <param name="_isGrounded">是否著地</param>
    public void setGroundedState(bool _isGrounded)
    {
        isGrounded = _isGrounded;
    }

    /// <summary>
    /// 受傷 :  Run on the shooter's computer
    /// </summary>
    /// <param name="damage">傷害值</param>
    public void TakeDamage(float damage)
    {
        pv.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    /// <summary>
    /// 受傷 : Will run on everyone's computer but !PV.ismine make it only run Victim's computer 
    /// </summary>
    /// <param name="damage">傷害</param>
    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!pv.IsMine) return;

        currentHp -= damage;

        healthBarImage.fillAmount = currentHp / maxHp;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 視角移動
    /// </summary>
    void CalculateView()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity_X);  // 角色直接左右旋轉 (Y軸)

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity_Y;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 90);

        cameraHolder.transform.localEulerAngles = Vector3.forward * verticalLookRotation;    // 攝影機角度轉換 (X軸)
    }

    /// <summary>
    /// 角色移動
    /// </summary>
    void CalculateMove()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Vertical"), 0, -Input.GetAxisRaw("Horizontal")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * ((Input.GetKey(KeyCode.LeftShift) & Input.GetKey(KeyCode.W)) ? sprintSpeed : walkSpeed), ref moveSmoothVelocity, moveSmoothTime);  // 同時按著 W + shift 才能跑步
    }

    /// <summary>
    /// 角色跳躍
    /// </summary>
    void CalculateJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rig.AddForce(transform.up * jumpForce);
        }
    }

    /// <summary>
    /// 裝上裝備
    /// </summary>
    /// <param name="_index">裝備編號</param>
    void EquipItem(int _index)
    {
        if (_index == previousItemIndex) return;

        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if (pv.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    /// <summary>
    /// 切換武器
    /// </summary>
    void ChangeItem()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }
    }

    /// <summary>
    /// 切換武器 - 滑鼠滾輪
    /// </summary>
    void ChangeItem_Wheel()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (itemIndex >= items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(itemIndex + 1);
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (itemIndex <= 0)
            {
                EquipItem(items.Length - 1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }
        }

    }

    /// <summary>
    /// 使用武器
    /// </summary>
    void UseItem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    void Die()
    {
        playerManager.Die();
    }

    /// <summary>
    /// 掉出地圖死亡
    /// </summary>
    void FallOutMap()
    {
        if (transform.position.y <= -10)
        {
            Die();
        }
    }
}
