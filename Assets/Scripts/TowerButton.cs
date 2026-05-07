using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private TowerData towerData;
   
    public void SelectTower()
    {
        Debug.Log("Enemy has entered tower range");
        TowerPlacementManager.Instance.BeginPlacement(towerData);

    }
}
