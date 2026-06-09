using UnityEngine;
using UnityEngine.InputSystem;

public class TowerSelectionManager : MonoBehaviour
{
    public static TowerSelectionManager Instance;

    [SerializeField] private GameObject rangeIndicatorPrefab;
    [SerializeField] private LayerMask towerSelectionMask;

    private Tower selectedTower;
    private GameObject activeRangeIndicator;

    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
        Debug.Log("Tower Selection Active");
    }
    

    private void Update()
    {
        HandleSelectionInput();
    }

    public void SelectTower(Tower tower)
    {
        if (selectedTower == tower)
            return;

        DeselectCurrentTower();

        selectedTower = tower;

        activeRangeIndicator =
            Instantiate(rangeIndicatorPrefab);

        activeRangeIndicator.transform.position =
            tower.transform.position;

        float diameter = tower.Data.range / 2;

        activeRangeIndicator.transform.localScale =
            new Vector3(diameter, diameter, 1f);
    }

    public void DeselectCurrentTower()
    {
        selectedTower = null;

        if (activeRangeIndicator != null)
        {
            Destroy(activeRangeIndicator);
            activeRangeIndicator = null;
        }
    }
    
    
    private void HandleSelectionInput()
    {
        
       
        //Debug.Log("Selection Input Running Part 1");
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            DeselectCurrentTower();
            return;
        }


        //Debug.Log("Selection Input Running part 2");
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            Collider2D hit = Physics2D.OverlapPoint(mousePos, towerSelectionMask);
            if (hit != null)
            {
                Debug.Log("Hit: " + hit.name);

                Tower tower = hit.GetComponentInParent<Tower>();

                if (tower != null)
                {
                    Debug.Log("Tower selected");
                    SelectTower(tower);
                    return;
                }
            }

            Debug.Log("Nothing hit");

            DeselectCurrentTower();
        }

        if (selectedTower == null)
            return;

    }
    
    
}