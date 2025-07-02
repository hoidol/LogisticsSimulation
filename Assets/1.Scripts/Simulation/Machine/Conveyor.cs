using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Machine
{
    public LinkPoint startLinkPoint; //
    public LinkPoint endLinkPoint;
    public float moveSpeed = 0.2f;

    //수정 모드에서 설정됨
    //public List<Conveyor> conveyorsToAsrs; // asrs로 향하는 연결된 컨베이어 
    //public Machine preMachine; //이전 연결점 
    //public LinkPoint nextLinkPoint; // 

    
    public Vector3 direction;
    //public string asrsLoodedId; //

    public override void Init(MachineSaveData data)
    {
        base.Init(data);
        EditMachine();
    }

    public override void PlaySimulation()
    {
        base.PlaySimulation();
        //StartCoroutine(CoProcess());
    }

    public override void StopSimulation()
    {
        base.StopSimulation();
        StopAllCoroutines();
    }

    public override void EditMachine()
    {
        direction = (endLinkPoint.transform.position - startLinkPoint.transform.position).normalized;
        base.EditMachine();
    }

    public List<LinkPoint> inLinkPoints = new List<LinkPoint>();
    public override void CheckLink()
    {
        base.CheckLink();
        inLinkPoints.Clear();
        BoxCollider box = GetComponent<BoxCollider>();

        Vector3 worldCenter = transform.TransformPoint(box.center);
        Vector3 halfExtents = Vector3.Scale(box.size * 0.5f, transform.lossyScale); // 스케일까지 반영

        Collider[] colliders = Physics.OverlapBox(
            worldCenter,
            halfExtents,
            transform.rotation,
            LayerMask.GetMask("LinkPoint")
        );

       

        foreach (var col in colliders)
        {
            LinkPoint linkedPoint = col.GetComponent<LinkPoint>();
            if (linkedPoint.Machine == this)
                continue;

            linkedPoints.Add(linkedPoint);
        }

        
        for (int i =0;i< linkedPoints.Count; i++)
        {
            if (linkedPoints[i].Machine == this)
            {
                //현재 장비에서는 Out으로 설정
                if(linkedPoints[i].transferDirection == TransferDirection.Out)
                    inLinkPoints.Add(linkedPoints[i]);
                continue;
            }
                
            if (linkedPoints[i].transferDirection == TransferDirection.In)
                inLinkPoints.Add(linkedPoints[i]);
        }

        if(endLinkPoint.linkedMachines.Count >0)
            nextMachine = endLinkPoint.linkedMachines[0];

        //float minDist = float.MaxValue;
        //nextMachine = null;

        //foreach (var lp in outLinkPoints)
        //{
        //    float dist = Vector3.Distance(endLinkPoint.transform.position, lp.transform.position);
        //    if (dist < 1f && dist < minDist)
        //    {
        //        minDist = dist;
        //        nextLinkPoint = lp;
        //    }
        //}
    }
    BoxCollider boxCollider;
    void OnDrawGizmos()
    {
        
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();

        Gizmos.color = Color.red;

        // BoxCollider의 로컬 중심을 월드 기준으로 변환
        Vector3 worldCenter = transform.TransformPoint(boxCollider.center);
        Vector3 size = boxCollider.size ; // extents가 아니라 size 그대로 사용

        // 현재 오브젝트의 회전 적용
        Quaternion rotation = transform.rotation;

        Matrix4x4 matrix = Matrix4x4.TRS(worldCenter, rotation, Vector3.one);
        Gizmos.matrix = matrix;

        // 로컬 공간 기준으로 그리므로 center는 Vector3.zero
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
    // 박스를 pos 위치에 올리고 리스트에 등록
    public override void Load(Box box, Vector3 pos)
    {
        base.Load(box, pos);
        Vector3 centerLinePos;

        if (pos != Vector3.zero)
        {
            Vector3 flatDirection = new Vector3(direction.x, 0, direction.z).normalized;
            Vector3 offset = flatDirection * Vector3.Dot(pos - startLinkPoint.transform.position, flatDirection);
            centerLinePos = startLinkPoint.transform.position + offset;
        }
        //box.transform.position = pos;
        else
            centerLinePos = startLinkPoint.transform.position;

        box.transform.position = centerLinePos;
        box.transform.parent = transform;
        box.transform.forward = direction;
        boxes.Add(box);
    }

    // 해당 위치에 박스를 올릴 수 있는지 확인
    //public override bool CanLoad()
    //{
    //    foreach (var box in boxes)
    //    {
    //        float distance = Vector3.Distance(pos, box.transform.position);
    //        float minDistance = box.width + 0.01f;
    //        if (distance < minDistance)
    //            return false;
    //    }
    //    return true;
    //}

    IEnumerator CoProcess()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            CheckUnloadBox();
        }
    }

    void CheckUnloadBox()
    {
        for (int i = boxes.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < inLinkPoints.Count; j++)
            {
                float dis = Vector3.Distance(inLinkPoints[j].transform.position, boxes[i].transform.position);
                if (dis > 0.5f)
                    continue;

                if (string.IsNullOrEmpty(inLinkPoints[j].Machine.destinationId) || boxes[i].productData.asrs_id == inLinkPoints[j].Machine.destinationId)
                {
                    Unload(boxes[i], inLinkPoints[j].Machine);
                    break;
                }
               
            }
        }
    }

    private void Update()
    {
        if (boxes.Count <= 0)
            return;

        for (int i = 0; i < boxes.Count; i++)
        {
            // 이동
            boxes[i].transform.position = boxes[i].transform.position + direction * moveSpeed * Time.deltaTime;

        }
    }

    public override void Unload(Box box,Machine at)
    {
        boxes.Remove(box);
        if (at == null || !at.CanLoad())
        {
            ProductManager.Instance.RemoveBox(box);
            //박스 버려짐
            Debug.Log("박스 버려짐");
            return;
        }
        
        at.Load(box, box.transform.position);
    }

}
