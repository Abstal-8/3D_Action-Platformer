using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{

    public GameObject player;
    public static bool _isEnemiesKilled;
    public static bool _isPartsCollected;
    public static bool _isWaypointReached;

    public static Action onMissionStart;
    public static Action onMissionComplete;

    private void OnEnable() {
        PartsCollection.onPartsCollected += PartsCollection.CollectionMission;
    }

    private void OnDisable() {
        PartsCollection.onPartsCollected -= PartsCollection.CollectionMission;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            onMissionStart?.Invoke();
        }
    }


    

}
