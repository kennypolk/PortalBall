using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _rigidBody2D;

	// Use this for initialization
	void Start () 
	{
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            _rigidBody2D.velocity = Vector2.zero;
        }
    }

    public void Shoot(Vector3 moveDirection, float speed)
    {
        transform.parent = null;

        _rigidBody2D.AddForce(moveDirection.normalized * speed, ForceMode2D.Impulse);
    }
}
