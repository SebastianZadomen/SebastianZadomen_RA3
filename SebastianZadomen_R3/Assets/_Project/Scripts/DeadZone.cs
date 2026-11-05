using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private float deadZoneTime = 0f;
    private bool playerContact = false;
    private bool eventTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerContact)
        {
            playerContact = true;

            GameEvents.PlayerIsDead(true);
        }
    }

    private void Update()
    {
        if (playerContact && !eventTriggered)
        {
            deadZoneTime += Time.deltaTime;

            if (deadZoneTime >= 1.8f)
            {
                eventTriggered = true;
                GameManager.instace.ControlPanelDead(true);
                
                Time.timeScale = 0f;
            }
        }
    }
}
