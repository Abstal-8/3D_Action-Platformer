using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{

    [SerializeField] private int playerHealth = 10;
    private Enemy enemy;

    public GameObject enemyObj;
    

    private void Start() {
        enemy = enemyObj.GetComponent<Enemy>();
    }



    public void DamageTaken(int damageAmt)
    {
        playerHealth -= damageAmt;
        if (playerHealth <= 0)
        {
            Debug.Log("Player has been defeated!");
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.collider.gameObject.CompareTag("Projectile"))
        {
            DamageTaken(enemy.bulletDamage);
        }
    }

    
}
