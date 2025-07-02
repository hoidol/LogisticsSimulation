using UnityEngine;
using System.Collections;
using TMPro;
public class ConveyorAdjustDetailPanel : AdjustDetailPanel
{
    public TMP_InputField linkedAsrsInputField;

    public override void SetMachine(Machine machine)
    {
        base.SetMachine(machine);
        Conveyor conveyor = machine as Conveyor;
        linkedAsrsInputField.text = conveyor.destinationId;
    }

    public void OnClickedEdit()
    {
        if (string.IsNullOrEmpty(linkedAsrsInputField.text))
            return;

        bool have = false;
        MachineManager.Instance.allMachines.ForEach(e => {
            if (e.id == linkedAsrsInputField.text)
            {
                have = true;
                return;
            }
        });
        if (!have)
            return;
            
        machine.SetDestination(linkedAsrsInputField.text);
        MachineManager.Instance.Save();
    }
}
