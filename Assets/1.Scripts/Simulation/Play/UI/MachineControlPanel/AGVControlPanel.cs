using UnityEngine;

public class AGVControlPanel : MachineControlPanel
{
    AGV asrsLooped;
    public override void SetMachine(Machine machine)
    {
        asrsLooped = machine as AGV;
    }

    public void OnClickedPanel()
    {
        //AGV 보여지기
    }
}
