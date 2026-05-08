using UnityEngine;
using UnityEditor;

public class Path : MonoBehaviour
{
    public GameObject[] Waypoints;

    private EdgeCollider2D edgeCollider;
    private LineRenderer lineRenderer;

    public int WaypointCount => Waypoints.Length;

    private void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();

        GenerateColliderFromWaypoints();
        GenerateLineFromWaypoints();
    }
    
    public Vector3 GetPosition(int index)
    {
        return Waypoints[index].transform.position;
    }
    

    private void OnDrawGizmos()
    {
        if (Waypoints == null || Waypoints.Length <= 0)
            return;
            
        for (int i = 0; i < Waypoints.Length; i++)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleCenter;
            Handles.Label(Waypoints[i].transform.position + Vector3.up * 0.7f, Waypoints[i].name, style);

            if (i < Waypoints.Length - 1)
            {  
                Vector3 start = Waypoints[i].transform.position;
                Vector3 end = Waypoints[i + 1].transform.position;
                
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(start, end);
            }                
        }
    }

    public void GenerateColliderFromWaypoints()
    {
        if (Waypoints == null || Waypoints.Length < 2)
            return;

        Vector2[] points = new Vector2[Waypoints.Length];

        for (int i = 0; i < Waypoints.Length; i++)
        {
            // convert world to local space
            points[i] = transform.InverseTransformPoint(
                Waypoints[i].transform.position
            );
        }

        edgeCollider.points = points;
    }

    public void GenerateLineFromWaypoints()
    {
        if (Waypoints == null || Waypoints.Length < 2)
            return;

        lineRenderer.positionCount = Waypoints.Length;

        for (int i = 0; i < Waypoints.Length; i++)
        {
            lineRenderer.SetPosition(i, Waypoints[i].transform.position);
        }
    }
    

    private float DistancePointToSegment(Vector3 p, Vector3 a, Vector3 b)
    {
        Vector3 ab = b - a;
        Vector3 ap = p - a;

        float t = Vector3.Dot(ap, ab) / Vector3.Dot(ab, ab);
        t = Mathf.Clamp01(t);

        Vector3 closest = a + t * ab;

        return Vector3.Distance(p, closest);
    }

    private void OnValidate()
    {
        if (edgeCollider == null)
        {
            edgeCollider = GetComponent<EdgeCollider2D>();
        }
        GenerateColliderFromWaypoints();
    }

}
