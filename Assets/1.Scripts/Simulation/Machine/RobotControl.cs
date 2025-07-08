using UnityEngine;

using System.Collections;
using System.Collections.Generic;
public class RobotControl : Machine
{
    public Transform stackPoint;
    public Transform boxPickPoint;

    public static int MAX_Stack_COUNT =27;
    public float loadSpeed;
    public Animator animator;
    bool canLoad;
    public override void PlaySimulation()
    {
        base.PlaySimulation();
        animator = GetComponentInChildren<Animator>();
        canLoad = linkPoints[0].linkedMachines.Count > 0;
        StartCoroutine(CoTryToLoad());
    }
    public override void StopSimulation()
    {
        base.StopSimulation();
        StopAllCoroutines();
        boxes.Clear();
    }

    IEnumerator CoTryToLoad()
    {
        while (true)
        {
            yield return new WaitForSeconds(loadSpeed);
            if (CanLoad())
            {
                Collider[] cols = Physics.OverlapSphere(linkPoints[0].transform.position, 0.7f, LayerMask.GetMask("Box"));
                if (cols.Length > 0)
                {
                    animator.Play("Pick");
                    yield return new WaitForSeconds(0.3f);
                    Box b = cols[0].GetComponent<Box>();
                    Pick(b);
                    yield return new WaitForSeconds(0.6f);
                    Stack(b);
                }
            }
           
            

            //Link 위치에 있는 박스를 들고옴
        }    
    }
    public override void Load(Box box, Vector3 pos)
    {
        base.Load(box, pos);
        boxes.Add(box);
        box.transform.parent = boxPickPoint;
        box.transform.localPosition = Vector3.zero;
        box.transform.localRotation = Quaternion.identity; 
    }
    public void Pick(Box box)
    {
        if(box.machine is Conveyor)
        {
            box.machine.Unload(box, this);
        }
        
    }
    public void Stack(Box box)
    {
        int index = boxes.Count - 1;

        // 3차원 인덱스 계산 (가로 3, 세로 3)
        int layer = index / 9;        // Y축 (0~2)
        int row = (index % 9) / 3;    // Z축 (0~2)
        int col = index % 3;          // X축 (0~2)

        // 기준 위치 (예: OutPoint 기준)
        Vector3 basePos = stackPoint.position;

        // 박스 간 간격 (약간 여유 두기)
        float spacingX = box.width *2f + 0.01f;
        float spacingY = box.height * 2f + 0.01f;
        float spacingZ = box.deeth * 2f + 0.01f;

        // 위치 계산
        Vector3 localOffset = new Vector3(
            (col - 1) * spacingX,
            layer * spacingY,
            (row - 1) * spacingZ
        );

        box.transform.position = basePos + localOffset;
        box.transform.rotation = Quaternion.identity;
        box.transform.parent = transform;

    }

    public override bool CanLoad()
    {
        return canLoad && boxes.Count < MAX_Stack_COUNT;
    }

    public override void CheckLink()
    {
        base.CheckLink();
        
        if (linkPoints[0].linkedMachines.Count > 0)
            nextMachine = linkPoints[0].linkedMachines[0];
    }

    //Load 후 3초 후 Stack Point
    

    
}
