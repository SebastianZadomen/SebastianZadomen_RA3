using UnityEngine;

public class Fireball : MonoBehaviour
{
    private bool hasHit = false;
    private Animator AnimationFire;
    private Rigidbody2D rb;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        AnimationFire = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        if (hasHit) return;

        if (collision.CompareTag("Player"))
        {
            hasHit = true;
            rb.linearVelocity = new Vector2(0, 0);

            GameEvents.PlayerIsDead(true);
            AnimationFire.SetTrigger("ExplodeTrigger");
            
            Destroy(gameObject, 0.5f);

        }
        else if (collision.CompareTag("Ground")) 
        {
            rb.linearVelocity = new Vector2(0, 0);

            hasHit = true;
            AnimationFire.SetTrigger("ExplodeTrigger");
            Destroy(gameObject, 0.5f);
        }
    }
}

