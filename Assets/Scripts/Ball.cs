using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    public void PlayerCollision(Transform ballPosition)
    {
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
        transform.parent = ballPosition;
        transform.localPosition = ballPosition.localPosition;
    }

    public void Shoot(float force)
    {
        _rigidbody2D.isKinematic = false;
        _rigidbody2D.velocity = transform.up * force;
        transform.parent = null;
    }

    private void Start ()
    {
        _rigidbody2D = this.GetComponentSafe<Rigidbody2D>();
    }
}
