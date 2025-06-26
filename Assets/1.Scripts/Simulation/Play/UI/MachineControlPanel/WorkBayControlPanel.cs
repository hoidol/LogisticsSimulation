using UnityEngine;

public class WorkBayControlPanel : MachineControlPanel
{
    Workbay workbay;
    public override void SetMachine(Machine machine)
    {
        workbay = machine as Workbay;
    }

    
}
