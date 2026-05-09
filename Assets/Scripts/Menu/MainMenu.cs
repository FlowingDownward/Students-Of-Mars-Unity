using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject instructionsPanel;


    public void PlayPressed()
    {
        ShowInstructions();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ShowInstructions()
    {
        titlePanel.SetActive(false);
        instructionsPanel.SetActive(true);
    }

    
}