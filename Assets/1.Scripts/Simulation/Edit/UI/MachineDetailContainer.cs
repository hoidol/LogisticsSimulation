using UnityEngine;

public class MachineDetailContainer : MonoBehaviour
{
    public MachineDetailPanel[] machineDetailPanels;


    public MachineDetailPanel GetMachineDetailPanel(MachineName name)
    {
        for(int i =0;i< machineDetailPanels.Length; i++)
        {
            if (machineDetailPanels[i].machineName == name)
                return machineDetailPanels[i];
        }
        return null;
    }
}
