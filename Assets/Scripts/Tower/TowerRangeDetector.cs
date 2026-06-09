using UnityEngine;

public class TowerRangeDetector : MonoBehaviour
{
    private Tower tower;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision proof");
        
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                tower.AddEnemy(enemy);
                //Debug.Log("Enemy has entered tower range");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                tower.RemoveEnemy(enemy);
                //Debug.Log("Enemy has left tower range");

            }
        }
    }
}