using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalSign : MonoBehaviour
{
    [SerializeField] private GameObject SignCanva;
    [SerializeField] private GameObject Boss;
    private bool flagCollider;
    private float timerEnd;
    private bool ActiveTimer;
    [SerializeField]private float timeMax;
    private bool winTriggered;
    private Animator animBoss;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && flagCollider)
        {
            DeadBoss();
            ActiveTimer = true;
            timerEnd += Time.deltaTime;
        }
        if (ActiveTimer && !winTriggered)
        {
            timerEnd += Time.deltaTime;

            if (timerEnd >= timeMax)
            {
                winTriggered = true;
                GameEvents.PlayerWin(true);
                AudioManager.instance.PlayWinGame();
                SignCanva.SetActive(false);
            }
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
        timerEnd = 0;
        ActiveTimer = false;
        winTriggered = false;
       
    }
    void DeadBoss()
    {
        animBoss = Boss.GetComponent<Animator>();
        animBoss.SetTrigger("isDead");


        Destroy(Boss, timeMax-0.5f);
        
    }
  
}
