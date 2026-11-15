using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip deadSound;
    [SerializeField] private AudioClip ambientSound;
    [SerializeField] private AudioClip menuSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip bossDetected;
    [SerializeField] private AudioClip FireSound;
    [SerializeField] private AudioClip WinSound;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SceneMenu")
        {
            PlayMenuSound();
        }
        else
        {
            if (audioSource.clip != ambientSound || !audioSource.isPlaying)
            {
                PlayAmbientSound();
            }
        }
    }

    public void PlayDeadSound()
    {
        audioSource.loop = false;
        audioSource.clip = deadSound;
        audioSource.Play();
    }

    public void PlayMenuSound()
    {
        if (audioSource.clip == menuSound && audioSource.isPlaying) return;
        audioSource.clip = menuSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayAmbientSound()
    {
        if (audioSource.clip == ambientSound && audioSource.isPlaying) return;
        audioSource.clip = ambientSound;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void PlayDashSound()
    {        
        audioSource.PlayOneShot(dashSound);
    }
    public void PlayBossDetected()
    {
        audioSource.PlayOneShot(bossDetected);
    }
    public void PlayFireSound() {   
            audioSource.PlayOneShot(FireSound);
    }
    public void PlayWinGame()
    {
        audioSource.clip = WinSound;
        audioSource.loop = true;
        audioSource.Play();
       
    }

}