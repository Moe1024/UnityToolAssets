using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XLua;

namespace ClothesLua
{

    public enum DressType
    {
        Headdress, Clothes, Trousers, Shoe,
    }
    /// <summary>
    /// 被换装的角色本身
    /// </summary>
    [LuaCallCSharp]
    public class Role_Lua : MonoBehaviour
    {
        public static Role_Lua I;

        public TextAsset luaScript;

        internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
        internal static float lastGCTime = 0;
        internal const float GCInterval = 1;//1 second 
        private LuaTable scriptEnv;

        private Action luaStart;//默认的Lua方法变量

        [CSharpCallLua] public delegate void Action_Array_DressType_Int(GameObject[] dresses, DressType type, int index);
        private Action_Array_DressType_Int changeDress;//自定义的Lua方法变量

        //当前服装索引
        public int currentHeaddressIndex;
        public int currentClothesIndex;
        public int currentTrousersIndex;
        public int currentShoeIndex;

        //各种服装数组
        public GameObject[] headdresses;
        public GameObject[] clotheses;
        public GameObject[] trouserses;
        public GameObject[] shoes;

        void Awake()
        {
            if (I != null) { Destroy(this); }
            I = this;

            InitLuaEnv();
            SetScripts();
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
            scriptEnv.Get("Start", out luaStart);

            scriptEnv.Get("ChangeDress", out changeDress);
        }


        void Start()
        {
            //初始化默认的服装
            if (luaStart != null)
            {
                luaStart();
            }

        }

        /// <summary>
        /// 换装方法
        /// </summary>
        /// <param name="dresses">服装数组</param>
        /// <param name="type">服装类型</param>
        /// <param name="index">换装索引</param>
        public void ChangeDress(GameObject[] dresses, DressType type, int index = 0)
        {
            if (changeDress != null)
                changeDress(dresses, type, index);
        }

    }
}

