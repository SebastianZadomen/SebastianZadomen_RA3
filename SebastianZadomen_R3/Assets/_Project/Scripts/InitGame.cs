using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{

    public void Init()
    {
        SceneManager.LoadScene("OpeningScene");
    }
}
