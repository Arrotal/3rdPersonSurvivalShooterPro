using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    private CharacterController _pController;
    public bool _grounded,_extraJump;
    private float _moveSpeed = 7f, gravityValue = -9f, _pJHeight = 2f;
    [SerializeField]private float mouseSens = 1f;
    private Vector3 _pVelocity;
    [SerializeField]private Camera _pCameraTP, _pCameraFP, _pCameraActive;
    [SerializeField] private GameObject _gun;
    public Vector3 _gunPos;

    private Animator _animator;
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    void Start()
    {
        Application.targetFrameRate = 300;
        _pController = GetComponent<CharacterController>();
        if (_pController == null)
        {
            Debug.LogError("Character Controller is missing");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.Log("Animator is missing");
        }
        Cursor.lockState = CursorLockMode.Locked;
        MaxHealth = 10;
        Health = MaxHealth;
        _gunPos = _gun.transform.localPosition;
    }

    void Update()
    {
        Jump();
        Movement();
        CameraControl();
        UnlockMouse();
    }

    private void UnlockMouse()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;            
        }
        
    }
    private void CameraControl()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _pCameraActive.transform.position = _pCameraTP.transform.position;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _pCameraActive.transform.position = _pCameraFP.transform.position;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            //mousex to y
            Vector3 currentRot = transform.eulerAngles;
            currentRot.y += mouseX * mouseSens;
            transform.localRotation = Quaternion.AngleAxis(currentRot.y, Vector3.up);
            //mousey to camerax clamp 0-15
           
            Vector3 CurrentCamRot = _pCameraActive.transform.localEulerAngles;
            CurrentCamRot.x -= mouseY * mouseSens;
            CurrentCamRot.x = Mathf.Clamp(CurrentCamRot.x, 0, 22);
            _pCameraActive.transform.localRotation = Quaternion.AngleAxis(CurrentCamRot.x, Vector3.right);
        }
    }

    private void Movement()
    {

        if (!_grounded)
        {
            _pVelocity.y += gravityValue * Time.deltaTime;
        }
        if (_grounded)
        {
            _extraJump = true;
        }
        float xAxis = Input.GetAxisRaw("Horizontal");
        float zAxis = Input.GetAxisRaw("Vertical");
        _pVelocity = new Vector3( xAxis* _moveSpeed, _pVelocity.y,  zAxis* _moveSpeed) ;
        if (Mathf.Abs(xAxis) > 0 || Mathf.Abs(zAxis) > 0)
        {
            _animator.SetBool("Moving", true);
        }
        else
        {
            _animator.SetBool("Moving", false);
        }
        _pVelocity = transform.TransformDirection(_pVelocity);
        _pController.Move(_pVelocity * Time.deltaTime);
        _grounded = _pController.isGrounded;

    }

    private void Jump()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_grounded && _extraJump)
            {
                _extraJump = false;
                _animator.SetTrigger("Jump");
                _pVelocity.y = Mathf.Sqrt(_pJHeight * -3f * gravityValue);
            }
            else if (_grounded)
            {
                _animator.SetTrigger("Jump");
                _pVelocity.y = Mathf.Sqrt(_pJHeight * -3f * gravityValue);
            }
            
            
        }


    }
    public void Damage(int damage)
    {
        Health -= damage;
        Debug.Log("Health: " + Health);
        if (Health < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
