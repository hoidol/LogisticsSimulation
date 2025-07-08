using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class ObjectControlMenuButton : MonoBehaviour
{
    public GameObject barImage;
    public string objectName;
    public SimulationObjectType objType;
    public virtual void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickedButton);
    }

    public virtual void SetControlButton(string name)
    {
        barImage.gameObject.SetActive(objectName == name);
    }

    public void UpdateButton()
    {

    }

    public void OnClickedButton()
    {
        GetComponentInParent<PlayModePanel>().SetControl(objectName);
    }
}
