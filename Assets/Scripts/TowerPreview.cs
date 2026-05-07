using UnityEngine;

public class TowerPreview : MonoBehaviour
{
    private SpriteRenderer[] renderers;

    private void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void SetValid(bool isValid)
    {
        Color color = isValid ? Color.green : Color.red;
        color.a = 0.5f;

        foreach (var r in renderers)
        {
            r.color = color;
        }
    }
}
