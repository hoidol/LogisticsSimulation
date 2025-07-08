using UnityEngine;

public class AGVPickUpPoint : Machine
{
    public AGV currentAGV;

    public override void StopSimulation()
    {
        base.StopSimulation();
        currentAGV = null;
    }

    public void Standby(AGV agv)
    {
        //Debug.Log($"AGVPickUpPoint Standby() agv.id {agv.id}");
        currentAGV = agv;
        agv.transform.position = transform.position;
        currentAGV.transform.forward = transform.forward;
    }

    public bool CanStandby()
    {
        return currentAGV == null;
    }

    public override bool CanLoad()
    {
        if (currentAGV == null || !currentAGV.CanLoad())
            return false;

        return true;
    }
    public override void Load(Box box, Vector3 pos)
    {

        if (currentAGV == null)
            return;

        currentAGV.Load(box, Vector3.zero);
        currentAGV = null;
    }
}
