using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class ShowSign : MonoBehaviour
{
    [SerializeField] private GameObject SignCanva;
    [SerializeField] private SceneEnum Scene;
    private bool flagCollider;
    public bool OnlyShow;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && flagCollider && !OnlyShow)
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
            if (OnlyShow)
            {
                AudioManager.instance.PlayBossDetected();
            }
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
        OnlyShow = false;

    }
    void ChangeScene()
    {
        SceneManager.LoadScene(Scene.ToString());
    }
    private enum SceneEnum
    {
        SceneCave,
        OutdoorScene,
        FinalScene
    }
  
}
