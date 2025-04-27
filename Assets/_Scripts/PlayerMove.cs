using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour, Resettable
{
    [SerializeField] float slideSpeed;
    private Rigidbody2D rb;
    public bool isClicking = false;
    private bool isBanned = false;
    private Vector2 frameDeltaPos;
    private Vector2 lastFramePos;
    private Vector2 slideVecSum;
    float minDragDistance = 0.1f;
    [SerializeField] float fallMax = -25f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, fallMax)); // 아래로 향하는 속도를 -10으로 제한

        if (isBanned) return;

        // 마우스 버튼을 눌렀다면
        // 상태를 클릭 중으로 변경
        // 슬라이드 벡터를 초기화
        // 월드 좌표계 기준으로 lastFramePos를 설정
        if (Input.GetMouseButtonDown(0))
        {
            isClicking = true;

            slideVecSum = Vector2.zero;
            lastFramePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.linearVelocity = Vector2.zero;
        }
        // 마우스 버튼을 뗏다면
        // 클릭 중 상태를 취소
        else if (Input.GetMouseButtonUp(0))
        {
            isClicking = false;
        }

        // 클릭 중이 아니라면 이동 로직 호출 안 함
        if (!isClicking) return;

        Vector2 currentWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 현재 마우스의 월드 좌표계 위치
        frameDeltaPos = currentWorldPos - lastFramePos; // 이전 프레임 좌표와 현재 월드 좌표계 위치
        slideVecSum += frameDeltaPos; // 이번 프레임의 마우스 이동 변위

        if (slideVecSum.magnitude > minDragDistance) // 일정 거리 이상 이동해야 방향 전환으로 인한 슬라이드 종료 판정 시작
        {
            float angle = Mathf.Abs(Vector2.SignedAngle(slideVecSum, frameDeltaPos)); // 지금까지의 이동 변위의 합과 이번 프레임의 이동 변위 각도 비교
            if (angle > 45) isClicking = false;
        }

        lastFramePos = currentWorldPos; // 이번 프레임 위치를 저장해둠
    }

    void FixedUpdate()
    {
        if (isClicking) // 클릭 중이라면 프레임 간 마우스 이동 변위만큼 힘을 더함
        {
            frameDeltaPos /= Mathf.Pow(1.1f, frameDeltaPos.magnitude);
            rb.AddForce(frameDeltaPos * slideSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ban"))
        {
            isClicking = false;
            isBanned = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ban"))
        {
            isBanned = false;
        }
    }

    public void DoReset()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        isClicking = false;
        isBanned = false;
    }
}
