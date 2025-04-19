using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float slideSpeed;
    private Rigidbody2D rb;
    private bool isClicking = false;
    private bool isBanned = false;
    private Vector2 lastFramePos;
    private Vector2 slideVecSum;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isBanned) return;

        if (isClicking)
        {
            if (Input.GetMouseButtonUp(0)) isClicking = false;

            Vector2 deltaPos = (Vector2)Input.mousePosition - lastFramePos;
            slideVecSum += deltaPos;

            float angle = Mathf.Abs(Vector2.SignedAngle(slideVecSum, deltaPos));
            if (angle > 45) isClicking = false;

            rb.AddForce(deltaPos * slideSpeed);
            lastFramePos = (Vector2)Input.mousePosition;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            isClicking = true;
            slideVecSum = Vector2.zero;
            lastFramePos = Input.mousePosition;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ban")) EnterBanArea();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ban")) ExitBanArea();
    }

    public void EnterBanArea()
    {
        isClicking = false;
        isBanned = true;
    }

    public void ExitBanArea()
    {
        isBanned = false;
    }

}
