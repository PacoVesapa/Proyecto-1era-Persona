using UnityEngine;
using UnityEngine.SceneManagement;

public class ManejarEscena : MonoBehaviour
{
    public void CambiarEscena()
    {
        SceneManager.LoadScene("Nivel");
        Time.timeScale = 1.0f;
    }
    public void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }
}
