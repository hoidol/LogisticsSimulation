using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class AddMachinePanel : MonoBehaviour
{
    public MachineName machineName;
    
    public void Awake()
    {
        GetComponentInChildren<Button>().onClick.AddListener(OnClickedButton);
    }


    public void UpdatePanel()
    {
        if(EditMode.Instanace.targetMachine.machineName == machineName)
        {

        }
    }

    public void OnClickedButton()
    {
        EditMode.Instanace.TryToAddMachine(machineName);
    }
}
