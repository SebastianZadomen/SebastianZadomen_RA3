using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private bool playerContact = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerContact)
        {
            playerContact = true;
            GameEvents.PlayerIsDead(true);
        }
    }
}
