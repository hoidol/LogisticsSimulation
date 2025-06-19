using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager Instance;
    public HandWorkSpace[] handWorkSpaces;
    public Conveyor[] conveyors;
    public ASRSLooped[] asrsLoopeds;
    public HandMachine[] handMachines;
    ISimulationMachine[] simulationMachines;

    private void Awake()
    {
        Instance = this;
    }
    public bool simulating
    {
        get;
        set;
    }
    private void Start()
    {
        simulating = false;
    }

    public void StartSimulation()
    {
        simulationMachines = GetComponents<ISimulationMachine>();
        simulating = true;
    }

    public void StopSimulation()
    {
        simulating = false;
    }
}
