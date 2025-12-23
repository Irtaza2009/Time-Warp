using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtons : MonoBehaviour
{
    public AudioClip clickClip;
    public void LoadLevel1()
    {
        AudioManager.Instance.PlaySFX(clickClip, 0.7f);
        SceneManager.LoadScene("Level-1");
    }

    public void LoadCredits()
    {
        AudioManager.Instance.PlaySFX(clickClip, 0.7f);
        SceneManager.LoadScene("Credits");
    }

    public void LoadStartMenu()
    {
        AudioManager.Instance.PlaySFX(clickClip, 0.7f);
        SceneManager.LoadScene("Start");
    }
}
