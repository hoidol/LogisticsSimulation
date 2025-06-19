using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour, ISimulationMachine
{
    public List<Box> boxes = new List<Box>();

    public LinkPoint startTr;
    public LinkPoint endTr;
    public float speed = 0.2f;

    //수정 모드에서 설정됨
    public List<Conveyor> conveyorsToAsrs; // asrs로 향하는 연결된 컨베이어 

    public Conveyor preConveyor; //이전 컨베이어
    public Conveyor nextConveyor; // ✅ 다음 컨베이어

    public void StartSimulation()
    {

    }

    public void StopSimulation()
    {

    }

    
    // 박스를 pos 위치에 올리고 리스트에 등록
    public void Put(Box box, Vector3 pos)
    {
        if (pos != Vector3.zero)
            box.transform.position = pos;
        else
            box.transform.position = startTr.transform.position;

        boxes.Add(box);
    }

    // 해당 위치에 박스를 올릴 수 있는지 확인
    public bool CanPut(Vector3 pos)
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

        Vector3 direction = (endTr.transform.position - startTr.transform.position).normalized;

        for (int i = boxes.Count - 1; i >= 0; i--)
        {
            Box box = boxes[i];
            Vector3 current = box.transform.position;
            Vector3 target = endTr.transform.position;

            // 이동
            Vector3 nextPos = current + direction * speed * Time.deltaTime;

            if (Vector3.Distance(current, target) > 0.01f)
            {
                box.transform.position = nextPos;
            }
            else
            {
                // ✅ 도착 지점 도달 → 다음 컨베이어로 이동 시도
                if (nextConveyor != null)
                {
                    Vector3 nextPosOnNextConveyor = nextConveyor.startTr.transform.position;

                    if (nextConveyor.CanPut(nextPosOnNextConveyor))
                    {
                        boxes.RemoveAt(i);
                        nextConveyor.Put(box, nextPosOnNextConveyor);
                    }
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
