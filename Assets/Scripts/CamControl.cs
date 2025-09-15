using System.Collections;
using UnityEngine;

public class CamControl : MonoBehaviour
{

    [SerializeField] private GameObject _normalCam;


    public GameObject player;
    public CameraType _currentType;

    void Start() 
    {
        _normalCam.SetActive(true);
        _currentType = CameraType.Basic;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentType = CameraType.Basic;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentType = CameraType.Combat;
        }
        
    }

}

public enum CameraType
{
    Basic,
    Combat
}
