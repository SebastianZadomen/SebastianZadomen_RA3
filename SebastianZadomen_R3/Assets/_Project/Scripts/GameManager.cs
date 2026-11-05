using UnityEngine;
using UnityEngine.SceneManagement;

public  class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject PanelGameOver;
    public static GameManager instace;

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
    void Start()
    {
        PanelGameOver.SetActive(false);    
    }
    void Update()
    {
        
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
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
