using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseMovementSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    
    [Header("Status")]
    private float currentMovementSpeed;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    // 아이템 효과 관련 변수들
    private float speedBoostMultiplier = 1f;
    private float temporarySpeedBoost = 0f;
    
    private void Awake()
    {
        // Rigidbody2D 컴포넌트 확인 및 자동 추가
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        InitializeStatus();
    }

    private void InitializeStatus()
    {
        currentMovementSpeed = baseMovementSpeed;
    }

    private void Update()
    {
        // 입력 처리만 담당
        HandleInput();
    }

    private void FixedUpdate()
    {
        // 실제 물리 이동 처리
        Move();
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // A, D 키
        float verticalInput = Input.GetAxisRaw("Vertical");     // W, S 키
        
        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;
    }

    private void Move()
    {
        Vector2 movement = moveDirection * (currentMovementSpeed * speedBoostMultiplier);
        rb.velocity = movement;
    }

    // 아이템 효과 관련 메서드들
    public void ApplySpeedBoost(float multiplier, float duration)
    {
        speedBoostMultiplier = multiplier;
        StartCoroutine(ResetSpeedBoostAfterDelay(duration));
    }

    private System.Collections.IEnumerator ResetSpeedBoostAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        speedBoostMultiplier = 1f;
    }

    // 아이템 획득 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpeedBoostItem"))
        {
            // 예시: 속도 부스트 아이템 효과
            ApplySpeedBoost(1.5f, 5f);
            Destroy(other.gameObject);
        }
    }
}
