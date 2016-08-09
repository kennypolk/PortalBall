using UnityEngine;

public static class Extensions 
{
    public static T GetComponentSafe<T>(this GameObject obj) where T : Component
    {
        var component = obj.GetComponent<T>();

        if (component == null)
        {
            Debug.LogError(string.Format("GetComponent failed Type: {0} Object: {1} ", typeof(T), obj.name), obj);
        }

        return component;
    }

    public static T GetComponentSafe<T>(this Component obj) where T : Component
    {
        var component = obj.GetComponent<T>();

        if (component == null)
        {
            Debug.LogError(string.Format("GetComponent failed Type: {0} Object: {1} ", typeof(T), obj.name), obj);
        }

        return component;
    }
}
