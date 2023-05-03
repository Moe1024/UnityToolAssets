using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;

namespace ClothesLua
{

    /// <summary>
    /// UI管理者
    /// </summary>
    [LuaCallCSharp]
    public class UIManager_Lua : MonoBehaviour
    {
        public static UIManager_Lua I;

        public TextAsset luaScript;

        internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
        internal static float lastGCTime = 0;
        internal const float GCInterval = 1;//1 second 
        private LuaTable scriptEnv;

        //默认的Lua方法变量
        private Action luaAwake;
        private Action luaStart;
        private Action luaUpdate;

        //自定义的Lua方法变量
        [CSharpCallLua] public delegate void Action_Int(int a);
        [CSharpCallLua] public delegate void Action_Array_Enum(GameObject[] a, Enum b);
        private Action_Int ani_DressTypeButton;
        private Action_Int onClick_SwitchDressType;
        private Action_Array_Enum showItemIcon;
        private Action_Int onClick_SelectItem;


        public DressType currentDressType;//当前服装类型
        public GameObject[] buttons_DressTypes;//服装类型按钮数组
        public GameObject[] buttons_Items;//物品按钮数组
        public Color[] itemColors;//颜色数组


        void Awake()
        {
            if (I != null) { Destroy(this); }
            I = this;

            InitLuaEnv();
            SetScripts();
            if (luaAwake != null)
            {
                luaAwake();
            }
        }
        void InitLuaEnv()
        {
            //初始化环境
            scriptEnv = luaEnv.NewTable();
            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();
            //设置脚本执行体
            scriptEnv.Set("self", this);
            luaEnv.DoString(luaScript.text, luaScript.name, scriptEnv);
        }
        void SetScripts()
        {
            //设置函数生命周期
            luaAwake = scriptEnv.Get<Action>("Awake");
            scriptEnv.Get("Start", out luaStart);
            scriptEnv.Get("Update", out luaUpdate);

            scriptEnv.Get("Ani_DressTypeButton", out ani_DressTypeButton);
            scriptEnv.Get("OnClick_SwitchDressType", out onClick_SwitchDressType);
            scriptEnv.Get("ShowItemIcon", out showItemIcon);
            scriptEnv.Get("OnClick_SelectItem", out onClick_SelectItem);
        }

        void Start()
        {
            //初始化默认服装类型按钮事件
            if (luaStart != null)
            {
                luaStart();
            }
        }

        void Update()
        {
            if (luaUpdate != null)
            {
                luaUpdate();
            }
            if (Time.time - UIManager_Lua.lastGCTime > GCInterval)
            {
                luaEnv.Tick();
                UIManager_Lua.lastGCTime = Time.time;
            }
        }

        /// <summary>
        /// 服装类型按钮动画
        /// </summary>
        /// <param name="index">按钮索引</param>
        public void Ani_DressTypeButton(int index)
        {
            if (ani_DressTypeButton != null) ani_DressTypeButton(index);
        }

        /// <summary>
        /// 切换服装类型按钮事件
        /// </summary>
        /// <param name="index">按钮索引</param>
        public void OnClick_SwitchDressType(int index)
        {
            if (onClick_SwitchDressType != null) onClick_SwitchDressType(index);
        }

        /// <summary>
        /// 展示物品图标
        /// </summary>
        /// <param name="array">服装数组</param>
        /// <param name="type">服装类型</param>
        public void ShowItemIcon(GameObject[] array, Enum type)
        {
            if (showItemIcon != null) showItemIcon(array, type);
        }

        /// <summary>
        /// 选择服装物品按钮事件
        /// </summary>
        /// <param name="index">换装索引</param>
        public void OnClick_SelectItem(int index)
        {
            if (onClick_SelectItem != null) onClick_SelectItem(index);
        }
    }
}
