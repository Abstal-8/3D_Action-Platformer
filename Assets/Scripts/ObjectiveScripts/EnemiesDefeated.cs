using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDefeated : MonoBehaviour
{
    public static int enemiesKilled;
    public static int maxEnemies = 15;

    public static Action OnEnemyKilled;

    [SerializeField] private static WaypointCreation _waypointCreation;
    public WaypointCreation wayPointOBJ;

    private void Start() {
        _waypointCreation = wayPointOBJ.GetComponent<WaypointCreation>();
    }

    public static void KillCount()
    {
        enemiesKilled++;
        Debug.Log("ENEMY SLAIN!");
        if (enemiesKilled >= maxEnemies)
        {
            enemiesKilled = maxEnemies;
            ObjectiveManager._isEnemiesKilled = true;
            ObjectiveManager.onMissionComplete += Completed;
            ObjectiveManager.onMissionComplete?.Invoke();
            ObjectiveManager.onMissionComplete = null;
            Debug.Log("COMPLETED!");
        }
    }

    private static void Completed()
    {
        if (ObjectiveManager._isEnemiesKilled)
        {
            ObjectiveManager.onMissionStart += _waypointCreation.SpawnMissionEnd;
            ObjectiveManager.onMissionStart?.Invoke();
            ObjectiveManager.onMissionStart = null;
        }
    }

}
