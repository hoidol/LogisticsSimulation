using UnityEngine;
using UnityEngine.UI;
public class MachineDetailCloseButton : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickedButton);
    }

    public void OnClickedButton()
    {
        PlayMode.Instanace.ClosedDetail();
        EditMode.Instanace.ClosedDetail();
        MachineDetailCanvas.Instance.gameObject.SetActive(false);
    }
    
}
