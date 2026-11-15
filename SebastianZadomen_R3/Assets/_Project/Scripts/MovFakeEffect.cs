using UnityEngine;

public class MovFakeEffect : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    void Start()
    {

    }

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
}
