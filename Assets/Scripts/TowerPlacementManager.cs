using UnityEngine;
using UnityEngine.InputSystem;

public class TowerPlacementManager : MonoBehaviour
{
    public static TowerPlacementManager Instance;

    private TowerData selectedTower;
    private GameObject previewObject;

    private Camera mainCamera;

    [SerializeField] private LayerMask blockedLayer;
    [SerializeField] private float checkRadius = 0.5f;

    [SerializeField] private Path path;
    [SerializeField] private float minDistanceFromPath = 1.5f;

    private TowerPreview previewVisual;
    private bool canPlace;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (previewObject == null)
            return;

        FollowMouse();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlaceTower();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelPlacement();
        }
    }

    public void BeginPlacement(TowerData towerData)
    {
        selectedTower = towerData;
        previewObject = Instantiate(towerData.prefab);
        previewVisual = previewObject.GetComponent<TowerPreview>();
        SetPreviewMode(previewObject, true);
    }

    private void FollowMouse()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        previewObject.transform.position = worldPos;

        ValidatePlacement(worldPos);
    }

    private void ValidatePlacement(Vector3 position)
    {
        bool blockedByObjects = Physics2D.OverlapCircle(position, checkRadius, blockedLayer) != null;

        bool tooCloseToPath = path.IsPointTooClose(position, minDistanceFromPath);

        canPlace = !blockedByObjects && !tooCloseToPath;

        if (previewVisual != null)
        {
            previewVisual.SetValid(canPlace);
        }
    }

    private void PlaceTower()
    {
        if (!canPlace)
        {
            return;
        }

        Instantiate(selectedTower.prefab, previewObject.transform.position, Quaternion.identity);

        Destroy(previewObject);

        previewObject = null;
        selectedTower = null;
    }

    private void CancelPlacement()
    {
        Destroy(previewObject);

        previewObject = null;
        selectedTower = null;
    }

    private void SetPreviewMode(GameObject tower, bool previewMode)
    {
        Collider2D col = tower.GetComponent<Collider2D>();

        if (col != null)
            col.enabled = !previewMode;
    }
}
