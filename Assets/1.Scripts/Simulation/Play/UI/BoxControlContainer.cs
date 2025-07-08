using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class BoxControlContainer : ObjectControlContainer
{
    public List<BoxControlPanel> boxControlPanels = new List<BoxControlPanel>();
    public Transform panelParent;
    public BoxControlPanel boxControlPanelPrefab;

    public TMP_Text countText;

    public override void Init()
    {
        objName = "Box";
        if (boxControlPanels.Count <= 0)
        {
            BoxControlPanel[] panels = GetComponentsInChildren<BoxControlPanel>();
            boxControlPanels.AddRange(panels);
        }
             

    }

    BoxControlPanel GetBoxControlPanel()
    {
        for(int i =0;i< boxControlPanels.Count; i++)
        {
            if (!boxControlPanels[i].gameObject.activeSelf)
            {
                boxControlPanels[i].gameObject.SetActive(true);
                return boxControlPanels[i];
            }
        }

        BoxControlPanel panel = Instantiate(boxControlPanelPrefab, panelParent);
        boxControlPanels.Add(panel);
        return panel;

    }

    public override void UpdateControl()
    {
        for(int i =0;i< boxControlPanels.Count; i++)
        {
            boxControlPanels[i].gameObject.SetActive(false);
        }
        List<Box> boxes = ProductManager.Instance.GetBoxes();

        Debug.Log($"BoxControlContainer UpdateControl boxes.Count {boxes.Count}");
        countText.text = string.Format("{0}EA", boxes.Count);
        for (int i = 0; i < boxes.Count; i++)
        {
            BoxControlPanel panel = GetBoxControlPanel();
            panel.SetBox(boxes[i]);
        }
    }

    public override void UpdateContainer()
    {
        
        UpdateControl();
    }
}
