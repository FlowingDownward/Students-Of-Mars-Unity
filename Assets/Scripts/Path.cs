 using UnityEngine;
using UnityEditor;

public class Path : MonoBehaviour
{
    public GameObject[] Waypoints;

    public int WaypointCount => Waypoints.Length;

    public float minBuildDistance = 1.5f;

    public Vector3 GetPosition(int index)
    {
        return Waypoints[index].transform.position;
    }
    

    private void OnDrawGizmos()
    {
        if (Waypoints.Length > 0)
        {
            for (int i = 0; i < Waypoints.Length; i++)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.white;
                style.alignment = TextAnchor.MiddleCenter;
                Handles.Label(Waypoints[i].transform.position + Vector3.up * 0.7f, Waypoints[i].name, style);


                if (i < Waypoints.Length - 1)
                {  
                    Gizmos.color = Color.gray;
                    Gizmos.DrawLine(Waypoints[i].transform.position, Waypoints[i+1].transform.position);
                }
                
            }
        }
    }

    public bool IsPointTooClose(Vector3 point, float minDistance)
    {
        for (int i = 0; i < Waypoints.Length - 1; i++)
        {
            Vector3 a = Waypoints[i].transform.position;
            Vector3 b = Waypoints[i + 1].transform.position;

            float dist = DistancePointToSegment(point, a, b);

            if (dist < minDistance)
            {
                return true;
            }
        }

        return false;
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
}
