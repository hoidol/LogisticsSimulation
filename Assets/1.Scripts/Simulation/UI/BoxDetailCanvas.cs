using UnityEngine;

public class BoxDetailCanvas : MonoBehaviour
{
    private static BoxDetailCanvas instance;
    public static BoxDetailCanvas Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<BoxDetailCanvas>(FindObjectsInactive.Include);
            return instance;
        }
    }
    public void ShowDetail(Box box)
    {
        gameObject.SetActive(true);

        //MachineDetailPanel panel = GetMachineDetailPanel(machine.machineName);
        //panel.SetMachine(machine);
        //panel.gameObject.SetActive(true);
    }
}
