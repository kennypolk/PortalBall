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
}
