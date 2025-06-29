using UnityEngine;
using UnityEngine.EventSystems;

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

        if (EventSystem.current.IsPointerOverGameObject())
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
                if (Physics.Raycast(ray, out RaycastHit hit, 50, LayerMask.GetMask("Machine")))
                {
                    targetMachine = hit.collider.GetComponent<Machine>();
                    ShowDetail(targetMachine);
                    playModePanel.SetMachineControl(targetMachine.machineName);
                    return;
                }else if (Physics.Raycast(ray, out hit, 50, LayerMask.GetMask("Box")))
                {
                    targetBox = hit.collider.GetComponent<Box>();
                    ShowDetail(targetBox);
                    return;
                }
            }
        }
    }

    public void ShowDetail(SimulationObject obj)
    {
        if (targetMachine != null)
            targetMachine.ViewDetail(false);

        targetMachine = obj as Machine;
        if (obj.simulationObjectType == SimulationObjectType.Machine)
        {
            targetMachine.ViewDetail(true);
            MachineDetailCanvas.Instance.ShowDetail(targetMachine);
        }
        else
        {
            BoxDetailCanvas.Instance.ShowDetail(targetBox);
        }
        SetPlayMode(PlayModeType.Detail);
    }
    

    public void ClosedDetail()
    {
        if (SimulationManager.Instance.simulationModeType != SimulationModeType.Play)
            return;

        if(targetMachine != null)
            targetMachine.ViewDetail(false);

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