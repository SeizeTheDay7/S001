using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour, Resettable
{
    [SerializeField] float waitTime = 3f;
    [SerializeField] GameObject dummy;
    [SerializeField] Door door;
    Collider2D button_col;
    SpriteRenderer button_sr;
    Coroutine curCoroutine;

    void Start()
    {
        button_col = GetComponent<Collider2D>();
        button_sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        foreach (ContactPoint2D contact in col.contacts)
        {
            if (Vector2.Dot(contact.normal, transform.TransformDirection(Vector2.down)) > 0.5f && col.relativeVelocity.magnitude > 0)
            {
                PressButton();
                break;
            }
        }
    }

    private void PressButton()
    {
        button_col.enabled = false;
        button_sr.enabled = false;
        dummy.SetActive(true);

        door.Open();

        curCoroutine = StartCoroutine(ReleaseButton());
    }

    private IEnumerator ReleaseButton()
    {
        yield return new WaitForSeconds(waitTime);
        button_col.enabled = true;
        button_sr.enabled = true;
        dummy.SetActive(false);

        door.Close();
    }

    public void DoReset()
    {
        button_col.enabled = true;
        button_sr.enabled = true;
        dummy.SetActive(false);
        if (curCoroutine != null)
            StopCoroutine(curCoroutine);
    }
}
