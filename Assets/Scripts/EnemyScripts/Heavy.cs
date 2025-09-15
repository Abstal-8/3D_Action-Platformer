using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Heavy : Enemy
{
    
    [SerializeField] private float _spreadAngle = 0.1f;

    protected override void AttackPlayer()
    {
        base.AttackPlayer();
        if (!base.alreadyAttacked)
        {
            for (int i = 0; i < bulletAmt; i++)
            {
                Shoot();
            }

            base.alreadyAttacked = true;
            Invoke(nameof(base.ResetAttack), base.timeBtwnAttack);
        }
    }

    void Shoot()
    {
        GameObject p = Instantiate(projectile, barrelEnd.position, transform.rotation);
        Vector3 dir = barrelEnd.forward;
        dir.x += Random.Range(-_spreadAngle, _spreadAngle);
        dir.y += Random.Range(-_spreadAngle, _spreadAngle);
        p.GetComponent<Rigidbody>().AddForce(dir * _bulletSpeed, ForceMode.Impulse);
    }
}
