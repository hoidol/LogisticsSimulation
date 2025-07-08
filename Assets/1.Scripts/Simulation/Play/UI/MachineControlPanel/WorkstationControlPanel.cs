using UnityEngine;
using TMPro;

public class WorkstationControlPanel : MachineControlPanel
{
    Workstation workstation;
    public TMP_Text speedText;
    public override void SetMachine(Machine machine)
    {
        base.SetMachine(machine);
        workstation = machine as Workstation;
        speedText.text = workstation.processSpeed.ToString() +"s";
    }

    public void OnClickedUp()
    {
        workstation.processSpeed += 0.25f;
        speedText.text = workstation.processSpeed.ToString() + "s";
    }

    public void OnClickedDown()
    {
        workstation.processSpeed -= 0.25f;
        speedText.text = workstation.processSpeed.ToString() + "s";
    }

    public void OnClickedPanel()
    {
        PlayMode.Instanace.ShowDetail(workstation);
    }


}
