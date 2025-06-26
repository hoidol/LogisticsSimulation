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

    }

    public void StartSimulation()
    {
        GetSimulationMode(SimulationModeType.Edit).EndMode();
        GetSimulationMode(SimulationModeType.Play).StartMode();
        MachineManager.Instance.StartMode(SimulationModeType.Play);
    }

    public void StopSimulation()
    {
        GetSimulationMode(SimulationModeType.Play).EndMode();
        GetSimulationMode(SimulationModeType.Edit).StartMode();
        MachineManager.Instance.StartMode(SimulationModeType.Edit);
        
    }

}


public enum SimulationModeType
{
    Edit,
    Play
} 