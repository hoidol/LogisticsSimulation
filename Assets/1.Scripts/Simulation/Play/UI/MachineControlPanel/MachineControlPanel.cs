using TMPro;
using UnityEngine;

public abstract class MachineControlPanel : MonoBehaviour
{
    public MachineName machineName;

    public TMP_Text idText;

    public virtual void SetMachine(Machine machine)
    {
        idText.text = machine.id;
    }

    public void UpdatePanel()
    {

    }
}
