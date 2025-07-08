using UnityEngine;
using System.Collections;

public class ObjectControlContainer : MonoBehaviour
{
    public string objName;

    public virtual void Init()
    {
        
    }
    public virtual void SetControlContainer(string oName)
    {
        gameObject.SetActive(objName == oName);
        if (objName == oName)
        {
            UpdateContainer();
        }
    }
    public virtual void UpdateContainer()
    {
    }
    public virtual void UpdateControl()
    {
        
    }
}
