using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MachineControlContainer : ObjectControlContainer
{
    public MachineName machineName;
    public MachineControlPanel[] machineControlPanels;

    public TMP_Text countText;

    public override void Init()
    {
        objName = machineName.ToString();
        if(machineControlPanels == null || machineControlPanels.Length <=0)
            machineControlPanels = GetComponentsInChildren<MachineControlPanel>();

    }

    public override void UpdateControl()
    {
        List<Machine> machines = MachineManager.Instance.GetMachines(machineName);
        countText.text = string.Format("{0}EA 가동중", machines.Count);
        for (int i = 0; i < machineControlPanels.Length; i++)
        {
            if (i < machines.Count)
            {
                machineControlPanels[i].SetMachine(machines[i]);
                machineControlPanels[i].gameObject.SetActive(true);
            }
            else
            {
                machineControlPanels[i].gameObject.SetActive(false);
            }

        }
    }

    public override void UpdateContainer()
    {
        for(int i =0;i< machineControlPanels.Length; i++)
        {
            machineControlPanels[i].UpdatePanel();
        }
    }

    

}
