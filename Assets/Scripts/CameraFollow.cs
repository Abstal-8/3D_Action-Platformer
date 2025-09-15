using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Transform combatReticle;
    public Transform orientation;
    public Rigidbody playerRB;
    public GameObject normalCam;
    public GameObject battleCam;
    public GameObject player;

    private float rotationSpeed = 8;
    private CamControl _cameraType;
    
    public float distance;
    public float height;
    public float smoothTime;
    public float shoulderOffset;
    public float verticalRotateSpeed;
    public bool switchShoulder;
    Vector3 lookTarget;
    


    void Start()
    {
        _cameraType = player.GetComponent<CamControl>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        if (_cameraType._currentType == CameraType.Basic)
        {
            normalCam.SetActive(true);
            battleCam.SetActive(false);

            Vector3 viewDir = playerTransform.position - new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;

            float Horizontal = Input.GetAxis("Horizontal");
            float Vertical = Input.GetAxis("Vertical");
            Vector3 combatMovement = orientation.forward * Vertical + orientation.right * Horizontal;

            if (combatMovement != Vector3.zero)
            {
                playerTransform.forward = Vector3.Slerp(playerTransform.forward, combatMovement.normalized, Time.deltaTime * rotationSpeed);
            }

            
        }

        if (_cameraType._currentType == CameraType.Combat)
        {
            normalCam.SetActive(false);
            battleCam.SetActive(true);

            Vector3 combatView = combatReticle.position - new Vector3(transform.position.x, combatReticle.position.y, transform.position.z);
            orientation.forward = combatView.normalized;

            ReticleAim();

            lookTarget = combatReticle.position; 
        }

    }

    void ReticleAim()
    {
        Vector3 mouseTest = new(0f, Input.GetAxis("Mouse Y") * 0.5f, 0f);
        Vector2 aimCeiling = playerTransform.position;
        float minCeil = aimCeiling.y - 2.5f, maxCeil = aimCeiling.y + 2.5f;
        

       
        float boundary = Mathf.Clamp(combatReticle.position.y, minCeil, maxCeil);
        
        
        combatReticle.position = new(combatReticle.position.x, boundary, combatReticle.position.z);
        combatReticle.position += mouseTest;


        if (combatReticle.position.y >= maxCeil)
        {
            combatReticle.position = new(combatReticle.position.x, maxCeil, combatReticle.position.z);
        }
        else if (combatReticle.position.y <= minCeil)
        {
            combatReticle.position = new(combatReticle.position.x, minCeil, combatReticle.position.z);
        }

    }

    private void OnDrawGizmos() 
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(lookTarget, new Vector3(0.5f, 0.5f, 0.5f));


        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + (transform.forward * 2) + transform.up, 0.5f);
        Gizmos.DrawWireSphere(transform.position + (transform.forward * 2.12f) + (-transform.up), 0.5f);

        
    }
}
