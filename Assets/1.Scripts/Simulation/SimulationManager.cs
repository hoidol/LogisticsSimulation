using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public SimulationModeType simulationModeType;
    public static SimulationManager Instance;
    public SimulationMode[] simulationModes;

    //public List<Machine> machines = new List<Machine>(); //현재 설치된 기기

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        simulationModes = GetComponentsInChildren<SimulationMode>();
    }
    public SimulationMode GetSimulationMode(SimulationModeType type)
    {
        for(int i =0;i< simulationModes.Length; i++)
        {
            if (simulationModes[i].type == type)
                return simulationModes[i];
        }
        return null;
    }
    private void Start()
    {
        SetMode(SimulationModeType.Edit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            Time.timeScale += 1;
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            Time.timeScale -= 1;
        }
    }

    public void SetMode(SimulationModeType type)
    {
        simulationModeType = type;
        if (type == SimulationModeType.Play)
        {
            StartSimulation();
        }
        else
        {
            StopSimulation();
        }

        BoxDetailCanvas.Instance.gameObject.SetActive(false);
        MachineDetailCanvas.Instance.gameObject.SetActive(false);
    }

    public void StartSimulation()
    {
        GetSimulationMode(SimulationModeType.Edit).EndMode();
        GetSimulationMode(SimulationModeType.Play).StartMode();
        MachineManager.Instance.StartMode(SimulationModeType.Play);
        ProductManager.Instance.StartMode(SimulationModeType.Play);
    }

    public void StopSimulation()
    {
        GetSimulationMode(SimulationModeType.Play).EndMode();
        GetSimulationMode(SimulationModeType.Edit).StartMode();
        MachineManager.Instance.StartMode(SimulationModeType.Edit);
        ProductManager.Instance.StartMode(SimulationModeType.Edit);

    }

}


public enum SimulationModeType
{
    Edit,
    Play
} 