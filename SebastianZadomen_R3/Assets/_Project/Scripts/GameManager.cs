using UnityEngine;

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

    // Update is called once per frame
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
    
}
