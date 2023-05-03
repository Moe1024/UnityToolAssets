using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace ClothesLua
{
    /// <summary>
    /// 被换装的角色本身
    /// </summary>
    public class Role : MonoBehaviour
    {
        public static Role I; void Awake() { if (I != null) { Destroy(this); } I = this; }

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


        void Start()
        {
            //初始化默认的服装
            ChangeDress(headdresses, DressType.Headdress);
            ChangeDress(clotheses, DressType.Clothes);
            ChangeDress(trouserses, DressType.Trousers);
            ChangeDress(shoes, DressType.Shoe);
        }

        /// <summary>
        /// 换装方法
        /// </summary>
        /// <param name="dresses">服装数组</param>
        /// <param name="type">服装类型</param>
        /// <param name="index">换装索引</param>
        public void ChangeDress(GameObject[] dresses, DressType type, int index = 0)
        {
            for (int i = 0; i < dresses.Length; i++)
            {
                if (i != index) dresses[i].SetActive(false);
            }
            dresses[index].SetActive(true);

            switch (type)
            {
                case DressType.Headdress:
                    currentHeaddressIndex = index;
                    break;
                case DressType.Clothes:
                    currentClothesIndex = index;
                    break;
                case DressType.Trousers:
                    currentTrousersIndex = index;
                    break;
                case DressType.Shoe:
                    currentShoeIndex = index;
                    break;
            }
        }

    }
}

