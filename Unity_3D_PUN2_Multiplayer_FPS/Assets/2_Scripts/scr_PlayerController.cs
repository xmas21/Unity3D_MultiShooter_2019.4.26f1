using UnityEngine;
using Photon.Pun;

public class scr_PlayerController : MonoBehaviour
{
    [Header("攝影機座標")]
    public GameObject cameraHolder;

    [Header("水平靈敏度")]
    [SerializeField]
    private float mouseSensitivity_X;
    [Header("垂直靈敏度")]
    [SerializeField]
    private float mouseSensitivity_Y;
    [Header("跑步速度")]
    [SerializeField]
    private float sprintSpeed;
    [Header("走路速度")]
    [SerializeField]
    private float walkSpeed;
    [Header("跳躍力道")]
    [SerializeField]
    private float jumpForce;
    [Header("移動滑順時間")]
    [SerializeField]
    private float moveSmoothTime;

    private float verticalLookRotation;  // 上下視角旋轉值

    private bool isGrounded;  // 是否在地板

    private Vector3 moveSmoothVelocity;
    private Vector3 moveAmount;

    private Rigidbody rig;
    private PhotonView pv;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!pv.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rig);
        }
    }

    private void Update()
    {
        if (!pv.IsMine) return;

        CalculateView();
        CalculateMove();
        CalculateJump();
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) return;

        rig.MovePosition(rig.position + transform.TransformDirection(moveAmount) * Time.deltaTime);
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
    /// 視角移動
    /// </summary>
    private void CalculateView()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity_X);  // 角色直接左右旋轉 (Y軸)

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity_Y;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 90);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;    // 攝影機角度轉換 (X軸)
    }

    /// <summary>
    /// 角色移動
    /// </summary>
    private void CalculateMove()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * ((Input.GetKey(KeyCode.LeftShift) & Input.GetKey(KeyCode.W)) ? sprintSpeed : walkSpeed), ref moveSmoothVelocity, moveSmoothTime);  // 同時按著 W + shift 才能跑步
    }

    /// <summary>
    /// 角色跳躍
    /// </summary>
    private void CalculateJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rig.AddForce(transform.up * jumpForce);
        }
    }

}
