using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject PanelGameOver;
    public static GameManager instace;

    private float deadZoneTime = 0f;
    private bool playerIsDead = false;
    private bool eventTriggered = false;

    private void Awake()
    {
        if (instace == null)
        {
            instace = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerIsDead += ActivateDeathTimer;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerIsDead -= ActivateDeathTimer;
    }

    void Start()
    {
        PanelGameOver.SetActive(false);
    }

    private void ActivateDeathTimer(bool flag)
    {
        if (flag && !playerIsDead)
        {
            playerIsDead = true;
        }
    }

    void Update()
    {
        if (playerIsDead && !eventTriggered)
        {
            deadZoneTime += Time.deltaTime;

            if (deadZoneTime >= 1.8f)
            {
                eventTriggered = true;
                ControlPanelDead(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void ControlPanelDead(bool flag)
    {
        if (flag)
        {
            PanelGameOver.SetActive(true);
        }
    }

    public void BtnRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}