using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text creditsText;

    private void OnEnable()
    {
        Spawner.OnWaveChanged += UpdateWaveText;
        GameManager.OnLivesChanged += UpdateLivesText;
        GameManager.OnCreditsChanged += UpdateCreditsText;
    }

    private void OnDisable()
    {
        Spawner.OnWaveChanged -= UpdateWaveText;
        GameManager.OnLivesChanged -= UpdateLivesText;
        GameManager.OnCreditsChanged -= UpdateCreditsText;
    }

    private void UpdateWaveText (int currentWave)
    {
        waveText.text = $"Wave: {currentWave}";
    }

    private void UpdateLivesText (int currentLives)
    {
        livesText.text = $"Lives: {currentLives}";
    }

    private void UpdateCreditsText (int currentCredits)
    {
        creditsText.text = $"Credits: {currentCredits}";
    }
}
