using UnityEngine;
using System.Collections;

public class EditModePanel : MonoBehaviour
{
    public GameObject addMachinePanel;
    public GameObject addingMachinePanel;
    public GameObject adjustMachinePanel;

    public AddMachinePanel[] addMachinePanels;


    private void Awake()
    {
        addMachinePanels = GetComponentsInChildren<AddMachinePanel>();
    }
    public void StartEditMode()
    {
        gameObject.SetActive(true);
    }

    public void EndEditMode()
    {
        gameObject.SetActive(false);
    }

    public void UpdatePanel()
    {
        if(EditMode.Instanace.editModeType == EditModeType.Add)
        {
            addMachinePanel.SetActive(true);
            addingMachinePanel.SetActive(false);
            adjustMachinePanel.SetActive(false);


            foreach (var panel in addMachinePanels)
            {
                panel.UpdatePanel();
            }
        }
        else if (EditMode.Instanace.editModeType == EditModeType.Adding)
        {
            addMachinePanel.SetActive( false);
            addingMachinePanel.SetActive(true);
            adjustMachinePanel.SetActive(false);
        }
        else if (EditMode.Instanace.editModeType == EditModeType.Adjust)
        {
            addMachinePanel.SetActive(false);
            addingMachinePanel.SetActive(false);
            adjustMachinePanel.SetActive(true);
        }

    }
}
