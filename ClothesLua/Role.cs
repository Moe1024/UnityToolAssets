using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace ClothesLua
{
    public class Role : MonoBehaviour
    {
        public static Role I; void Awake() { if (I != null) { Destroy(this); } I = this; }

        public int currentHeaddressIndex;
        public int currentClothesIndex;
        public int currentTrousersIndex;
        public int currentShoeIndex;

        public GameObject[] headdresses;
        public GameObject[] clotheses;
        public GameObject[] trouserses;
        public GameObject[] shoes;


        void Start()
        {
            ChangeDress(headdresses, DressType.Headdress);
            ChangeDress(clotheses, DressType.Clothes);
            ChangeDress(trouserses, DressType.Trousers);
            ChangeDress(shoes, DressType.Shoe);
        }

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

