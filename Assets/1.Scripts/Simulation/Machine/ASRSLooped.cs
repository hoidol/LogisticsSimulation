using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ASRSLooped : Machine
{
    //public string asrsLoopedKey;//ASRSLooped 간의 구분자 - 수동 이름 설정 가능하게

    public float speed;

    //
    public List<ProductData> productDatas = new List<ProductData>();

    public override void PlaySimulation()
    {
        base.PlaySimulation();
        StartCoroutine(CoProcess());
    }
    
    public override void StopSimulation()
    {
        base.StopSimulation();
        StopAllCoroutines();
    }
    public override void CheckLink()
    {
        base.CheckLink();
        nextMachine = null;
        if(linkPoints[1].linkedMachines.Count >0)
            nextMachine = linkPoints[1].linkedMachines[0];
    }

    IEnumerator CoProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(speed);
            //Debug.Log("ASRSLooped Unload시도 ");
            if (productDatas.Count <= 0)
                continue;

            if (linkPoints.Length < 2)
                continue;
                
            if (nextMachine == null)
                continue;

            if (!nextMachine.CanLoad())
                continue;
            
                

            ProductData productData = productDatas[0];
            productDatas.RemoveAt(0);
            Box box = ProductManager.Instance.GetBox();
            box.SetProductData(productData);


            Unload(box,nextMachine);
        }
    }

    public override void Load(Box box, Vector3 pos)
    {
        base.Load(box, pos);
        
        productDatas.Add(box.productData);
        ProductManager.Instance.RemoveBox(box);
        
    }

    public override void Unload(Box box, Machine at)
    {
        Debug.Log("ASRS Looped Unload () 함수 호출 ");

        at.Load(box,Vector3.zero);
        
        
    }

} 
