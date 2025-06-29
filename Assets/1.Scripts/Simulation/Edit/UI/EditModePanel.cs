using UnityEngine;
using System.Collections;
using TMPro;

public class EditModePanel : MonoBehaviour
{
    public GameObject addMachinePanel;
    public GameObject addingMachinePanel;
    public GameObject adjustMachinePanel;

    public AddMachinePanel[] addMachinePanels;
    [SerializeField] TMP_InputField inputField_id;

    private void Awake()
    {
        Debug.Log("EditModePanel");
        //addMachinePanels = GetComponentsInChildren<AddMachinePanel>();
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



            inputField_id.text = EditMode.Instanace.targetMachine.id;
        }

    }
    
    public void OnClicekdEditId()
    {
        if (string.IsNullOrEmpty(inputField_id.text))
            return;
        Debug.Log($"EditModePanel OnClicekdEditId() {inputField_id.text}");
        EditMode.Instanace.targetMachine.id = inputField_id.text;
        MachineManager.Instance.Save();
    }
}
