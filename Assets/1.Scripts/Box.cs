using UnityEngine;
using System.Collections;

public class Box : SimulationObject
{
    public float width = 0.235f;
    public float height = 0.2f;
    public float deeth = 0.18f;

    public ProductData productData;
    public void SetProductData(ProductData productData)
    {
        this.productData = productData;
    }
}

