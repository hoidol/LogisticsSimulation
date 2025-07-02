using System.Collections.Generic;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    public static ProductManager Instance;

    //public List<Box> boxes = new List<Box>();
    public List<Box> boxesInPool = new List<Box>();

    [SerializeField] List<ProductData> productDatas = new List<ProductData>();
    private void Awake()
    {
        Instance = this;
    }

    public void StartMode(SimulationModeType type)
    {
        if(type == SimulationModeType.Play)
        {
            string csv = Resources.Load<TextAsset>("CSV/Product").text;
            ParseCSV(csv);
        }

        boxesInPool.ForEach(e => e.gameObject.SetActive(false));
    }


    public Box GetBox()
    {
        for(int i =0;i< boxesInPool.Count; i++)
        {
            if (!boxesInPool[i].gameObject.activeSelf)
            {
                boxesInPool[i].gameObject.SetActive(true);
                return boxesInPool[i];
            }
        }


        Box box = Instantiate(Resources.Load<Box>("Box"));
        boxesInPool.Add(box);
        return box;
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
            data.asrs_id = tokens[1];

            productDatas.Add(data);
        }
    }

    public void RemoveBox(Box b)
    {
        b.gameObject.SetActive(false);
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
    public string asrs_id;
}