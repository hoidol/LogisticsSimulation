using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineManager : MonoBehaviour, ISimulation
{
    public Dictionary<MachineName, List<Machine>> machineDic = new Dictionary<MachineName, List<Machine>>();
    public List<Machine> allMachines = new List<Machine>();
    public static MachineManager Instance;

    public MachineInfo[] machineInfos;
    
    MachineDB machineDB;
    public void Awake()
    {
        Instance = this;
        for(int i =0;i< (int)MachineName.Count; i++)
        {
            machineDic.Add((MachineName)i, new List<Machine>());
        }

        machineInfos = new MachineInfo[(int)MachineName.Count];
        machineInfos[0] = new MachineInfo() { machineName = MachineName.Conveyor, prefix = "C_" };
        machineInfos[1] = new MachineInfo() { machineName = MachineName.AGV, prefix = "AGV_" };
        machineInfos[2] = new MachineInfo() { machineName = MachineName.ASRSLooped, prefix = "ASRS_" };
        machineInfos[3] = new MachineInfo() { machineName = MachineName.Workbay, prefix = "W_" };
        machineInfos[4] = new MachineInfo() { machineName = MachineName.RobotControl, prefix = "RC_" };
        machineInfos[5] = new MachineInfo() { machineName = MachineName.OutPoint, prefix = "OP_" };
        machineInfos[6] = new MachineInfo() { machineName = MachineName.AGVPickUpPoint, prefix = "AGVPPoint_" };

        machineDB = SaveManager.LoadData<MachineDB>("MachineDB");
        if (machineDB == null)
        {
            Debug.Log("신규 플레이");
            machineDB = new MachineDB();
        }
    }

    MachineInfo GetMachineInfo(MachineName machineName)
    {
        foreach(var d in machineInfos)
        {
            if (d.machineName == machineName)
                return d;
        }
        return null;
    }

    void Start()
    {
        for(int i =0;i< machineDB.machineSaveDatas.Count; i++)
        {
            Machine machine = Instantiate(machineDB.machineSaveDatas[i].machineName);
            machine.transform.position = machineDB.machineSaveDatas[i].position;
            machine.transform.rotation = Quaternion.Euler( machineDB.machineSaveDatas[i].rotation);
            machine.Init(machineDB.machineSaveDatas[i]);
            AddMachine(machine);
        }
        Invoke("CheckLink", 0.2f);
    }

    void CheckLink()
    {
        allMachines.ForEach(e => e.CheckLink());
    }
    public void StartMode(SimulationModeType type)
    {
        if(type == SimulationModeType.Play)
        {
            foreach(var machineList in machineDic)
            {
                machineList.Value.ForEach(m => m.CheckLink());
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

    public Machine Instantiate(MachineName machineName)
    {
        Machine machine =  Instantiate(Resources.Load<Machine>(machineName.ToString()));

        return machine;


    }

    public void AddMachine(Machine machine)
    {
        if (machineDic[machine.machineName].Contains(machine))
            return;

        machineDic[machine.machineName].Add(machine);
        allMachines.Add(machine);

        List<Machine> machines = GetMachines(machine.machineName);

        MachineInfo info = GetMachineInfo(machine.machineName);

        if(string.IsNullOrEmpty(machine.id))
        {
            string id;
            HashSet<string> existingIds = new HashSet<string>();
            foreach (var m in machines)
            {
                existingIds.Add(m.id); // Machine 클래스에 id 필드가 있다고 가정
            }

            do
            {
                id = info.prefix + Random.Range(0, 10000).ToString("D4"); // 0000 ~ 9999 형식
            } while (existingIds.Contains(id));

            MachineSaveData saveData = new MachineSaveData();
            machineDB.machineSaveDatas.Add(saveData);
            saveData.id = id;
            machine.Init(saveData);
        }
        
    }

    public List<Machine> GetMachines(MachineName name)
    {
        return machineDic[name];
    }

    public void Remove(Machine machine)
    {
        machineDB.machineSaveDatas.Remove(machine.machineSaveData);
        machineDic[machine.machineName].Remove(machine);
        allMachines.Remove(machine);
        Destroy(machine.gameObject);
        Save();
    }
    
    public void Save()
    {
        
        machineDB.machineSaveDatas.Clear();
        for (int i = 0; i < allMachines.Count; i++)
        { 
            machineDB.machineSaveDatas.Add(allMachines[i].machineSaveData);

            allMachines[i].machineSaveData.machineName = allMachines[i].machineName;
            allMachines[i].machineSaveData.position = allMachines[i].transform.position;
            allMachines[i].machineSaveData.rotation = allMachines[i].transform.rotation.eulerAngles;
            allMachines[i].machineSaveData.destinationId = allMachines[i].destinationId;
 
        }

        SaveManager.SaveData("MachineDB", machineDB);
    }

}

public class MachineDB
{
    public List<MachineSaveData> machineSaveDatas = new List<MachineSaveData>();
}

[System.Serializable]
public class MachineSaveData
{
    public string id;
    public MachineName machineName;
    public Vector3 position;
    public Vector3 rotation;

    public string destinationId;
    
}

public class MachineInfo
{
    public MachineName machineName;
    public string prefix;
}