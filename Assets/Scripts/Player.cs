using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MoveSpeed = 20f;
    public float TurnSpeed = 0.25f;
    public float SprintSpeed = 40f;
    public float SprintStamina = 1f;
    public float ShootMinForce = 5f;
    public float ShootMaxForce = 50f;
    public float ShootChargeTime = 0.75f;
    public Transform BallPosition;

    private float _inputX;
    private float _inputY;
    private float _currentMoveSpeed;
    private float _currentSprintStamina;
    private float _shootChargeSpeed;
    private float _currentShootForce;
    private Ball _ball;
    private bool _hasBall;

    private void Start ()
    {
        _shootChargeSpeed = (ShootMaxForce - ShootMinForce)/ShootChargeTime;
    }
    
    private void Update ()
    {
        _inputX = Input.GetAxis("Horizontal");
        _inputY = Input.GetAxis("Vertical");

        ShootBall();
    }

    private void FixedUpdate()
    {
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

    private void OnTriggerEnter2D(Collider2D col)
    {  
        if (col.CompareTag("Portal"))
        {
            var portal = col.gameObject.GetComponent<Portal>();
            if (!portal.Delay)
            {
                col.gameObject.GetComponent<Portal>().Port(gameObject);
                portal.Delay = true;
            }
        }
        else if (col.CompareTag("Ball"))
        {
            _ball = col.GetComponentInParent<Ball>();
            _ball.PlayerCollision(BallPosition);
            _hasBall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Portal"))
        {
            col.gameObject.GetComponent<Portal>().Delay = false;
        }
        else if (col.CompareTag("Ball"))
        {
            _hasBall = false;
            _ball = null;
        }
    }

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

        movement *= MoveSpeed;

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
}
