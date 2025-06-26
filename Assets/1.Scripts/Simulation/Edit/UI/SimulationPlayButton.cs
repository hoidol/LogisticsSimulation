using UnityEngine;

using UnityEngine.UI;

public class SimulationPlayButton : MonoBehaviour
{
    
    public void Awake()
    {
    
        GetComponent<Button>().onClick.AddListener(OnClickedPlay);    
    }

    public void OnClickedPlay()
    {
        SimulationManager.Instance.SetMode(SimulationModeType.Play);
        
    }
}
