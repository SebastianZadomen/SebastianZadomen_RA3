using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject PanelGameOver;
    [SerializeField] private GameObject PanelWin;
    [SerializeField] private GameObject PanelOptions;

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
        GameEvents.OnPlayerWin += ControlPanelWin;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerIsDead -= ActivateDeathTimer;
        GameEvents.OnPlayerWin -= ControlPanelWin;

    }

    void Start()
    {
        PanelGameOver.SetActive(false);
        PanelWin.SetActive(false);
        PanelOptions.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            Time.timeScale = 0f;
            PanelOptions.SetActive(true);

        }
    }

    public void ControlPanelDead(bool flag)
    {
        if (flag)
        {
            PanelGameOver.SetActive(true);
        }
    }
    public void ControlPanelWin(bool flag)
    {
        if (flag)
        {
            PanelWin.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void BtnRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BtnReturnMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SceneMenu");
    }
    public void BtnResum()
    {
        Time.timeScale = 1f;
        PanelOptions.SetActive(false);
    }
}