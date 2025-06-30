using System.Collections.Generic;
using UnityEngine;

public class LinkIndicatorCanvas : MonoBehaviour
{
    private static LinkIndicatorCanvas instance;
    public static LinkIndicatorCanvas Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<LinkIndicatorCanvas>(FindObjectsInactive.Include); 
            return instance;
        }
    }

    public LinkIndicator[] linkIndicators;
    public void Init()
    {
        linkIndicators = GetComponentsInChildren<LinkIndicator>();
    }

    [SerializeField] Color unlinkedColor;
    [SerializeField] Color linkedColor;
    Machine machine;
    public void ShowLink(Machine machine)
    {
        this.machine = machine;

        if (linkIndicators == null || linkIndicators.Length == 0)
            Init();
        Debug.Log("LinkIndicatorCanvas ShowLink");
        gameObject.SetActive(true);
        UpdateCanvas();
    }
    List<LinkPoint> linkPoints = new List<LinkPoint>();
    public void UpdateCanvas()
    {
        linkPoints.Clear();
        // 안보이게 초기화
        foreach (var indicator in linkIndicators)
            indicator.gameObject.SetActive(false);
        linkPoints.AddRange(machine.linkPoints);
        if(machine is Conveyor)
        {
            linkPoints.AddRange(((Conveyor)machine).sidePoints);
        }
        for (int i = 0; i < machine.linkPoints.Length; i++)
        {
            LinkPoint linkPoint = machine.linkPoints[i];
            LinkIndicator indicator = linkIndicators[i];

            // 월드 좌표 → 스크린 좌표
            Vector3 screenPos = Camera.main.WorldToScreenPoint(linkPoint.transform.position);

            // 스크린 좌표 → RectTransform 위치
            RectTransform rect = indicator.GetComponent<RectTransform>();
            rect.position = screenPos;

            // 색상 처리
            indicator.SetColor(linkPoint.linkedPoint != null ? linkedColor : unlinkedColor);

            // 활성화
            indicator.gameObject.SetActive(true);
        }
    }

}
