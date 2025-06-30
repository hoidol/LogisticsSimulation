using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Machine
{
    public LinkPoint startLinkPoint; //
    public LinkPoint endLinkPoint;
    public float moveSpeed = 0.2f;

    //수정 모드에서 설정됨
    //public List<Conveyor> conveyorsToAsrs; // asrs로 향하는 연결된 컨베이어 
    public Machine preMachine; //이전 연결점 
    public Machine nextMachine; // 

    public List<LinkPoint> sidePoints = new List<LinkPoint>(); //옆 부분
    public Vector3 direction;
    public override void Init(string id)
    {
        base.Init(id);
        EditMachine();
    }

    public override void EditMachine()
    {
        direction = transform.rotation.eulerAngles;
        base.EditMachine();
        // transform.right;// (endLinkPoint.transform.position - startLinkPoint.transform.position).normalized;
    }


    public override void CheckLink()
    {
        base.CheckLink();
        Debug.Log($"Conveyor CheckLink () {id}");
        LinkPoint nextPoint = endLinkPoint.Link();

        if (nextPoint == null)
        {
            Collider[] cols = Physics.OverlapBox(endLinkPoint.transform.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, LayerMask.GetMask("Machine"));
            for (int i = 0; i < cols.Length; i++)
            {
                Machine machine = cols[i].GetComponent<Machine>();
                if (machine == this)
                    continue;

                //endLinkPoint. = nextMachine;
                nextMachine = machine;
                break;
            }
        }

        BoxCollider box = GetComponent<BoxCollider>();

        sidePoints.Clear(); // 기존 데이터 초기화
        Collider[] colliders = Physics.OverlapBox(
            box.bounds.center,
            box.bounds.extents,
            transform.rotation,
            LayerMask.GetMask("LinkPoint")
        );

        foreach (var col in colliders)
        {
            LinkPoint link = col.GetComponent<LinkPoint>();
            if (link == null) continue;

            // 시작/끝/앞/뒤 포인트는 제외
            if (link == startLinkPoint || link == endLinkPoint)
                continue;

            Conveyor conveyor = link.GetComponentInParent<Conveyor>(); 
            if(conveyor != null)
            {
                if (conveyor.startLinkPoint == link)
                {
                    if (!sidePoints.Contains(link))
                        sidePoints.Add(link);
                }
            }
            
        }
    }

    // 박스를 pos 위치에 올리고 리스트에 등록
    public override void Load(Box box, Vector3 pos)
    {
        if (pos != Vector3.zero)
            box.transform.position = pos;
        else
            box.transform.position = startLinkPoint.transform.position;

        box.transform.parent = transform;
        box.transform.forward = direction;
        boxes.Add(box);
    }

    // 해당 위치에 박스를 올릴 수 있는지 확인
    public override bool CanLoad(Vector3 pos)
    {
        foreach (var box in boxes)
        {
            float distance = Vector3.Distance(pos, box.transform.position);
            float minDistance = box.width + 0.01f;
            if (distance < minDistance)
                return false;
        }
        return true;
    }

    private void Update()
    {
        if (boxes.Count <= 0)
            return;

        

        for (int i = boxes.Count - 1; i >= 0; i--)
        {
            Box box = boxes[i];
            Vector3 current = box.transform.position;
            Vector3 target = endLinkPoint.transform.position;

            // 이동
            Vector3 nextPos = current + direction * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(current, target) > 0.01f)
            {
                box.transform.position = nextPos;
            }
            else
            {

                // ✅ 도착 지점 도달 → 다음 컨베이어로 이동 시도
                if (nextMachine != null)
                {
                    //Vector3 nextPosOnNextConveyor = nextPoint.machine.startTr.transform.position;
                    //if (nextPoint.machine.CanLoad(nextPosOnNextConveyor))
                    //{ 
                    //}
                    boxes.RemoveAt(i);
                    nextMachine.Load(box, box.transform.position);
                    i--;
                    // 공간이 없으면 계속 대기
                }
            }
        }
    }

    #region 수정 모드일때 호출되는 함수
    public void Link() //
    {

    }
    #endregion
}
