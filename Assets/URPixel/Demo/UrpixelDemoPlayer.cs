using Unity.Cinemachine;
using UnityEngine;
using Urpixel;

public class UrpixelDemoPlayer : UrpixelSnap
{

    private Camera _cam;
    private CharacterController _characterController;
    private Vector3 _moveDirection;
    private Vector3 _gravity = new(0f, -9f, 0f);
    private Vector3 _forward;
    private Vector3 _right;
    private float _moveSpeed = 5f;
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _cam = Camera.main;
    }
    
    private void Update()
    {
        _forward = _cam.transform.forward.ProjectOntoPlane(Vector3.up).normalized;
        
        MovePlayer();
        RotateCamera();
        
        base.Update();
    }

    private void MovePlayer()
    {
        _moveDirection = Vector3.zero;
        _right = _cam.transform.right.ProjectOntoPlane(Vector3.up).normalized;
        
        if (Input.GetKey(KeyCode.W))
            _moveDirection += _forward;
        else if (Input.GetKey(KeyCode.S))
            _moveDirection -= _forward;
        
        if (Input.GetKey(KeyCode.A))
            _moveDirection -= _right;
        else if (Input.GetKey(KeyCode.D))
            _moveDirection += _right;

        _characterController.Move(_moveDirection.normalized * (_moveSpeed * Time.deltaTime) + _gravity * Time.deltaTime);
    }

    private void RotateCamera()
    {
        if (Input.GetKey(KeyCode.Q))
            _cam.transform.eulerAngles += new Vector3(0f, 45f, 0f) * Time.deltaTime;
        else if (Input.GetKey(KeyCode.E))
            _cam.transform.eulerAngles -= new Vector3(0f, 45f, 0f) * Time.deltaTime;
    }
}
