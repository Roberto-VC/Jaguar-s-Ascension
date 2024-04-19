using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        // Carga la escena del juego cuando se presiona el botón "Play"
        // Asegúrate de cambiar "NombreDeTuEscenaDeJuego" al nombre de tu escena de juego
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        // Cierra el juego cuando se presiona el botón "Quit"
        Application.Quit();
    }
}

