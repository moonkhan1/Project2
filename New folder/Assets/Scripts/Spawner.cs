using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject _spawnerPrefab;

    [SerializeField] float _spawnerTime = 3.5f;
    [SerializeField] int _maxCount = 8;
    [SerializeField] int count = 0;
    Vector3 enemySpawn;

    public PlayerController _player;
    [SerializeField] Transform spawn;
    void Start()
    {
         StartCoroutine(spawnEnemy(_spawnerTime, _spawnerPrefab));
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        if (_maxCount > count)
        {
    
            yield return new WaitForSeconds(interval);
            GameObject newEnemy = Instantiate(enemy,(spawn.transform.localPosition +  new Vector3(Random.Range(-5f,10),0, 0)),Quaternion.Euler(0,94,0));
            // newEnemy.transform.position = GetRandomPoint(GetComponent<Collider>());
            StartCoroutine(spawnEnemy(interval, enemy));
            count++;
        }
    }

    // Vector3 GetRandomPoint (Collider collider)
    // {
    //     Vector3 point = new Vector3(
    //     Random.Range(collider.bounds.min.x, collider.bounds.max.x),
    //     Random.Range(collider.bounds.min.y, collider.bounds.max.y),
    //     Random.Range(collider.bounds.min.z, collider.bounds.max.z)
    //     );

    //     if (point != collider.ClosestPoint(point))
    //     {
    //         point = GetRandomPoint(collider);
    //     }
    //     point.y = 1;
    //     return point;
    // }
}
