using UnityEngine;

public class TowerSelectable : MonoBehaviour
{
    private Tower tower;

    private void Awake()
    {
        tower = GetComponent<Tower>();
    }

    private void OnMouseDown()
    {
        //TowerSelectionManager.Instance.SelectTower(tower);
    }
}