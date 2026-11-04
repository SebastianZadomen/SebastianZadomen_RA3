using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private float DeadZoneTime;
    private bool PlayerContact;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerContact = true;
        }
    }
    private void Update()
    {
        if (PlayerContact)
        {
            DeadZoneTime += Time.deltaTime;
        }
        if (DeadZoneTime >= 2)
        {
            GameManager.instace.ControlPanelDead(true);
            Time.timeScale = 0;
        }
    }

}
