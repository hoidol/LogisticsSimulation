using System.Collections.Generic;
using UnityEngine;

public abstract class Machine : SimulationObject
{
    public MachineName machineName;  // 장비 구분자
    public string id;

    [Header("수동으로 지정")]
    public LinkPoint[] linkPoints;
    public List<Box> boxes = new List<Box>();

    public virtual void Init(string id)
    {
        this.id = id;
        for(int i =0;i< linkPoints.Length; i++)
        {
            linkPoints[i].Init();
        }
    }

    public virtual void EditMachine()
    {
        CheckLink();
    }

    public virtual void ViewDetail(bool on)
    {

    }


    public virtual void PlaySimulation()
    {
        CheckLink();
    }

    public virtual void CheckLink()
    {
        for (int i = 0; i < linkPoints.Length; i++)
        {
            linkPoints[i].Link();
        }
    }
    public virtual void StopSimulation()
    {

    }

    public virtual void Load(Box box, Vector3 pos)
    {
    }

    // 해당 위치에 박스를 올릴 수 있는지 확인
    public virtual bool CanLoad(Vector3 pos)
    {
        return true;
    }
}
public enum MachineName
{
    Conveyor, //C_i
    AGV, //AGV_i
    ASRSLooped, //ASRS_i
    Workbay, //Workbay - W_i
    RobotControl, //RC_i
    Count
}