using UnityEngine;

public class PlayMode : SimulationMode
{
    public static PlayMode Instanace;
    public PlayModePanel playModePanel;
    public PlayModeSettingCanvas playModeSettingCanvas;

    public Machine targetMachine;
    public Box targetBox;

    public PlayModeType playModeType;
    private void Awake()
    {
        Instanace = this;

    }

    public override void StartMode()
    {
        playModeSettingCanvas.gameObject.SetActive(false);
        playModePanel.StartPlayMode();
        SetPlayMode(PlayModeType.View);
        
    }

    public override void EndMode()
    {
        playModeSettingCanvas.gameObject.SetActive(false);
        playModePanel.EndPlayMode();
    }

    public void SetPlayMode(PlayModeType type)
    {
        playModeType = type;
        UpdateMode();
    }

    void UpdateMode()
    {
        playModePanel.UpdatePanel();
    }

    private void Update()
    {
        if (SimulationManager.Instance.simulationModeType != SimulationModeType.Play)
            return;

        if (Input.GetMouseButton(1))
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playModeSettingCanvas.gameObject.SetActive(!playModeSettingCanvas.gameObject.activeSelf);
        }

        if(playModeType == PlayModeType.View)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hit, 50, LayerMask.GetMask("Machine")))
                {
                    targetMachine = hit.collider.GetComponent<Machine>();
                    SetPlayMode(PlayModeType.Detail);
                    MachineDetailCanvas.Instance.ShowDetail(targetMachine);
                    return;
                }else if (!Physics.Raycast(ray, out hit, 50, LayerMask.GetMask("Box")))
                {
                    targetBox = hit.collider.GetComponent<Box>();
                    SetPlayMode(PlayModeType.Detail);
                    BoxDetailCanvas.Instance.ShowDetail(targetBox);
                    return;
                }
            }
        }
        else
        {
            
        }
    }

    public void ClosedDetail()
    {
        if (SimulationManager.Instance.simulationModeType != SimulationModeType.Play)
            return;
        targetBox = null;
        targetMachine = null;
        SetPlayMode(PlayModeType.View);
    }

}

public enum PlayModeType
{
    View,
    Detail, // 특정 Machine or Box 클릭
}