using UnityEngine;
using System.Collections;

using UnityEngine.UI;
public class SimulationStopButton : MonoBehaviour
{
    public void Awake()
    {

        GetComponent<Button>().onClick.AddListener(OnClickedPlay);
    }

    public void OnClickedPlay()
    {
        SimulationManager.Instance.SetMode(SimulationModeType.Edit);
        PlayMode.Instanace.ClosedDetail();

    }
}
