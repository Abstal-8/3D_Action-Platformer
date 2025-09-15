using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaypointCreation : MonoBehaviour
{
    public GameObject wayPoint;

    private void OnTriggerEnter(Collider other) {
        if (ObjectiveManager._isEnemiesKilled && other.gameObject.tag == "Player")
        {
            ObjectiveManager._isWaypointReached = true;
            Debug.Log("CHECKPOINT REACHED");
        }
    }

    public void SpawnMissionEnd()
    {
        Debug.Log("WAYPOINT SPAWNED");
        wayPoint.SetActive(true);
    }
}
