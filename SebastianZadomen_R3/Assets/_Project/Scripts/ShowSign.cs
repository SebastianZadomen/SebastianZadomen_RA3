using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class ShowSign : MonoBehaviour
{
    [SerializeField] private GameObject SignCanva;
    [SerializeField] private SceneEnum Scene;
    private bool flagCollider;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && flagCollider)
        {
            ChangeScene();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SignCanva.SetActive(true);
            flagCollider = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        SignCanva.SetActive(false);
        flagCollider = false;
    }

    void Start()
    {
        SignCanva.SetActive(false);
        flagCollider = false;

    }
    void ChangeScene()
    {
        SceneManager.LoadScene(Scene.ToString());
    }
    private enum SceneEnum
    {
        SceneCave,
        SceneForest
    }
  
}
