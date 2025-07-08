using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class BoxControlPanel : MonoBehaviour
{
    public TMP_Text productIdText;
    public TMP_Text machineIdText;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickedButton);
    }

    public Box box;
    public void SetBox(Box box)
    {
        this.box = box;

        UpdatePanel();
    }

    public void UpdatePanel()
    {
        if (box == null)
        {
            gameObject.SetActive(false);
            return;
        }
        productIdText.text = box.productData.id;
        
        if (box.machine != null)
        {
            machineIdText.text = box.machine.id;
        }
        else
        {
            machineIdText.text = "대기중";

        }
    }
    public void OnClickedButton()
    {
        if (box == null || !box.gameObject.activeSelf)
        {
            //UpdatePanel();
            return;
        }
            
        PlayMode.Instanace.ShowDetail(box);
    }
}
