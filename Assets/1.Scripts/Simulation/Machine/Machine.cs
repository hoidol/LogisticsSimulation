using System.Collections.Generic;
using UnityEngine;

public abstract class Machine : SimulationObject
{
    public MachineName machineName;  // 장비 구분자
    public string id;
    public string destinationId;
    public void SetDestination(string id)
    {
        destinationId = id;
    }
    [Header("수동으로 지정")]
    public LinkPoint[] linkPoints;

    [Header("모든 연결점 - 자동 지정")]
    public List<LinkPoint> linkedPoints; 
    public List<Box> boxes = new List<Box>();

     LinkIcon[] linkIcons;
    public Machine nextMachine;
    public MachineSaveData machineSaveData;
    public virtual void Init( MachineSaveData machineSaveData)
    {
        this.machineSaveData = machineSaveData;
        this.id = machineSaveData.id;
        this.destinationId = machineSaveData.destinationId;
        linkIcons = GetComponentsInChildren<LinkIcon>(true);
        for(int i =0;i< linkIcons.Length; i++)
        {
            linkIcons[i].gameObject.SetActive(false);
        }
        for (int i =0;i< linkPoints.Length; i++)
        {
            linkPoints[i].Init();
        }

        CheckLink();
    }

    public void Drag(bool on)
    {
        for(int i =0;i< linkIcons.Length; i++)
        {
            linkIcons[i].gameObject.SetActive(on);
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
        //CheckLink();
    }

    public virtual void CheckLink()
    {
        //Debug.Log($"machine CheckLink id  { id}");
        linkedPoints.Clear();
        linkedPoints.AddRange(linkPoints);
        for (int i = 0; i < linkPoints.Length; i++)
        {
            linkPoints[i].Link();
        }

        if(linkPoints.Length > 0)
        {
            Collider[] cols = Physics.OverlapSphere(linkPoints[linkPoints.Length - 1].transform.position, 0.3f, LayerMask.GetMask("LinkPoint"));
            if (cols.Length <= 0)
                return;
            for (int i = 0; i < cols.Length; i++)
            {
                LinkPoint linkPoint = cols[0].GetComponent<LinkPoint>();
                if (linkPoint.Machine == this)
                    continue;
                if (linkPoint.transferDirection == TransferDirection.Out)
                    continue;

                nextMachine = linkPoint.Machine;
                break;
            }
        }
    }
    public virtual void StopSimulation()
    {

    }

    public virtual void Load(Box box, Vector3 pos)
    {
        box.Loaded(this);
    }

    public virtual void Unload(Box box, Machine at)
    {

    } 
    // 해당 위치에 박스를 올릴 수 있는지 확인
    public virtual bool CanLoad()
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
    OutPoint, //OP_i
    AGVPickUpPoint,
    Count
}