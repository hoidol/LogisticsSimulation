using UnityEngine;

public class AdjustDetailContainer : MonoBehaviour
{
    public AdjustDetailPanel[] adjustDetailPanels;
    public void SetMachine(Machine machine)
    {
        //Debug.Log($"AdjustDetailContainer SetMachine() {machine != null}");
        for (int i = 0; i < adjustDetailPanels.Length; i++)
        {
            adjustDetailPanels[i].gameObject.SetActive(false);
        }
        if(machine != null)
        {
            AdjustDetailPanel panel = GetAdjustDetailPanel(machine.machineName);
            if (panel != null)
            {
                panel.gameObject.SetActive(true);
                panel.SetMachine(machine);
            }
        }
        
        

    }
    AdjustDetailPanel GetAdjustDetailPanel(MachineName machineName)
    {
        for (int i = 0; i < adjustDetailPanels.Length; i++)
        {
            if (adjustDetailPanels[i].machineName == machineName)
            {
                return adjustDetailPanels[i];
            }
        }
        return null;
    }
}
