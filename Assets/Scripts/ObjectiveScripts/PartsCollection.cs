using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsCollection :  MonoBehaviour
{
    [SerializeField] private static WaypointCreation _waypointCreation;
    public WaypointCreation wayPointOBJ;

    public static Action onPartsCollected;

    public static int currentScrap;
    public static int maxScrap = 3;

    private void Start() {
        _waypointCreation = wayPointOBJ.GetComponent<WaypointCreation>();
    }

    
    public static void CollectionMission()
    {
        if (currentScrap >= maxScrap)
        {
            ObjectiveManager._isPartsCollected = true;
            ObjectiveManager.onMissionComplete += Completed;
            ObjectiveManager.onMissionComplete?.Invoke();
            ObjectiveManager.onMissionComplete = null;
            Debug.Log("MISSION COMPLETE!");
        }
    }

    private static void Completed()
    {
        ObjectiveManager.onMissionStart -= CollectionMission;
        if (ObjectiveManager._isPartsCollected)
        {
            ObjectiveManager.onMissionStart += _waypointCreation.SpawnMissionEnd;
            ObjectiveManager.onMissionStart?.Invoke();
            ObjectiveManager.onMissionStart = null;
        }
    }

}
