using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D { get; set; }
    public Collider2D Collider2D { get; set; }

    private void Start ()
    {
        Rigidbody2D = this.GetComponentSafe<Rigidbody2D>();
        Collider2D = this.GetComponentSafe<Collider2D>();
    }
}
