using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private GameObject creditsPanel;

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

    public void ShowCredits()
    {
        titlePanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void ShowTitleFromCredits()
    {
        creditsPanel.SetActive(false);
        titlePanel.SetActive(true);
    }
    
}