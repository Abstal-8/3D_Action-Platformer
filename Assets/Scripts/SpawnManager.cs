using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    public Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 2, 3);
    }


    void SpawnEnemy()
    {
        Vector3 spawnPos = spawnPoint.position;
        int _randVal = Random.Range(0, 3);
        Instantiate(enemies[_randVal], spawnPos, Quaternion.identity);
    }

    
}
