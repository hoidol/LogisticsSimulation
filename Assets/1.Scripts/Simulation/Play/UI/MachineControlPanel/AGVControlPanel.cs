using UnityEngine;

public class AGVControlPanel : MachineControlPanel
{
    AGV agv;
    public override void SetMachine(Machine machine)
    {
        base.SetMachine(machine);
        agv = machine as AGV;
    }

    public void OnClickedPanel()
    {
        //AGV 보여지기
        PlayMode.Instanace.ShowDetail(agv);
    }
}
