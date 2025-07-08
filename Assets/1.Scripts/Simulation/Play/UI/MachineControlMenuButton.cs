using UnityEngine;
using UnityEngine.UI;
public class MachineControlMenuButton : ObjectControlMenuButton
{
    public MachineName machineName;
    public override void Awake()
    {
        base.Awake();
        objectName = machineName.ToString();
    }
}
