using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject elimMissionPanel;
    public TextMeshProUGUI scrapText;
    public TextMeshProUGUI killText;


    private void OnEnable() {
        EnemiesDefeated.OnEnemyKilled += EnemiesDefeated.KillCount;
    }

    private void OnDisable() {
        EnemiesDefeated.OnEnemyKilled -= EnemiesDefeated.KillCount;
    }
 
 
    void Update()
    {
        killText.text = $"Killed: {EnemiesDefeated.enemiesKilled} of {EnemiesDefeated.maxEnemies}";
        scrapText.text = $"Scrap: {PartsCollection.currentScrap} of {PartsCollection.maxScrap}";
        if (Input.GetKeyDown(KeyCode.Q))
        {
            elimMissionPanel.SetActive(true);
        }
    }
    
}
