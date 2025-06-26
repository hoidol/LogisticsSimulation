using UnityEngine;
using UnityEngine.UI;
public class MachineControlMenuButton : MonoBehaviour
{
    public MachineName machineName;
    public GameObject barImage;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickedButton);
    }

    public void SetMachineControl(MachineName name)
    {
        barImage.gameObject.SetActive(machineName == name);
    }

    public void OnClickedButton()
    {
        GetComponentInParent<PlayModePanel>().SetMachineControl(machineName);
    }
}
