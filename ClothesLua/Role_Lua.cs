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
    [LuaCallCSharp]
    public class Role_Lua : MonoBehaviour
    {
        public static Role_Lua I;

        public TextAsset luaScript;

        internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
        internal static float lastGCTime = 0;
        internal const float GCInterval = 1;//1 second 
        private LuaTable scriptEnv;

        private Action luaStart;

        [CSharpCallLua] public delegate void Action_Array_DressType_Int(GameObject[] dresses, DressType type, int index);
        private Action_Array_DressType_Int changeDress;

        public int currentHeaddressIndex;
        public int currentClothesIndex;
        public int currentTrousersIndex;
        public int currentShoeIndex;

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
            if (luaStart != null)
            {
                luaStart();
            }

        }

        public void ChangeDress(GameObject[] dresses, DressType type, int index = 0)
        {
            if (changeDress != null)
                changeDress(dresses, type, index);
        }

    }
}

