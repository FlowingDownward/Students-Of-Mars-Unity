using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerPlacementManager : MonoBehaviour
{
    public static TowerPlacementManager Instance;

    private TowerData selectedTower;
    private GameObject previewObject;

    private Camera mainCamera;

    [SerializeField] private LayerMask blockedLayer;
    [SerializeField] private Path path;
    [SerializeField] private TMP_Text placementHintText;

    private TowerPreview previewVisual;
    private bool canPlace;

    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
        placementHintText.gameObject.SetActive(false);
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
        //Debug.Log("Preview");

        previewVisual = previewObject.GetComponent<TowerPreview>();
        SetPreviewMode(previewObject, true);
        placementHintText.gameObject.SetActive(true);
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
        Collider2D hit = Physics2D.OverlapCircle(position, 0.5f,blockedLayer );

        canPlace = hit == null;

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

    if (!GameManager.Instance.TrySpendCredits(selectedTower.price))
    {
        //Debug.Log("Not enough credits!");
        return;
    }
        //Debug.Log("Enough credits!");

        Instantiate(selectedTower.prefab,
        previewObject.transform.position,
        Quaternion.identity);
        Destroy(previewObject);

        previewObject = null;
        selectedTower = null;

        placementHintText.gameObject.SetActive(false);
    }

    private void CancelPlacement()
    {
        Destroy(previewObject);

        previewObject = null;
        selectedTower = null;

        placementHintText.gameObject.SetActive(false);
    }

    private void SetPreviewMode(GameObject tower, bool previewMode)
    {
        Collider2D col = tower.GetComponent<Collider2D>();

        if (col != null)
        {
            col.enabled = !previewMode;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (previewObject != null)
        {
            Gizmos.DrawWireSphere(previewObject.transform.position, 0.5f);
        }
    }
}
