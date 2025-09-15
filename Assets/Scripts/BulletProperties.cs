using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class BulletProperties : MonoBehaviour
{
    public bool _isDestrucable;

    private float _randomVal;
    private int despawnTime = 5;
    


    void Start()
    {
        _randomVal = Random.value;
        _isDestrucable = true;
        StartCoroutine(DespawnBullet());
    }

    
    void Update()
    {
        // 10% chance that this object can be destroyed when shot (Random.value from 0 to 1)
        if (_randomVal > 0.9)
        {
            _isDestrucable = false;
            this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        }
    }

    IEnumerator DespawnBullet()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(this.gameObject);
    }
}
