using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text creditsText;

    //Unit Panel UI
    public GameObject unitPanel;
    [SerializeField] private TMP_Text towerText;
    [SerializeField] private TMP_Text killcountText;

    private Tower SelectedTower = null;

    private void Awake()
    {
        unitPanel.SetActive(false);

    }

    private void OnEnable()
    {
        Spawner.OnWaveChanged += UpdateWaveText;
        GameManager.OnLivesChanged += UpdateLivesText;
        GameManager.OnCreditsChanged += UpdateCreditsText;
        TowerSelectionManager.OnNewTowerSelected += UpdateUnitPanel;
        Tower.OnKillCountChanged += UpdateKillText;
    }

    private void OnDisable()
    {
        Spawner.OnWaveChanged -= UpdateWaveText;
        GameManager.OnLivesChanged -= UpdateLivesText;
        GameManager.OnCreditsChanged -= UpdateCreditsText;
        TowerSelectionManager.OnNewTowerSelected -= UpdateUnitPanel;
        Tower.OnKillCountChanged -= UpdateKillText;
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

    private void UpdateUnitPanel (Tower NewTower)
    {
        if (NewTower == null)
        {
            unitPanel.SetActive(false);
            SelectedTower = null;
        }
        else
        {
            SelectedTower = NewTower;
            unitPanel.SetActive(true);
            towerText.text = $"{NewTower.Data.towerName}";
            killcountText.text = $"Kills: {NewTower.KillCount}";
        }
    }

    //Unit Panel Stuff
    private void UpdateKillText (int newKillCount)
    {
        killcountText.text = $"Kills: {newKillCount}";
    }
}
