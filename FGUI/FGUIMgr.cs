using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System;
/// <summary>
/// FairyGUI 管理者
/// </summary>
public class FGUIMgr : MonoBehaviour
{
    public static FGUIMgr I; void Awake() { if (I != null) Destroy(I); I = this; }

    public static Dictionary<string, string> ResourcesDirectory = new Dictionary<string, string>()//资源目录
    {
        { "MainPanel", "MainPackage" },//"MainPanel" 为一个面板类，也是 FGUI 编辑器里 "MainPackage" 包中的一块UI面板子节点
        { "PanelName", "PackageName" },
    };
    private Dictionary<string, UIPackage> PackagesDirectory = new Dictionary<string, UIPackage>();//包目录

    public Dictionary<string, PanelBase> PanelDirectory = new Dictionary<string, PanelBase>();//面板目录

    public void LoadPackage(string panelName)//加载包资源
    {
        string packageName = ResourcesDirectory[panelName];
        if (PackagesDirectory.ContainsKey(packageName)) return;
        var package = UIPackage.AddPackage("FGUI/" + packageName);
        PackagesDirectory.Add(packageName, package);
    }
    public void BuildPanel<T>()
    {
        string panelName = typeof(T).Name;
        if (!PanelDirectory.TryGetValue(panelName, out var panel))
        {
            panel = UIPackage.CreateObject(ResourcesDirectory[panelName], panelName) as PanelBase;
            panel = (PanelBase)Activator.CreateInstance(typeof(T));//让子类装进父类的容器，但其实质内容还是子类
            panel.sortingOrder = panel.GetSortIndex();//设置排序
            GRoot.inst.AddChild(panel);//把UI添加给UI列表
            panel.MakeFullScreen();//设置UI覆盖全屏幕
            PanelDirectory.Add(panelName, panel);
        }
    }
    public T ShowPanel<T>(bool isShow) where T : PanelBase
    {
        string panelName = typeof(T).Name;
        T panel = PanelDirectory[panelName] as T;
        panel.visible = isShow;
        if(isShow)panel.OnShow();//触发 T 类的 OnShow() 函数
        return panel;
    }
    public T GetPanel<T>() where T : PanelBase
    {
        string panelName = typeof(T).Name;
        return PanelDirectory[panelName] as T;
    }

    public class PanelBase : GComponent
    {
        /// <summary>
        /// 返回排序索引
        /// </summary>
        public virtual int GetSortIndex(){return 0;}
        /// <summary>
        /// UI 面板展示时触发
        /// </summary>
        public virtual void OnShow(){}
    }
    public class MainPanel : PanelBase
    {
        public override int GetSortIndex()
        {
            return 0;
        }
        public override void OnShow()
        {
        }
    }
}
