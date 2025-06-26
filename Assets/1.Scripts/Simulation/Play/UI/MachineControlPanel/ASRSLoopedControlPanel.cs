using UnityEngine;

public class ASRSLoopedControlPanel : MachineControlPanel
{
    public float speed;
    ASRSLooped asrsLooped;
    public override void SetMachine(Machine machine)
    {
        asrsLooped = machine as ASRSLooped;
        
    }
}
