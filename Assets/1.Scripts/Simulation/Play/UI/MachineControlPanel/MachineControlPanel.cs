using UnityEngine;

public abstract class MachineControlPanel : MonoBehaviour
{
    public MachineName machineName;
    public abstract void SetMachine(Machine machine);

    public void UpdatePanel()
    {

    }
}
