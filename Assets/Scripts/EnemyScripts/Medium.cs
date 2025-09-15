using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medium : Enemy
{
    private WaitForSeconds _bulletDelay = new(0.3f);


    protected override void AttackPlayer()
    {
        base.AttackPlayer();
        if (!base.alreadyAttacked)
        {
            StartCoroutine(ShotBurst());

            base.alreadyAttacked = true;
            Invoke(nameof(base.ResetAttack), base.timeBtwnAttack);
        }
    }

    IEnumerator ShotBurst()
    {
        for (int i = 0; i < bulletAmt; i++)
        {
            Shoot();
            yield return _bulletDelay;
        }
    }

    void Shoot()
    {
        GameObject p = Instantiate(projectile, barrelEnd.position, transform.rotation);
        Vector3 dir = barrelEnd.forward;
        p.GetComponent<Rigidbody>().AddForce(dir * _bulletSpeed, ForceMode.Impulse);
    }
}
