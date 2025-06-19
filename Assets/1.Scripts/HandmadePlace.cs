using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class HandWorkSpace : MonoBehaviour, ISimulationMachine
{
    public Conveyor conveyor;
    public float processSpeed =2; //처리 속도

    public void StartSimulation()
    {
        StartCoroutine(CoProcess());
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
        Box box = Instantiate(Resources.Load<Box>("Box"));
        conveyor.Put(box, Vector3.zero);
    }
    
    public void StopSimulation()
    {

    }

}
