using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMove playerMove;
    Vector2 startPos;
    Rigidbody2D rb;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Thorn")) Die();
        else if (collision.gameObject.CompareTag("Exit")) Exit();
    }

    private void Die()
    {
        transform.position = startPos;
        rb.linearVelocity = Vector2.zero;
    }

    private void Exit()
    {
        print("Exit");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
