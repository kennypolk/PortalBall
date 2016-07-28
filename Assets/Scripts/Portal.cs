using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    public GameObject LinkedPortal;
    public bool Delay;

    // Use this for initialization
	void Start () 
	{
	    
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    public void Port(GameObject obj)
    {
        if (!Delay)
        {
            obj.transform.position = LinkedPortal.transform.position;
            LinkedPortal.GetComponent<Portal>().Delay = true;
        }
    }
}
