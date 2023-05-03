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
    /// ����װ�Ľ�ɫ����
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

        private Action luaStart;//Ĭ�ϵ�Lua��������

        [CSharpCallLua] public delegate void Action_Array_DressType_Int(GameObject[] dresses, DressType type, int index);
        private Action_Array_DressType_Int changeDress;//�Զ����Lua��������

        //��ǰ��װ����
        public int currentHeaddressIndex;
        public int currentClothesIndex;
        public int currentTrousersIndex;
        public int currentShoeIndex;

        //���ַ�װ����
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
            //��ʼ������
            scriptEnv = luaEnv.NewTable();
            // Ϊÿ���ű�����һ�������Ļ�������һ���̶��Ϸ�ֹ�ű���ȫ�ֱ�����������ͻ
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();
            //���ýű�ִ����
            scriptEnv.Set("self", this);
            luaEnv.DoString(luaScript.text, luaScript.name, scriptEnv);
        }
        void SetScripts()
        {
            //���ú�����������
            scriptEnv.Get("Start", out luaStart);

            scriptEnv.Get("ChangeDress", out changeDress);
        }


        void Start()
        {
            //��ʼ��Ĭ�ϵķ�װ
            if (luaStart != null)
            {
                luaStart();
            }

        }

        /// <summary>
        /// ��װ����
        /// </summary>
        /// <param name="dresses">��װ����</param>
        /// <param name="type">��װ����</param>
        /// <param name="index">��װ����</param>
        public void ChangeDress(GameObject[] dresses, DressType type, int index = 0)
        {
            if (changeDress != null)
                changeDress(dresses, type, index);
        }

    }
}

