using UnityEngine;

public class PlayModePanel : MonoBehaviour
{
    [SerializeField] MachineControlContainer[] controlContainers;
    [SerializeField] MachineControlMenuButton[] menuButtons;


    bool init;
    public void Init()
    {
        if (init)
            return;
        init = true;
    }
    
    public void SetMachineControl(MachineName name)
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].SetMachineControl(name);
            controlContainers[i].SetMachineControl(name);
        }
    }

    public void UpdatePanel()
    {
        Init();    
    }

    public void StartPlayMode()
    {
        gameObject.SetActive(true);
        for (int i = 0; i < menuButtons.Length; i++)
        {
            controlContainers[i].Init();
        }
        SetMachineControl(MachineName.ASRSLooped);
    }

    public void EndPlayMode()
    {
        gameObject.SetActive(false);
    }
}
