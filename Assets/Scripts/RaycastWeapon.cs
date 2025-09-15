using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;
    public Camera _mainCam;

    
    private WaitForSeconds _bulletDuraton = new WaitForSeconds(0.07f);
    private LineRenderer _lineRenderer;
    private float _nextFire;
    [SerializeField] private GameObject _battleCamOBJ;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    
    void Update()
    {
        //Debug
        Vector3 bugLineOrigin = _mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(bugLineOrigin, _mainCam.transform.forward * weaponRange, Color.green);


        if (Input.GetButtonDown("Fire1") && Time.time > _nextFire && _battleCamOBJ.activeSelf)
        {
            _nextFire = Time.time + fireRate;
            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = _mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            _lineRenderer.SetPosition(0, gunEnd.position);

            if (Physics.Raycast(rayOrigin, _mainCam.transform.forward, out hit, weaponRange))
            {
                _lineRenderer.SetPosition(1, hit.point);

                // we set a reference to the enemy script if what we hit has that script.
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                BulletProperties projectile = hit.collider.GetComponent<BulletProperties>();

                if (enemy != null)
                {
                    enemy.DamageTaken(gunDamage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce, ForceMode.Impulse);
                }
                
                if (projectile != null && projectile._isDestrucable)
                {
                    Destroy(projectile.gameObject);
                }

            }
            else {
                // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                _lineRenderer.SetPosition(1, rayOrigin + (_mainCam.transform.forward * weaponRange));
            }
        }
    }


    IEnumerator ShotEffect()
    {
        _lineRenderer.enabled = true;
        yield return _bulletDuraton;
        _lineRenderer.enabled = false;
    }



}
