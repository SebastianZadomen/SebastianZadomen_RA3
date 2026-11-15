using UnityEngine;

public class FreezeZone : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            BossController boss = collision.GetComponent<BossController>();
             boss.isFrozen = true;
            
        }
    }
}
