using System;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private const float Tolerance = 0.001f;

    private bool _hasBall;
    private GameObject _ballObject;
    private Vector3 _moveDirection;

    public float MovementSpeed = 6f;
    public float RotationSpeed = 1f;
    public float KickSpeed = 6f;

	// Use this for initialization
	private void Start () 
	{
	
	}
	
	// Update is called once per frame
	private void Update ()
	{
        Movement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

	    if (Input.GetButton("Fire2"))
	    {
            ShootBall();
        }
	}

    private void Movement(float x, float y)
    {
        if (Math.Abs(x) < Tolerance && Math.Abs(y) < Tolerance)
        {
            return;
        }

        _moveDirection = new Vector3(x, y);

        transform.Translate(_moveDirection * MovementSpeed * Time.deltaTime, Space.World);

        var angle = Mathf.Atan2(_moveDirection.x, _moveDirection.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.back), RotationSpeed);
    }

    private void ShootBall()
    {
        if (!_hasBall || _ballObject == null)
        {
            return;
        }

        var ball = _ballObject.GetComponent<Ball>();
        if (ball != null)
        {
            ball.Shoot(_moveDirection, KickSpeed);
            _hasBall = false;
            _ballObject = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Ball"))
        {
            _hasBall = true;
            _ballObject = col.gameObject;
            _ballObject.transform.parent = transform;
        }    
        else if (col.tag.Equals("Portal"))
        {
            var portal = col.gameObject.GetComponent<Portal>();
            if (!portal.Delay)
            {
                col.gameObject.GetComponent<Portal>().Port(gameObject);
                portal.Delay = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag.Equals("Portal"))
        {
            col.gameObject.GetComponent<Portal>().Delay = false;
        }
    }
}
