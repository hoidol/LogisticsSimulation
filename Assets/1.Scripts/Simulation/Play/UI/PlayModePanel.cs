using UnityEngine;

public class PlayModePanel : MonoBehaviour
{
    [SerializeField] ObjectControlContainer[] controlContainers;
    [SerializeField] ObjectControlMenuButton[] menuButtons;


    bool init;
    public void Init()
    {
        if (init)
            return;
        controlContainers = GetComponentsInChildren<ObjectControlContainer>(true);
        menuButtons = GetComponentsInChildren<ObjectControlMenuButton>(true);

        for (int i =0;i< controlContainers.Length; i++)
        {
            controlContainers[i].Init();
        }
        
        init = true;
    }
    
    public void SetControl(string name)
    {
        bool have = false;
        for(int i =0;i< controlContainers.Length; i++)
        {
            if (controlContainers[i].objName == name)
                have = true;
        }
        if (have)
        {
            for (int i = 0; i < menuButtons.Length; i++)
            {
                menuButtons[i].SetControlButton(name);
                controlContainers[i].SetControlContainer(name);
            }
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
        SetControl(MachineName.ASRSLooped.ToString());
    }

    public void EndPlayMode()
    {
        gameObject.SetActive(false);
    }
}
