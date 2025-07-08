using UnityEngine;

public class GroundGrid : MonoBehaviour
{
    public int gridSizeX = 10;
    public int gridSizeZ = 10;
    public float cellSize = 1f;
    public Material lineMaterial;
    public Color lineColor;
    public float lineWidth = 0.05f;

    void Start()
    {
        DrawGrid();
    }
    void DrawGrid()
    {
        float halfSizeX = gridSizeX * cellSize * 0.5f;
        float halfSizeZ = gridSizeZ * cellSize * 0.5f;

        // Draw vertical lines (Z 방향으로 뻗는 선, X 기준)
        for (int i = 0; i <= gridSizeX; i++)
        {
            float x = -halfSizeX + i * cellSize;
            Vector3 start = transform.position + new Vector3(x, 0, -halfSizeZ);
            Vector3 end = transform.position + new Vector3(x, 0, halfSizeZ);
            CreateLine(start, end);
        }

        // Draw horizontal lines (X 방향으로 뻗는 선, Z 기준)
        for (int j = 0; j <= gridSizeZ; j++)
        {
            float z = -halfSizeZ + j * cellSize;
            Vector3 start = transform.position + new Vector3(-halfSizeX, 0.01f, z);
            Vector3 end = transform.position + new Vector3(halfSizeX, 0.01f, z);
            CreateLine(start, end);
        }
    }

    void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.parent = this.transform;

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        lr.material = lineMaterial;
        lr.startColor = lineColor;
        lr.endColor = lineColor;
        lr.widthMultiplier = lineWidth;
        lr.useWorldSpace = true;
        lr.loop = false;
    }
}
