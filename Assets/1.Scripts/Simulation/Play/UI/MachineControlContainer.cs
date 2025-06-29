using System.Collections.Generic;
using UnityEngine;

public class MachineControlContainer : MonoBehaviour
{
    public MachineName machineName;
    public MachineControlPanel[] machineControlPanels;

    public void Init()
    {
        if(machineControlPanels == null || machineControlPanels.Length <=0)
            machineControlPanels = GetComponentsInChildren<MachineControlPanel>();

    }

    public void UpdateControl()
    {
        List<Machine> machines = MachineManager.Instance.GetMachines(machineName);
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
    public void SetMachineControl(MachineName key)
    {
        gameObject.SetActive(machineName == key);
        if(machineName == key)
        {
            UpdateContainer();
        }
    }

    public void UpdateContainer()
    {
        for(int i =0;i< machineControlPanels.Length; i++)
        {
            machineControlPanels[i].UpdatePanel();
        }
    }

    

}
