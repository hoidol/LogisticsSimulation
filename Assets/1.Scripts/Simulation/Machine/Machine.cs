using System.Collections.Generic;
using UnityEngine;

public abstract class Machine : MonoBehaviour
{
    public MachineName machineName;  // 장비 구분자
    [Header("수동으로 지정")]
    public LinkPoint[] linkPoints;

    public List<Box> boxes = new List<Box>();

    public virtual void PlaySimulation()
    {
        for(int i =0;i< linkPoints.Length; i++)
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
    Conveyor,
    AGV,
    ASRSLooped,
    Workbay,
    RobotControl,
    Count
}