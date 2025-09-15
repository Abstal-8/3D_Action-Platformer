using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Small : Enemy
{

    
    protected override void AttackPlayer()
    {
        base.AttackPlayer();
        if (!base.alreadyAttacked)
        {
            Shoot();

            base.alreadyAttacked = true;
            Invoke(nameof(base.ResetAttack), base.timeBtwnAttack);
        }
    }

    void Shoot()
    {
        GameObject p = Instantiate(projectile, barrelEnd.position, transform.rotation);
        Vector3 dir = barrelEnd.forward;
        p.GetComponent<Rigidbody>().AddForce(dir * _bulletSpeed, ForceMode.Impulse);
    }
}
