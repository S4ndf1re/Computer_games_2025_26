using UnityEngine;

public class EnemyIdle : EnemyMove
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Move(GameObject enemy, GameObject target)
    {
        int direction = Random.Range(0, 2);
        if (direction == 0)
        {
            StartCoroutine(WalkCoroutine(enemy, DetermineWalkPoint(enemy, new Vector3(enemy.transform.position.x - walkDistance, enemy.transform.position.y, enemy.transform.position.z))));
        }
        else if (direction == 1)
        {
            StartCoroutine(WalkCoroutine(enemy, DetermineWalkPoint(enemy, new Vector3(enemy.transform.position.x + walkDistance, enemy.transform.position.y, enemy.transform.position.z))));
        }
    }
}
