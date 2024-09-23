using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        SceneManager.LoadScene(1); 
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
