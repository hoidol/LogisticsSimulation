using UnityEngine;

public class MachineDetailPanel : MonoBehaviour
{
    public MachineName machineName;
    public Machine machine;

    public void SetMachine(Machine machine)
    {
        this.machine = machine;
    }
    
}
