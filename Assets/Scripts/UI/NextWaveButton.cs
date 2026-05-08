using UnityEngine;
using UnityEngine.UI;

public class NextWaveButton : MonoBehaviour
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private Button button;


    private void Update()
    {
        button.interactable = spawner.CanStartWave();
    }

    public void StartWave()
    {
        spawner.StartNextWave();
    }

}
