using UnityEngine;
using UnityEngine.UI;
public class DeleteMachine : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickedDelete);
    }

    public void OnClickedDelete()
    {
        EditMode.Instanace.DeleteMachine();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
