using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimulationPlayButton : MonoBehaviour
{
    public TMP_Text stateText;
    public void Awake()
    {
        stateText = GetComponentInChildren<TMP_Text>();
        GetComponent<Button>().onClick.AddListener(OnClickedPlay);    
    }

    public void OnClickedPlay()
    {
        if (SimulationManager.Instance.simulating)
        {
            SimulationManager.Instance.StartSimulation();
        }
        else
        {
            SimulationManager.Instance.StopSimulation();
        }
    }
}
