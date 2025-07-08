using System.Collections.Generic;
using UnityEngine;

public class OutPoint : Machine
{
    public List<Box> stackedBoxs = new List<Box>();
    public int maxStackCount = 27; //maxStack이되면 3초 후 사라짐

    public override void StopSimulation()
    {
        base.StopSimulation();
        stackedBoxs.Clear();
    }
    public override void Load(Box box, Vector3 pos)
    {
        base.Load(box, pos);
        if (!CanLoad())
            return;

        stackedBoxs.Add(box);


        int index = stackedBoxs.Count - 1;

        // 3차원 인덱스 계산 (가로 3, 세로 3)
        int layer = index / 9;        // Y축 (0~2)
        int row = (index % 9) / 3;    // Z축 (0~2)
        int col = index % 3;          // X축 (0~2)

        // 기준 위치 (예: OutPoint 기준)
        Vector3 basePos = transform.position;

        // 박스 간 간격 (약간 여유 두기)
        float spacingX = box.width * 2f + 0.01f;
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

        if (stackedBoxs.Count >= maxStackCount)
        {
            Invoke("FlushStack",3);
        }

    }
    public override bool CanLoad()
    {
        return stackedBoxs.Count < maxStackCount;
    }


    public void FlushStack()
    {
        for(int i = stackedBoxs.Count-1;i >= stackedBoxs.Count; i++)
        {
            ProductManager.Instance.RemoveBox(stackedBoxs[i]);
            
        }
        stackedBoxs.Clear();

    }



}
