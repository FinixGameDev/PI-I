using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _direction;
    [SerializeField] private float _forwardSpeed = 5f;

    private int _desiredLane = 1; //left lane = 0; middle lane = 1; right lane = 2
    [SerializeField] float _laneDistance = 2.5f; //distance between lanes

    [SerializeField] private float _jumpForce = 8;
    [SerializeField] private float _gravity = -30f;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called every frame
    private void Update()
    {
        _direction.z = _forwardSpeed;
        _direction.y += _gravity * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) && _controller.isGrounded)
            Jump();

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            _desiredLane++;
            if (_desiredLane > 2)
                _desiredLane = 2;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            _desiredLane--;
            if (_desiredLane < 0)
                _desiredLane = 0;
        }

        //Calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        switch (_desiredLane)
        {
            case 0:
                targetPosition += Vector3.left * _laneDistance;
                break;
            case 2:
                targetPosition += Vector3.right * _laneDistance;
                break;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, 80 * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
       _controller.Move(_direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        _direction.y = _jumpForce;
    }
}
