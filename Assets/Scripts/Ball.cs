using UnityEngine;
using UnityEngine.Networking;

public class Ball : NetworkBehaviour
{
    [HideInInspector]
    public Rigidbody2D Rigidbody2D;

    [Server]
    public void PlayerCollision(Transform ballPosition)
    {
        //_rigidbody2D.isKinematic = true;
        //_rigidbody2D.velocity = Vector2.zero;
        ////transform.parent = ballPosition;
        ////transform.localPosition = ballPosition.localPosition;
        //_rigidbody2D.MovePosition(ballPosition.position);
        //_rigidbody2D.MoveRotation(0f);
        //transform.parent = ballPosition;
    }

    [Server]
    public void Shoot(float force, Vector3 direction)
    {
        //_rigidbody2D.isKinematic = false;
        //_rigidbody2D.velocity = direction * force;
        //transform.parent = null;
    }

    private void Start()
    {
        Rigidbody2D = this.GetComponentSafe<Rigidbody2D>();
        //_rigidbody2D = this.GetComponentSafe<Rigidbody2D>();
    }
}
