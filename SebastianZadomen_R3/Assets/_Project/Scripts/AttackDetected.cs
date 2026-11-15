using UnityEngine;

public class AttackDetected : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    private Animator animParent;
    void Start()
    {
        animParent = parent.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animParent.SetBool("AttackBoss", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animParent.SetBool("AttackBoss", false);
        }
    }
}
