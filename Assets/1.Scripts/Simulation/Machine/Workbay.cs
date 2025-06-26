using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Workbay : Machine
{
    public Conveyor conveyor;
    public float processSpeed =2; //처리 속도

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

    IEnumerator CoProcess()
    {
        float timer = processSpeed;
        while (true)
        {
            if(timer >= processSpeed)
            {
                AddBox();
                timer = 0; ;
            }
            yield return null;
            timer += Time.deltaTime;
            
        }
    }

    public void AddBox()
    {
        ProductData productData = ProductManager.Instance.Push();
        if (productData == null)
            return;

        Box box = Instantiate(Resources.Load<Box>("Box"));
        box.SetProductData(productData);
        conveyor.Load(box, Vector3.zero);
    }
    

}
