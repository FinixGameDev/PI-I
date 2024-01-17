using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _direction;
    [SerializeField] private float _forwardSpeed = 5f;
    [SerializeField] private float _strafeSpeed = 3.5f;

    private int _desiredLane = 1; //left lane = 0; middle lane = 1; right lane = 2
    [SerializeField] float _laneDistance = 2.5f; //distance between lanes

    [SerializeField] private float _jumpForce = 200;
    [SerializeField] private float _gravity = -9.98f;

    [SerializeField] private ParticleSystem _dustParticles;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called every frame
    private void Update()
    {
        if (_controller.isGrounded)
            _dustParticles.Play();
        else
            _dustParticles.Stop();

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && _controller.isGrounded)
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
    }

    private void FixedUpdate()
    {
        float targetX = _desiredLane * _laneDistance - _laneDistance;

        _direction.x = targetX - transform.position.x >= 0.05f ? _laneDistance * _strafeSpeed : 
                       targetX - transform.position.x <= -0.05f ? -_laneDistance * _strafeSpeed : 
                        0;

        _direction.z = _forwardSpeed;
        _direction.y += _gravity * Time.fixedDeltaTime;

        _controller.Move(_direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        _direction.y = _jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }

    }
}
