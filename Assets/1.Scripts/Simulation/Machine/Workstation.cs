using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Workstation : Machine
{
    public float processSpeed =2; //처리 속도
    
    public override void PlaySimulation()
    {
        base.PlaySimulation();
        //Debug.Log("Workbay PlaySimulation() 함수 호출 ");
        timer = 0;
    }

    float timer;

    public override void StopSimulation()
    {
        base.StopSimulation();
        StopAllCoroutines();
    }

    private void Update()
    {
        if (SimulationManager.Instance.simulationModeType == SimulationModeType.Edit)
            return;

        if (timer >= processSpeed)
        {
            AddBox();
            timer = 0;
            return;
        }
        timer += Time.deltaTime; 
    }

    public override void Unload(Box box, Machine m)
    {
        m.Load(box, linkPoints[0].transform.position);
    }

    public void AddBox()
    {
        if (linkPoints.Length <= 0)
            return;
        if (linkPoints[0].linkedMachines.Count <= 0)
            return;

        ProductData productData = ProductManager.Instance.Push();
        if (productData == null)
            return;

        
        Box box = ProductManager.Instance.GetBox();
        box.machine = null;
        box.SetProductData(productData);

        Unload(box, linkPoints[0].linkedMachines[0]);
    }


    

}
