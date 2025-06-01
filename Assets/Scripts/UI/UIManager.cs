using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Dictionary<string, BasePanel> PanelDict = new();

    protected void Awake()
    {
        var basePanels = GetComponentsInChildren<BasePanel>(true);
        foreach (var panel in basePanels)
        {
            panel.Init();
            PanelDict.Add(panel.GetId(), panel);
        }
    }

    public BasePanel GetPanel(string panelId)
    {
        return PanelDict[panelId];
    }
}
