using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, Resettable
{
    [SerializeField] GameObject deadSign;
    GameManager gameManager;
    Vector2 startPos;
    Rigidbody2D rb;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameManager.ResetAll();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Thorn")) Die();
        else if (collision.gameObject.CompareTag("Exit")) Exit();
    }

    private void Die()
    {
        Instantiate(deadSign, transform.position, Quaternion.identity);
        gameManager.ResetAll();
    }

    private void Exit()
    {
        // print("Exit");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void DoReset()
    {
        transform.position = startPos;
    }
}
