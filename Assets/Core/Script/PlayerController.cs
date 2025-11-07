using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 6f; // 常にダッシュ
    public float mouseSensitivity = 2f;
    public Transform cameraHolder; // カメラを入れたオブジェクト

    [Header("カメラ揺れ")]
    public float shakeAmount = 0.05f;
    public float shakeSpeed = 10f;
    private float shakeTimer = 0f;

    [HideInInspector] public Vector3 currentVelocity;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouse();
        HandleMoveInput();
    }

    void FixedUpdate()
    {
        Vector3 move = transform.forward * currentVelocity.z + transform.right * currentVelocity.x;
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
        DoCameraShake();
    }

    void HandleMouse()
    {
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mx);

        if (cameraHolder)
        {
            Vector3 local = cameraHolder.localEulerAngles;
            float currentX = local.x;

            // convert to -180..180
            if (currentX > 180) currentX -= 360;

            currentX -= my;
            currentX = Mathf.Clamp(currentX, -80f, 80f);
            cameraHolder.localEulerAngles = new Vector3(currentX, 0, 0);
        }
    }

    void HandleMoveInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;
        currentVelocity = dir * moveSpeed; // 外部に速度を公開（Enemyの予測に使う）

        // カメラ揺れの強さは速度で決める
        if (dir.magnitude > 0.1f)
        {
            shakeTimer += Time.deltaTime * shakeSpeed;
        }
        else
        {
            shakeTimer = 0f;
        }
    }

    void DoCameraShake()
    {
        if (cameraHolder == null) return;
        if (shakeTimer <= 0f) return;

        float s = Mathf.Sin(shakeTimer) * shakeAmount;
        cameraHolder.localPosition = new Vector3(0, s, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 敵のヘッドTransform を探して渡す
            Transform head = other.transform.Find("Head");
            if (head == null) head = other.transform; // 無ければ敵本体

            GameManager.I.OnPlayerCaught(head);
        }
    }
}
