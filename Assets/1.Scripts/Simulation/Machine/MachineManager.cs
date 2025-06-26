using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineManager : MonoBehaviour, ISimulation
{
    public Dictionary<MachineName, List<Machine>> machineDic = new Dictionary<MachineName, List<Machine>>();
    public List<Machine> allMachines = new List<Machine>();
    public static MachineManager Instance;

    
    public void Awake()
    {
        Instance = this;
        for(int i =0;i< (int)MachineName.Count; i++)
        {
            machineDic.Add((MachineName)i, new List<Machine>());
        }
    }

    public void StartMode(SimulationModeType type)
    {
        if(type == SimulationModeType.Play)
        {
            foreach(var machineList in machineDic)
            {
                machineList.Value.ForEach(m => m.PlaySimulation());
            }
        }
        else
        {
            foreach (var machineList in machineDic)
            {
                machineList.Value.ForEach(m => m.StopSimulation());
            }
        }
    }

    public void AddMachine(Machine machine)
    {
        if (machineDic[machine.machineName].Contains(machine))
            return;

        machineDic[machine.machineName].Add(machine);
        allMachines.Add(machine);

    }

    public List<Machine> GetMachines(MachineName name)
    {
        return machineDic[name];
    }

}
