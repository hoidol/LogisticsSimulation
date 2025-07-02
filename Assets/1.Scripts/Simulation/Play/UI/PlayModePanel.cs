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
        controlContainers = GetComponentsInChildren<MachineControlContainer>(true);
        menuButtons = GetComponentsInChildren<MachineControlMenuButton>(true);

        for (int i =0;i< controlContainers.Length; i++)
        {
            controlContainers[i].Init();
        }
        
        init = true;
    }
    
    public void SetMachineControl(MachineName name)
    {
        //Debug.Log($"PlayModePanel SetMachineControl {name}");
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
        Init();

        gameObject.SetActive(true);
        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].UpdateButton();
               controlContainers[i].UpdateControl();
        }
        SetMachineControl(MachineName.ASRSLooped);
    }

    public void EndPlayMode()
    {
        gameObject.SetActive(false);
    }
}
