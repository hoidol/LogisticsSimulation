using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class AddMachinePanel : MonoBehaviour
{
    public MachineName machineName;
    public TMP_Text countText;
    
    public void Awake()
    {
        GetComponentInChildren<Button>().onClick.AddListener(OnClickedButton);
    }

    private void Start()
    {
        UpdatePanel();
    }

    public void UpdatePanel()
    {

        //if(EditMode.Instanace.targetMachine.machineName == machineName)
        //{
            
        //}

        countText.text = MachineManager.Instance.GetMachines(machineName).Count.ToString();
    }

    public void OnClickedButton()
    {
        EditMode.Instanace.TryToAddMachine(machineName);
    }
}
