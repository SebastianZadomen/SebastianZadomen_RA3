using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject EnemyBody;
    [SerializeField] private GameObject EnemyHead;
    [SerializeField] private Transform pointFire;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float visionRange = 12f; 
    [SerializeField] private float timeBetweenShots = 1.2f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float bulletLifetime = 3f;
    [SerializeField] private LayerMask detectionLayer;


    private SpriteRenderer spHead;
    private SpriteRenderer spBody;
    private float shotTimer = 0f;
    private GameObject loadedBullet;

    void Start()
    {
        spHead = EnemyHead.GetComponent<SpriteRenderer>();
        spBody = EnemyBody.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (target == null) return;

        float distanceToPlayer = Vector2.Distance(pointFire.position, target.position);

        if (distanceToPlayer <= visionRange)
        {
            Vector2 direction = (target.position - pointFire.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(pointFire.position, direction, visionRange, detectionLayer);
            Debug.DrawRay(pointFire.position, direction * visionRange, Color.yellow);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                EnemyHead.transform.eulerAngles = new Vector3(0, 0, angle < 85 ? angle + 340 : angle + 160);
                spBody.flipX = angle < 85;
                spHead.flipX = angle < 85;

                if (loadedBullet == null)
                {
                    PrepareBullet();
                }

                shotTimer += Time.deltaTime;
                if (shotTimer >= timeBetweenShots)
                {
                    Shoot(direction);
                    shotTimer = 0f;
                }
                return;
            }
        }

        ResetAttack();
    }

    void PrepareBullet()
    {
        loadedBullet = Instantiate(bulletPrefab, pointFire.position, Quaternion.identity, pointFire);

        Collider2D bulletCol = loadedBullet.GetComponent<Collider2D>();
        if (bulletCol != null) bulletCol.enabled = false;
    }

    void Shoot(Vector2 direction)
    {
        if (loadedBullet == null) return;

        loadedBullet.transform.SetParent(null);

        Collider2D bulletCol = loadedBullet.GetComponent<Collider2D>();
        if (bulletCol != null) bulletCol.enabled = true;

        Animator bulletAnim = loadedBullet.GetComponent<Animator>();

        Rigidbody2D bulletRB = loadedBullet.GetComponent<Rigidbody2D>();
        if (bulletRB != null) bulletRB.linearVelocity = direction * bulletSpeed;

        Destroy(loadedBullet, bulletLifetime);
        loadedBullet = null;

        PrepareBullet();
    }

    void ResetAttack()
    {
        shotTimer = 0f;
        if (loadedBullet != null)
        {
            Destroy(loadedBullet);
            loadedBullet = null;
        }
    }
}