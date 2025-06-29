using UnityEngine;
using TMPro;
public class ASRSLoopedControlPanel : MachineControlPanel
{
    //public float speed;
    ASRSLooped asrsLooped;
    public TMP_Text speedText;

    public override void SetMachine(Machine machine)
    {
        base.SetMachine(machine);
        asrsLooped = machine as ASRSLooped;
        speedText.text = asrsLooped.speed.ToString();
    }

    public void OnClickedUp()
    {
        asrsLooped.speed += 0.25f;
        speedText.text = asrsLooped.speed.ToString();
    }

    public void OnClickedDown()
    {
        asrsLooped.speed -= 0.25f;
        speedText.text = asrsLooped.speed.ToString();
    }

    public void OnClickedPanel()
    {
        PlayMode.Instanace.ShowDetail(asrsLooped);
    }
}
