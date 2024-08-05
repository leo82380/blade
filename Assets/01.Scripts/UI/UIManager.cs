using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WindowEnum
{
    LevelUp
}

public class UIManager : MonoSingleton<UIManager>
{
    public Dictionary<WindowEnum, IWindowPanel> panelDictionary;

    [SerializeField] private Transform _canvasTrm;

    private void Awake()
    {
        panelDictionary = new Dictionary<WindowEnum, IWindowPanel>();
        foreach(WindowEnum windowEnum in Enum.GetValues(typeof(WindowEnum)))
        {
            IWindowPanel panel = _canvasTrm
                .GetComponent($"{windowEnum.ToString()}Panel") as IWindowPanel;
            panelDictionary.Add(windowEnum, panel);
        }
    }

    public void Open(WindowEnum target)
    {
        if(panelDictionary.TryGetValue(target, out IWindowPanel panel))
        {
            panel.Open();
        }
    }

    public void Close(WindowEnum target)
    {
        if (panelDictionary.TryGetValue(target, out IWindowPanel panel))
        {
            panel.Close();
        }
    }
}
