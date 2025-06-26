using UnityEngine;
using UnityEngine.UI;
public class BoxDetailCloseButton : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickedButton);
    }

    public void OnClickedButton()
    {
        PlayMode.Instanace.ClosedDetail();
        EditMode.Instanace.ClosedDetail();
        BoxDetailCanvas.Instance.gameObject.SetActive(false);

    }

}
