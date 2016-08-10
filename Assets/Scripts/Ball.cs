using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Ball : NetworkBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private NetworkIdentity _networkIdentity;

    public void PlayerCollision(Transform ballPosition)
    {
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
        //transform.parent = ballPosition;
        //transform.localPosition = ballPosition.localPosition;
        _rigidbody2D.MovePosition(ballPosition.position);
        transform.parent = ballPosition;
    }

    [Server]
    public void AssignAuth(NetworkConnection conn)
    {
        _networkIdentity.AssignClientAuthority(conn);
    }

    public void Shoot(float force, Vector3 direction)
    {
        _rigidbody2D.isKinematic = false;
        _rigidbody2D.velocity = direction * force;
        transform.parent = null;
    }

    private void Start ()
    {
        _rigidbody2D = this.GetComponentSafe<Rigidbody2D>();
        _networkIdentity = this.GetComponentSafe<NetworkIdentity>();
    }
}
