using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 10f;
    [SerializeField] private float _maxSpeed = 25f;

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
    private AudioSource _audioSource;

    [SerializeField] private AudioClip _hitSFX;

    public StaminaBarController staminaBarController;

    // Start is called before the first frame update
    void Start()
    {
        //Evitar que os obstáculos fiquem desligados ao trocar de scene
        Physics.IgnoreLayerCollision(6, 7, false);
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called every frame
    private void Update()
    {
        _animator.speed = forwardSpeed / 10;
        _animator.SetBool("isGrounded", _controller.isGrounded);

        forwardSpeed = _maxSpeed * (staminaBarController.slider.value / staminaBarController.slider.maxValue);

        //Control DustParticles
        if (_controller.isGrounded && PlayerManager.isGameStarted)
            _dustParticles.Play();
        else
            _dustParticles.Stop();

        //Check movement
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || SwipeManager.swipeUp) && _controller.isGrounded && !_animator.GetBool("isSliding"))
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

        if (staminaBarController.slider.value == 0)
            PlayerManager.gameOver = true;
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
            _audioSource.PlayOneShot(_hitSFX);            //Ativar Som
            _animator.SetTrigger("hit");                  //Ativar Animação
            _impactParticles.Play();                      //Emitir particulas de impacto
            staminaBarController.ReduzirEnergiaColisao(); //Reduzir a motivação do cão em 300
            _dustParticles.Stop();                        //Parar de emitir particulas de pó
            StartCoroutine("InvencibilityWindow", 3f);    //Ligar I-frames durante 3 segundos
        }
    }

    private IEnumerator InvencibilityWindow(float sec)
    {
        Physics.IgnoreLayerCollision(6, 7, true); //Desliga as colisões
        float timer = 0;
        StartCoroutine("SetMesh", sec); //Coloca a mesh a piscar

        while (timer < sec) //Timer
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Physics.IgnoreLayerCollision(6, 7, false); //Liga as colisões
    }

    private IEnumerator SetMesh(float sec)
    {
        var mesh = GetComponentInChildren<SkinnedMeshRenderer>().gameObject;

        float timer = 0;
        while (timer < sec)
        {
            timer += 0.1f;
            mesh.SetActive(!mesh.active); //Inverte estado da mesh
            yield return new WaitForSeconds(0.1f);
        }

        mesh.SetActive(true); //Coloca sempre no final a true
    }

    private IEnumerator Slide()
    {
        _animator.SetBool("isSliding", true);
        _controller.height = 1;
        _controller.center = new Vector3(0, -0.5f);
        _gravity *= 3;

        yield return new WaitForSeconds(0.5f);

        _animator.SetBool("isSliding", false);
        _gravity /= 3;
        _controller.height = 2;
        _controller.center = Vector3.zero;

    }
}
