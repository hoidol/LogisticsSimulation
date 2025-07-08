using UnityEngine;
using System.Collections;

public class Box : SimulationObject
{
    public Transform cameraTr;
    public float width = 0.235f;
    public float height = 0.2f;
    public float deeth = 0.18f;

    public ProductData productData;
    public void SetProductData(ProductData productData)
    {
        this.productData = productData;
    }
    public Machine machine;

    public override void ViewDetail(bool on)
    {
        cameraTr.gameObject.SetActive(on);
    }

    public void Loaded(Machine m)
    {
        machine = m;
        transform.parent = machine.transform;
    }

    private void OnDisable()
    {
        if (SimulationManager.Instance.simulationModeType != SimulationModeType.Play)
            return;
        if (PlayMode.Instanace.targetBox == null)
            return;

        if(PlayMode.Instanace.targetBox == this)
        {
            PlayMode.Instanace.ClosedDetail();
        }
    }
}

