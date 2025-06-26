using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Machine
{
    public LinkPoint startLinkPoint; //
    public LinkPoint endLinkPoint;
    public float speed = 0.2f;

    //수정 모드에서 설정됨
    //public List<Conveyor> conveyorsToAsrs; // asrs로 향하는 연결된 컨베이어 

    public LinkPoint prePoint; //이전 연결점 
    public LinkPoint nextPoint; // 

    public List<LinkPoint> sidePoints = new List<LinkPoint>();; //옆 부분

    public override void PlaySimulation()
    {
        base.PlaySimulation();

        prePoint = startLinkPoint.Link();
        nextPoint = endLinkPoint.Link();

        //SideLink 확인하기
        //닿아있는 것 중
        CheckSideLink();
    }


    public void CheckSideLink()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        //box 콜리더에 닿아있는 LinkPoint 레이어를 가진 충돌채 찾아서
        //LinkPoint[] cols 배열 변수에 담고
        // startLinkPoint, endLinkPoint,prePoint,nextPoint 는 제외해서
        //sidePoints에 담아줘

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
            if (link == startLinkPoint || link == endLinkPoint || link == prePoint || link == nextPoint)
                continue;

            //옆으로 연결된 링크 포인트 중 시작 방향인 것만 sidePoints 담기
            //이유 : 그쪽으로는 흘려보내야되므로, 반대 방향은 들어오기만해서 괜찮음
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

        Vector3 direction = (endLinkPoint.transform.position - startLinkPoint.transform.position).normalized;

        for (int i = boxes.Count - 1; i >= 0; i--)
        {
            Box box = boxes[i];
            Vector3 current = box.transform.position;
            Vector3 target = endLinkPoint.transform.position;

            // 이동
            Vector3 nextPos = current + direction * speed * Time.deltaTime;

            if (Vector3.Distance(current, target) > 0.01f)
            {
                box.transform.position = nextPos;
            }
            else
            {
                // ✅ 도착 지점 도달 → 다음 컨베이어로 이동 시도
                if (nextPoint != null)
                {
                    //Vector3 nextPosOnNextConveyor = nextPoint.machine.startTr.transform.position;

                    //if (nextPoint.machine.CanLoad(nextPosOnNextConveyor))
                    //{
                        
                    //}
                    boxes.RemoveAt(i);
                    nextPoint.machine.Load(box, box.transform.position);
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
