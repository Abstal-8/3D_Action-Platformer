using System.Collections;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public float Jumpforce;
    public float Strafeforce;
    public float Dashforce;
    public float Floordrag;
    public float Airmultiply;
    public float Jumpcooldown;
    public float Dashcooldown;
    public float Strafecooldown;
    public float Minslopeangle;
    public float Playerheight;
    public Camera Maincam;
    public LayerMask Groundmask;


    private Rigidbody _rb;
    private CamControl _cameraType;
    private RaycastHit _slopeHit;
    private Vector3 _playMove;
   [SerializeField] private bool _isGrounded;
   [SerializeField] private bool _isjumpReady;
   [SerializeField] private bool _isStrafeReady;
   [SerializeField] private bool _isDashReady;
   [SerializeField] private bool _exitSlopeJump;
   [SerializeField] private float _rotateSpeed = -5.0f;
   [SerializeField] private float _slopeSpeed;
    
    // Start is called before the first frame update
    void Start()
    {

        _isjumpReady = true;
        _isDashReady = true;
        _isStrafeReady = true;
        _rb = GetComponent<Rigidbody>();
        _cameraType = gameObject.GetComponent<CamControl>();
    }

    private void Update() 
    {
        SpeedLimit();

        if (Input.GetKeyDown(KeyCode.Space) && _isjumpReady && _isGrounded || Input.GetKeyDown(KeyCode.Space) && _isjumpReady && _isGrounded && OnSlope())
        {
            _isjumpReady = false;

            Jump();

            Invoke(nameof(ResetJump), Jumpcooldown);
        }

        RaycastHit hit;
        _isGrounded = Physics.SphereCast(transform.position, 0.55f, Vector3.down, out hit, 0.5f, Groundmask);
        _rb.drag = _isGrounded ? Floordrag : 0;

        if (Input.GetKeyDown(KeyCode.Space) && OnSlope())
            _exitSlopeJump = true;
            


        if (OnSlope() && !_exitSlopeJump)
        {
            _rb.AddRelativeForce(GetSlopeDirection() * _slopeSpeed, ForceMode.Force);
            if (_rb.velocity.y < 0)
            {
                _rb.AddForce(Vector3.down * 90f, ForceMode.Force);
            }
        }

        
        if (_cameraType._currentType == CameraType.Basic)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && _isDashReady)
            {
                _isDashReady = false;

                Dash();

                Invoke(nameof(ResetDash), Dashcooldown);
            }
        }
        

        if (_cameraType._currentType == CameraType.Combat)
        {

            if (Input.GetKeyDown(KeyCode.LeftShift) && _isStrafeReady)
            {
                _isStrafeReady = false;

                Strafe();

                Invoke(nameof(ResetStrafe), Strafecooldown);
            }

        }
    }

    void FixedUpdate()
    {  
        // DO NOT FORGET to take notes on the movement vectors from below link (this can happen during or after development)
        // https://www.youtube.com/watch?v=7kGCrq1cJew&list=LL&index=2&ab_channel=iHeartGameDev
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 frontMove = transform.InverseTransformVector(Maincam.transform.forward);
        Vector3 rightMove = transform.InverseTransformVector(Maincam.transform.right);
        frontMove.y = 0;
        rightMove.y = 0;
        frontMove = frontMove.normalized;
        rightMove = rightMove.normalized;
        _playMove = (frontMove * verticalInput + rightMove * horizontalInput);
        // * MOVEMENT NOTES END AFTER THIS COMMENT *

        if (_cameraType._currentType == CameraType.Basic)
        {
            if (_isGrounded)
            {
                _rb.AddRelativeForce(_playMove.normalized * Speed);
                  
            }
            else if (!_isGrounded)
                _rb.AddRelativeForce(_playMove.normalized * Speed * Airmultiply);

        }
        

        if (_cameraType._currentType == CameraType.Combat)
        {
            BattleMode();

            if (_isGrounded)
                _rb.AddRelativeForce(_playMove.normalized * Speed);

            else if (!_isGrounded)
                _rb.AddRelativeForce(_playMove.normalized * Speed * Airmultiply);

        }

    }

    void BattleMode()
    {
        float mouseHorizontal = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseHorizontal * _rotateSpeed, 0);
    }

    void SpeedLimit()
    {
        if (OnSlope() && !_exitSlopeJump)
        {
            if (_rb.velocity.magnitude > Speed)
            {
                _rb.velocity = _rb.velocity.normalized * Speed;
            }
        }
        else {

            Vector3 currentVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            if (currentVelocity.magnitude > Speed)
            {
                Vector3 limitVelocity = currentVelocity.normalized * Speed;
                _rb.velocity = new Vector3(limitVelocity.x, _rb.velocity.y, limitVelocity.z);
            }

        }
    }

    void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _rb.AddForce(transform.up * Jumpforce, ForceMode.Impulse);
    }

    void Dash()
    {
        _rb.AddForce(transform.forward.normalized * Dashforce, ForceMode.Impulse);
    }

    void Strafe()
    {
        
        _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
        
        switch(Input.GetAxis("Horizontal"))
        {
            case 1:
                _rb.AddForce(transform.right.normalized * Strafeforce, ForceMode.Impulse);
                break;
            case -1:
                _rb.AddForce(-transform.right.normalized * Strafeforce, ForceMode.Impulse);
                break;
        }

        switch(Input.GetAxis("Vertical"))
        {
            case 1:
                _rb.AddForce(transform.forward.normalized * Strafeforce, ForceMode.Impulse);
                break;
            case -1:
                _rb.AddForce(-transform.forward.normalized * Strafeforce, ForceMode.Impulse);
                break;
        }
    }
    
    void ResetStrafe()
    {
        _isStrafeReady = true;
    }
    
    void ResetDash()
    {
        _isDashReady = true;
    }

    void ResetJump()
    {
        _isjumpReady = true;
        _exitSlopeJump = false;
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down * Playerheight, out _slopeHit, Playerheight))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle > Minslopeangle && angle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(_playMove, _slopeHit.normal).normalized;
    }

    

    private void OnDrawGizmos() {
        Ray beam = new Ray(transform.position, transform.forward);
        Ray ray = new Ray(transform.position, Vector3.down);
        Ray planeRay = new Ray(transform.position, transform.forward);
        //RaycastHit hit;
       // int rayLen = 10;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * Playerheight);
        
       // Gizmos.DrawRay(beam);
        //Gizmos.DrawRay(ray);

       /* Debug.DrawRay(transform.position, transform.forward * rayLen, Color.cyan);

        if (Physics.Raycast(ray, out hit, rayLen))
        {
            Vector3 dir = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            Debug.DrawRay(hit.point, dir * rayLen, Color.cyan);
        } */
        

    }

    
}
