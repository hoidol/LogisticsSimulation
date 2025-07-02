using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//박스 싣고 다니는 자동차
public class AGV : Machine
{
    public Transform cameraTr;
    
    public Transform topPoint;//

    public float moveSpeed;

    public override void PlaySimulation()
    {
        base.PlaySimulation();
        StartCoroutine(CoDestination());
        StartCoroutine(CoCheckArrival());
    }
    public override void StopSimulation()
    {
        base.StopSimulation();
        StopAllCoroutines();
    }
    public override void ViewDetail(bool on)
    {
        cameraTr.gameObject.SetActive(on);
    }
    

    IEnumerator CoDestination()
    {
        WaitForSeconds wait = new WaitForSeconds(3f);

        while (true)
        {
            yield return wait;

            if (loadedBox != null) //가지고 있는게 있을때는 
            {

                if (nearestOutPoint != null && nearestOutPoint.CanLoad())
                    continue;

                FindOutPoint();
            }
            else
            {
                if (nearestPickUpPoint != null && nearestPickUpPoint.CanStandby())
                    continue;

                FindPickUpPoint(); //비어있는지 확인해야돼 
            }
        }
    }

    [SerializeField] Box loadedBox;

    [SerializeField] OutPoint nearestOutPoint = null;
    [SerializeField] AGVPickUpPoint nearestPickUpPoint;
    [SerializeField] bool moving;
    [SerializeField] bool standbying;


    public override void Load(Box box, Vector3 pos)
    {
        base.Load(box, pos);
        loadedBox = box;
        box.transform.position = topPoint.position;
        standbying = false;
        moving = false;
        // 가장 가까운 적재 가능한 OutPoint 찾기
        FindOutPoint();
    }


    public override void Unload(Box box, Machine at)
    {
        loadedBox = null;
        at.Load(box, Vector3.zero);
        FindPickUpPoint();
    }

    void FindOutPoint()
    {
        List<Machine> outPoints = MachineManager.Instance.GetMachines(MachineName.OutPoint);
        nearestOutPoint = null;
        float minDist = float.MaxValue;

        foreach (var m in outPoints)
        {
            if (!m.CanLoad()) continue;

            float dist = Vector3.Distance(transform.position, m.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestOutPoint = m as OutPoint; ;
            }
        }

        if (nearestOutPoint != null)
            moving = true;
    }


    void FindPickUpPoint()
    {
        List<Machine> pickUpPoints = MachineManager.Instance.GetMachines(MachineName.AGVPickUpPoint);
        nearestPickUpPoint = null;
        float minDist = float.MaxValue;
        
        foreach (var m in pickUpPoints)
        {
            float dist = Vector3.Distance(transform.position, m.transform.position);
            if (dist < minDist)
            {
                AGVPickUpPoint p = m as AGVPickUpPoint;
                if (!p.CanStandby()) continue;

                minDist = dist;
                nearestPickUpPoint = p; 
            }
        }

        if (nearestPickUpPoint != null)
            moving = true;
    }

    void UpdateLoaded()
    {
        if (nearestOutPoint == null)
            return;

        if (!moving)
            return;

        // 대상 위치
        Vector3 targetPos = nearestOutPoint.transform.position;

        // 현재 위치와 방향
        Vector3 currentPos = transform.position;
        Vector3 direction = (targetPos - currentPos).normalized;

        // 회전 보정 (Y축만)
        Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);

        // 이동
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void UpdateUnloaded()
    {
        if (!moving)
            return;
        if (standbying)
            return;
        if (nearestPickUpPoint == null)
            return;
        // 대상 위치
        Vector3 targetPos = nearestPickUpPoint.transform.position;

        // 현재 위치와 방향
        Vector3 currentPos = transform.position;
        Vector3 direction = (targetPos - currentPos).normalized;

        // 회전 보정 (Y축만)
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);            
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    private void Update()
    {
        if(loadedBox != null)
        {
            UpdateLoaded();
        }
        else
        {
            UpdateUnloaded();
            
        }

       
    }


    IEnumerator CoCheckArrival()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        while (true)
        {
            yield return wait;

            
            if (loadedBox != null && nearestOutPoint != null)
            {
                float distance = Vector3.Distance(transform.position, nearestOutPoint.transform.position);
                if (distance < 0.5f)
                {
                    if (nearestOutPoint.CanLoad())
                    {
                        moving = false;
                        Unload(loadedBox, nearestOutPoint);
                    }
                    else
                    {
                        FindOutPoint(); // 공간 없을 경우 재탐색
                    }
                }
            }
            else if (loadedBox == null && nearestPickUpPoint != null)
            {
                float distance = Vector3.Distance(transform.position, nearestPickUpPoint.transform.position);
                if (distance < 0.5f)
                {
                    if (nearestPickUpPoint.CanStandby())
                    {
                        moving = false;
                        standbying = true;
                        nearestPickUpPoint.Standby(this);
                    }
                    else
                    {
                        FindPickUpPoint(); // 대기 공간 없을 경우 재탐색
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Machine"))
        {
            Machine machine = other.GetComponent<Machine>();
            if(machine.machineName== MachineName.OutPoint && nearestOutPoint == machine)
            {
                Debug.Log("AGV OutPoint 도착 ");
                moving = false;
                if (!nearestOutPoint.CanLoad())
                {
                    FindOutPoint();
                    return;
                }
                Debug.Log("AGV OutPoint Unload ");
                Unload(loadedBox, nearestOutPoint);
            }
            else if (machine.machineName == MachineName.AGVPickUpPoint && nearestPickUpPoint == machine)
            {

                moving = false;
                if (!nearestPickUpPoint.CanStandby())
                {
                    FindPickUpPoint();
                    return;
                }
                standbying = true;
                nearestPickUpPoint.Standby(this);
            }
            
            
        }
    }

    public override bool CanLoad()
    {
        return loadedBox == null;
    }

}
