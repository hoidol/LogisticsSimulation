using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Workbay : Conveyor
{
    public float processSpeed =2; //처리 속도
    //public float moveSpeed = 0.2f;
    //[SerializeField] Vector3 direction;
    public override void PlaySimulation()
    {
        base.PlaySimulation();
        Debug.Log("Workbay PlaySimulation() 함수 호출 ");
        sidePoints.Clear(); 
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
        //Conveyor conveyor = endLinkPoint.GetComponentInParent<Conveyor>();

        Load(box, Vector3.zero);
    }


    

}
