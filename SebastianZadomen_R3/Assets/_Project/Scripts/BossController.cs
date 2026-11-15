using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float visionRange = 12f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private LayerMask obstacleLayer;
    private PolygonCollider2D polygonCollider;
    private Rigidbody2D rb;
    public bool isFrozen = false;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        polygonCollider = GetComponent<PolygonCollider2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (isFrozen)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, obstacleLayer);
        Debug.DrawRay(transform.position, direction * visionRange, Color.yellow);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            rb.linearVelocity = direction * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && polygonCollider.IsTouching(collision))
        {
            rb.linearVelocity = Vector2.zero;
            GameEvents.PlayerIsDead(true);
        }
    }
}
