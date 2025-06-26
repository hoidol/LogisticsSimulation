using System.Collections.Generic;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    public static ProductManager Instance;

    
    [SerializeField] List<ProductData> productDatas = new List<ProductData>();
    private void Awake()
    {
        Instance = this;
        string csv = Resources.Load<TextAsset>("CSV/Product").text;
       ParseCSV(csv);
    }

    void ParseCSV(string csv)
    {
        string[] lines = csv.Split('\n');
        int count = lines.Length - 1; // 첫 줄은 헤더
        //ProductData[] result = new ProductData[count];
        productDatas.Clear();
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] tokens = line.Split(',');
            if (tokens.Length < 2) continue;

            ProductData data = new ProductData();
            data.id = tokens[0];
            data.asrs_no = tokens[1];

            productDatas.Add(data);
        }
    }

    //ProductData 중 하나를 가져옴
    public ProductData Push()
    {
        if (productDatas.Count <= 0)
            return null;
        ProductData next = productDatas[0];
        productDatas.RemoveAt(0);

        return next;
    }
}
[System.Serializable]
public class ProductData
{
    public string id;
    public string asrs_no;
}