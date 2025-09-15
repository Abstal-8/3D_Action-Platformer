using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Rider.Unity.Editor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public int health;
    public int bulletDamage;
    public int bulletAmt;
    [SerializeField] protected float _bulletSpeed;

    //Most variables and methods are from this link: https://youtu.be/UjkSFoLxesw?si=1TJOVHKukevjLdhx implements basic enemy AI.
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public GameObject projectile;
    public Transform barrelEnd;

    //Patrolling
    public Vector3 walkPoint;
    [SerializeField] private bool walkPointSet;
    public float walkPointRange;

    //Attacking 
    public float timeBtwnAttack;
    protected bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    private bool inSightRange, inAttackRange;

    private void Awake() {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
       // alreadyAttacked = true; //DELETE THIS LINE AFTER TESTING!!!
    }

    
    

    private void Update() {

        inSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!inSightRange && !inAttackRange)
            Patrolling();

        if (inSightRange && !inAttackRange)
            ChasePlayer();

        if (inSightRange && inAttackRange)
            AttackPlayer();
    }

    private void Patrolling()
    {
        agent.autoBraking = false;
        agent.angularSpeed = 200;
        if (!walkPointSet)
            SearchWalkPoint();
        
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        agent.angularSpeed = 200;
    }

    protected virtual void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        barrelEnd.transform.LookAt(player.position);
        agent.angularSpeed = 0;
    }

    protected void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void DamageTaken(int damageAmt)
    {
        health -= damageAmt;
        if (health <= 0)
        {
            EnemiesDefeated.OnEnemyKilled?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
