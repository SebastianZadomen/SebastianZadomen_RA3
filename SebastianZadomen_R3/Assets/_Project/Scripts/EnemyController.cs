using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject EnemyBody;
    [SerializeField] private GameObject EnemyHead;
    [SerializeField] private GameObject pointFire;
    private SpriteRenderer spHead;
    private SpriteRenderer spBody;
    private Transform player;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float visionRange = 6f;
    [SerializeField] private float timeBetweenShots = 0.9f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float bulletLifetime = 3f;
    private float shotTimer = 0f;
    private bool isActiveBullet;
    private GameObject bulletClone;
    private Rigidbody2D bulletRB;
    private Animator bulletShotA;

    void Start()
    {
        spHead = EnemyHead.GetComponent<SpriteRenderer>();
        spBody = EnemyBody.GetComponent<SpriteRenderer>();
        bulletShotA = bulletPrefab.GetComponent<Animator>();
        isActiveBullet = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (isActiveBullet || bulletClone != null)
        {
            Vector2 direction = (target.position - pointFire.transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(EnemyHead.transform.position, direction, visionRange);
            Debug.DrawRay(EnemyHead.transform.position, direction * visionRange, Color.yellow);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                EnemyHead.transform.eulerAngles = new Vector3(0, 0, angle < 85 ? angle + 340 : angle - 160);

                spBody.flipX = angle < 85 ? true : false;
                spHead.flipX = angle < 85 ? true : false;

                if (bulletClone != null)
                {
                    bulletClone = Instantiate(bulletPrefab, pointFire.transform.position, Quaternion.identity);
                    bulletRB = bulletClone.GetComponent<Rigidbody2D>();
                }



                shotTimer += Time.deltaTime;
                if (shotTimer >= timeBetweenShots)
                {
                    Debug.Log("lanzando");
                    Shoot(direction);

                    shotTimer = 0f;
                }
            }
            else
            {
                shotTimer = 0f;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isActiveBullet = true;
        if (bulletClone == null)
        {
            bulletClone = Instantiate(bulletPrefab, pointFire.transform.position, Quaternion.identity);
            bulletRB = bulletClone.GetComponent<Rigidbody2D>();
        }
        else
        {
            return;
        }

    }
    void Shoot(Vector2 direction)
    {
        bulletShotA.SetTrigger("ShootTrigger");
        bulletRB.linearVelocity = direction * bulletSpeed;
        Destroy(bulletClone, bulletLifetime);
        isActiveBullet = false;
    }

}
