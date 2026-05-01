using UnityEngine;
using System.Collections;
using TMPro;

public class playermovement1 : MonoBehaviour
{
    [Header("Jump Settings")]
    public float maxJumpForce = 15f;
    public float chargeSpeed = 10f;

    public Rigidbody2D rb;
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private float jumpPower;
    private bool isCharging;
    private Vector2 aimDirection;

    public float jumpCooldown = 1f;
    private bool canJump = true;

    [Header("Trajectory")]
    public TrajectoryDot trajectoryDot;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    // 👻 GHOST POWER
    [Header("Ghost Power")]
    public bool canPassThroughPlatforms = false;
    public float ghostDuration = 5f;
    private float ghostTimer;
    private bool isGhostActive = false;

    // 🟢 DOUBLE JUMP
    [Header("Double Jump")]
    public bool hasDoubleJumpPower = false;
    public float doubleJumpDuration = 10f;
    private float doubleJumpTimer;
    private bool isDoubleJumpActive = false;

    // 🟢 AIR JUMP WITH DELAY
    public float airJumpCooldown = 0.15f;
    private float airJumpTimer = 0f;

    private int jumpCount = 0;
    private int maxJumps = 1;

    // 💥 BREAKING MODE
    [Header("Breaking Mode")]
    public bool isBreakingMode = false;
    public float breakSpeed = 20f;
    public float breakDuration = 3f;
    private float breakTimer;

    [Header("Platform Spawn Safety")]
    public GameObject platformPrefab;
    [Header("Squash & Stretch")]
public Transform playerVisual; // assign your cube sprite
public float squashFactor = 0.02f;
public float maxSquash = 0.4f;
public float squashSmoothSpeed = 10f;
public Transform PlayerVisual;
private Vector3 targetScale = Vector3.one;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (isGrounded)
        {
            jumpCount = 0;
        }

        maxJumps = hasDoubleJumpPower ? 2 : 1;

        // 💥 BREAKING MODE
        if (isBreakingMode)
        {
            rb.linearVelocity = new Vector2(0f, breakSpeed);

            breakTimer -= Time.deltaTime;

            if (breakTimer <= 0)
            {
                isBreakingMode = false;
                PlayerVisual.GetComponent<SpriteRenderer>().color = Color.white;
                SpawnPlatformBelow();
            }

            return;
        }

        // 👻 GHOST TIMER
        if (isGhostActive)
        {
            ghostTimer -= Time.deltaTime;
            timerText.text = "Ghost: " + Mathf.Ceil(ghostTimer);

            if (ghostTimer <= 0)
            {
                isGhostActive = false;
                canPassThroughPlatforms = false;
                timerText.gameObject.SetActive(false);
            }
        }

        // 🟢 DOUBLE JUMP TIMER
        if (isDoubleJumpActive)
        {
            doubleJumpTimer -= Time.deltaTime;
            airJumpTimer -= Time.deltaTime;

            timerText.text = "Double Jump: " + Mathf.Ceil(doubleJumpTimer);

            if (doubleJumpTimer <= 0)
            {
                isDoubleJumpActive = false;
                timerText.gameObject.SetActive(false);
            }
        }
// Smooth squash transition
if (playerVisual != null)
{
    playerVisual.localScale = Vector3.Lerp(
        playerVisual.localScale,
        targetScale,
        squashSmoothSpeed * Time.deltaTime
    );
}
        HandleInput();
    }

    // 🔥 INPUT HANDLER (PC + MOBILE)
    void HandleInput()
    {
        // 📱 MOBILE TOUCH
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began && canJump)
            {
                if (isGrounded)
                {
                    StartCharging(touchPos);
                }
                else if (isDoubleJumpActive && airJumpTimer <= 0f)
                {
                    StartCharging(touchPos);
                    airJumpTimer = airJumpCooldown;
                }
            }

            if (touch.phase == TouchPhase.Moved && isCharging)
            {
                ChargeJump(touchPos);
            }

            if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && isCharging)
            {
                ReleaseJump();
            }
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        // 🖱️ PC MOUSE INPUT
        if (Input.GetMouseButtonDown(0) && canJump)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (isGrounded)
            {
                StartCharging(mousePos);
            }
            else if (isDoubleJumpActive && airJumpTimer <= 0f)
            {
                StartCharging(mousePos);
                airJumpTimer = airJumpCooldown;
            }
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ChargeJump(mousePos);
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            ReleaseJump();
        }
#endif
    }

    void StartCharging(Vector2 startPos)
    {
        isCharging = true;
        jumpPower = 0f;

        jumpCount++;
        rb.linearVelocity = Vector2.zero;
    }

   void ChargeJump(Vector2 currentPos)
{
    jumpPower += chargeSpeed * Time.deltaTime;
    jumpPower = Mathf.Clamp(jumpPower, 0, maxJumpForce);

    Vector2 drag = (Vector2)transform.position - currentPos;
    aimDirection = drag.normalized;

    if (aimDirection.y < 0)
        aimDirection.y = Mathf.Abs(aimDirection.y);

    trajectoryDot.ShowDots(aimDirection * jumpPower, transform.position);

    // 🧊 SQUASH LOGIC
    float dragDistance = drag.magnitude;
    float squash = Mathf.Clamp(dragDistance * squashFactor, 0, maxSquash);

    targetScale = new Vector3(
        1 + squash,   // stretch X
        1 - squash,   // compress Y
        1
    );
}

   void ReleaseJump()
{
    jumpPower = Mathf.Max(jumpPower, 5f);
    rb.linearVelocity = aimDirection * jumpPower;

    isCharging = false;
    trajectoryDot.HideDots();

    // Reset scale target
    targetScale = Vector3.one;

    StartCoroutine(JumpCooldownRoutine());
}

    IEnumerator JumpCooldownRoutine()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    // 👻 GHOST
    public void ActivatePowerup()
    {
        isGhostActive = true;
        canPassThroughPlatforms = true;

        ghostTimer = ghostDuration;
        timerText.gameObject.SetActive(true);
    }

    // 🟢 DOUBLE JUMP
    public void ActivateDoubleJump()
    {
        isDoubleJumpActive = true;
        doubleJumpTimer = doubleJumpDuration;
        airJumpTimer = 0f;

        timerText.gameObject.SetActive(true);
    }

    // 💥 BREAKING MODE
    public void ActivateBreakingMode()
    {
        isBreakingMode = true;
        breakTimer = breakDuration;

        PlayerVisual.GetComponent<SpriteRenderer>().color = Color.red;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!isBreakingMode) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Destroy(collision.gameObject);
        }
    }

    void SpawnPlatformBelow()
    {
        if (platformPrefab == null) return;

        Vector3 pos = transform.position - new Vector3(0, 3f, 0);
        Instantiate(platformPrefab, pos, Quaternion.identity);
    }
}