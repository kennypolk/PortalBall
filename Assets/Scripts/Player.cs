using System;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public float MoveSpeed = 20f;
    public float TurnSpeed = 0.25f;
    public float SprintSpeed = 40f;
    public float SprintMaxStamina = 20f;
    public float SprintChargeTime = 0.75f;
    public float ShootMinForce = 5f;
    public float ShootMaxForce = 50f;
    public float ShootChargeTime = 0.75f;
    public Transform BallPosition;

    [SyncVar]
    public Color Color;

    private float _inputX;
    private float _inputY;
    private bool _isSprinting;
    private float _currentMoveSpeed;
    private float _currentSprintStamina;
    private float _sprintChargeSpeed;
    private float _shootChargeSpeed;
    private float _currentShootForce;
    private Ball _ball;
    private bool _hasBall;
    private Collider2D _collider2D;
    private NetworkIdentity _networkIdentity;

    //hard to control WHEN Init is called (networking make order between object spawning non deterministic)
    //so we call init from multiple location (depending on what between spaceship & manager is created first).
    protected bool WasInit = false;

    public void Init()
    {
        if (WasInit)
        {
            return;
        }

        WasInit = true;
    }

    private void Awake()
    {
        GameManager.Players.Add(this);
    }

    private void Start ()
    {
        _shootChargeSpeed = (ShootMaxForce - ShootMinForce)/ShootChargeTime;
        _sprintChargeSpeed = SprintMaxStamina/SprintChargeTime;
        _currentSprintStamina = SprintMaxStamina;

        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material.color = Color;
        }

        //We don't want to handle collision on client, so disable collider there
        //_collider2D = this.GetComponentSafe<Collider2D>();
        //_collider2D.enabled = isServer;
        _networkIdentity = this.GetComponentSafe<NetworkIdentity>();

        //we MAY be awake late (see comment on _wasInit above), so if the instance is already there we init
        if (GameManager.Instance != null)
        {
            Init();
        }
    }

    [ClientCallback]
    private void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        _inputX = Input.GetAxis("Horizontal");
        _inputY = Input.GetAxis("Vertical");

        Sprint();

        //ShootBall();
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (!hasAuthority)
        {
            return;
        }

        Move();
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(string.Format("Player collision enter with {0}", col.gameObject.tag));
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log(string.Format("Player collision exit with {0}", col.gameObject.tag));
    }

    [ClientCallback]
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Trigger Enter");
        if (!hasAuthority)
        {
            Debug.Log("I'm a server");
            return;
        }

        if (col.CompareTag("Portal"))
        {
            Debug.Log("Portal Enter");
            var portal = col.gameObject.GetComponentSafe<Portal>();
            if (!portal.Delay)
            {
                portal.LinkedPortal.Delay = true;
                transform.position = portal.LinkedPortal.transform.position;
                //portal.Delay = true;
                //col.gameObject.GetComponent<Portal>().Port(gameObject);

            }
        }
        else if (col.CompareTag("Ball"))
        {
            _ball = col.GetComponentInParent<Ball>();
            CmdBallPickUp();
            _hasBall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Portal"))
        {
            col.gameObject.GetComponentSafe<Portal>().Delay = false;
        }
        else if (col.CompareTag("Ball"))
        {
            _hasBall = false;
            _ball = null;
        }
    }

    [Client]
    private void Move()
    {
        if (Math.Abs(_inputX) < 0.1f && Math.Abs(_inputY) < 0.1f)
        {
            _currentMoveSpeed = 0f;
            return;
        }

        var movement = new Vector2(_inputX, _inputY);
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }

        movement *= _isSprinting ? SprintSpeed : MoveSpeed;

        _currentMoveSpeed = movement.magnitude;

        movement *= Time.deltaTime;

        transform.Translate(movement, Space.World);
        
        var angle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.back), TurnSpeed);
    }

    private void ShootBall()
    {
        if (!_hasBall)
        {
            return;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            _currentShootForce = ShootMinForce;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            Debug.Log(string.Format("Current Force: {0} CurrentSpeed: {1}", _currentShootForce, _currentMoveSpeed));
            _ball.Shoot(_currentShootForce + _currentMoveSpeed, transform.up);
        }
        else if (Input.GetButton("Fire2"))
        {
            var tempForce = _currentShootForce + _shootChargeSpeed * Time.deltaTime;
            _currentShootForce = tempForce < ShootMaxForce ? tempForce : ShootMaxForce;
        }
    }

    private void Sprint()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (_currentSprintStamina > 0)
            {
                _isSprinting = true;
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            _isSprinting = false;
        }
        else if (Input.GetButton("Fire1"))
        {
            if (_currentSprintStamina > 0)
            {
                var tempStamina = _currentSprintStamina - _sprintChargeSpeed*Time.deltaTime;
                _currentSprintStamina = tempStamina > 0 ? tempStamina : 0;
            }

            if (Math.Abs(_currentSprintStamina) < 0.1f)
            {
                _isSprinting = false;
            }
        }
        else
        {
            var tempStamina = _currentSprintStamina + _sprintChargeSpeed*Time.deltaTime;
            _currentSprintStamina = tempStamina < SprintMaxStamina ? tempStamina : SprintMaxStamina;
        }

        Debug.Log(string.Format("Stamina: {0}", _currentSprintStamina));
    }

    [Command]
    private void CmdBallPickUp()
    {
        _ball.PlayerCollision(BallPosition);
    }
}
