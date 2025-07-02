using UnityEngine;

public class AdjustDetailPanel : MonoBehaviour
{
    public MachineName machineName;
    public Machine machine;
    public virtual void SetMachine(Machine machine)
    {
        this.machine = machine;
    }
}
