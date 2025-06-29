using UnityEngine;
using TMPro;

public class WorkBayControlPanel : MachineControlPanel
{
    Workbay workbay;
    public TMP_Text speedText;
    public override void SetMachine(Machine machine)
    {
        base.SetMachine(machine);
        workbay = machine as Workbay;
        speedText.text = workbay.processSpeed.ToString();
    }

    public void OnClickedUp()
    {
        workbay.processSpeed += 0.25f;
        speedText.text = workbay.processSpeed.ToString();
    }

    public void OnClickedDown()
    {
        workbay.processSpeed -= 0.25f;
        speedText.text = workbay.processSpeed.ToString();
    }

    public void OnClickedPanel()
    {
        PlayMode.Instanace.ShowDetail(workbay);
    }


}
