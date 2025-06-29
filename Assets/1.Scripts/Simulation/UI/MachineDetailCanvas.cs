using UnityEngine;
using TMPro;
public class MachineDetailCanvas : MonoBehaviour
{

    private static MachineDetailCanvas instance;
    public static MachineDetailCanvas Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<MachineDetailCanvas>(FindObjectsInactive.Include);
            return instance;
        }
    }

    [SerializeField] TMP_Text machineNameText;
    [SerializeField] TMP_Text machineIdText;
    public MachineDetailPanel[] machineDetailPanels;
    public MachineDetailPanel GetMachineDetailPanel(MachineName name)
    {
        for (int i = 0; i < machineDetailPanels.Length; i++)
        {
            if (machineDetailPanels[i].machineName == name)
                return machineDetailPanels[i];
        }
        return null;
    }

    public void ShowDetail(Machine machine)
    {
        gameObject.SetActive(true);
        for (int i =0;i< machineDetailPanels.Length; i++)
        {
            machineDetailPanels[i].gameObject.SetActive(false);
        }
        machineNameText.text = machine.machineName.ToString();
        machineIdText.text = machine.id.ToString();
        MachineDetailPanel panel = GetMachineDetailPanel(machine.machineName);
        if (panel == null)
            return;

        panel.SetMachine(machine);
        panel.gameObject.SetActive(true);
    }
}
