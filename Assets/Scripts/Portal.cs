using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
    public Portal LinkedPortal;
    public bool Delay;

    //public void Port(GameObject obj)
    //{
    //    if (!Delay)
    //    {
    //        obj.transform.position = LinkedPortal.transform.position;
    //        LinkedPortal.GetComponent<Portal>().Delay = true;
    //    }
    //}
}
