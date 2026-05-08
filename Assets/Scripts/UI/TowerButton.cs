using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{   
    [SerializeField] private TowerData towerData;
    
    private Button button;
    
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        GameManager.OnCreditsChanged += UpdateButton;
    }

    private void OnDisable()
    {
        GameManager.OnCreditsChanged -= UpdateButton;
    }

    private void Start()
    {
        UpdateButton(GameManager.Instance.PlayerCredits);
    }

    private void UpdateButton(int credits)
    {
        button.interactable = credits >= towerData.price;
    }
    
    public void SelectTower()
    {
        if (!button.interactable)
        {
            return;
        }

        Debug.Log("Button clicked: " + towerData.name);
        TowerPlacementManager.Instance.BeginPlacement(towerData);
    }
}
