using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 10f;

    [SerializeField] private float _strafeSpeed = 3.5f;

    private int _desiredLane = 1; //left lane = 0; middle lane = 1; right lane = 2
    [SerializeField] float _laneDistance = 2.5f; //distance between lanes

    [SerializeField] private float _jumpForce = 200;
    [SerializeField] private float _gravity = -9.98f;

    [SerializeField] private ParticleSystem _dustParticles;
    [SerializeField] private ParticleSystem _impactParticles;

    private Animator _animator;
    private CharacterController _controller;
    private Vector3 _direction;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called every frame
    private void Update()
    {
        _animator.speed = forwardSpeed / 10;
        _animator.SetBool("isGrounded", _controller.isGrounded);

        forwardSpeed += 2f * Time.deltaTime;

        //Control DustParticles
        if (_controller.isGrounded && PlayerManager.isGameStarted)
        {
            _dustParticles.Play();
        }
        else
            _dustParticles.Stop();

        //Check movement
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || SwipeManager.swipeUp) && _controller.isGrounded)
            Jump();

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || SwipeManager.swipeRight)
        {
            _desiredLane++;
            if (_desiredLane > 2)
                _desiredLane = 2;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || SwipeManager.swipeLeft)
        {
            _desiredLane--;
            if (_desiredLane < 0)
                _desiredLane = 0;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || SwipeManager.swipeDown)
        {
            StartCoroutine(Slide());
        }
    }

    private void FixedUpdate()
    {
        float targetX = _desiredLane * _laneDistance - _laneDistance;

        _direction.x = targetX - transform.position.x >= 0.08f ? _laneDistance * _strafeSpeed : 
                       targetX - transform.position.x <= -0.08f ? -_laneDistance * _strafeSpeed : 
                        0;

        _direction.z = forwardSpeed;
        _direction.y += _gravity * Time.fixedDeltaTime;

        if (PlayerManager.isGameStarted)
        {
            _controller.Move(_direction * Time.fixedDeltaTime);
            _animator.SetBool("isGameStarted", true);
        }
    }

    private void Jump()
    {
        _direction.y = _jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            _animator.SetTrigger("hit");
            _impactParticles.Play();
            _dustParticles.Stop();

            forwardSpeed = Mathf.Min(forwardSpeed--, 10);
        }

    }

    private IEnumerator Slide()
    {
        _animator.SetTrigger("slide");
        _controller.height = 1;
        _controller.center = new Vector3(0, -0.5f);

        yield return new WaitForSeconds(1.4f);

        _controller.height = 2;
        _controller.center = Vector3.zero;
    }
}
