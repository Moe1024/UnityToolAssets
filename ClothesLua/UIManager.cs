using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ClothesLua
{
    /// <summary>
    /// UI������
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager I; void Awake() { if (I != null) { Destroy(this); } I = this; }

        public DressType currentDressType;//��ǰ��װ����
        public GameObject[] buttons_DressTypes;//��װ���Ͱ�ť����
        public GameObject[] buttons_Items;//��Ʒ��ť����
        public Color[] itemColors;//��ɫ����

        void Start()
        {
            //��ʼ��Ĭ�Ϸ�װ���Ͱ�ť�¼�
            OnClick_SwitchDressType(0);
        }

        /// <summary>
        /// ��װ���Ͱ�ť����
        /// </summary>
        /// <param name="index">��ť����</param>
        void Ani_DressTypeButton(int index)
        {
            foreach (var o in buttons_DressTypes)
                o.transform.localScale = Vector3.one;
            var scale = buttons_DressTypes[index].transform.localScale;
            scale.x *= 1.5f;
            buttons_DressTypes[index].transform.localScale = scale;

            switch (index)
            {
                case 0:
                    currentDressType = DressType.Headdress;
                    break;
                case 1:
                    currentDressType = DressType.Clothes;
                    break;
                case 2:
                    currentDressType = DressType.Trousers;
                    break;
                case 3:
                    currentDressType = DressType.Shoe;
                    break;
            }
        }

        /// <summary>
        /// �л���װ���Ͱ�ť�¼�
        /// </summary>
        /// <param name="index">��ť����</param>
        public void OnClick_SwitchDressType(int index)
        {
            Ani_DressTypeButton(index);
            switch (index)
            {
                case 0:
                    ShowItemIcon(Role.I.headdresses, DressType.Headdress);
                    break;
                case 1:
                    ShowItemIcon(Role.I.clotheses, DressType.Clothes);
                    break;
                case 2:
                    ShowItemIcon(Role.I.trouserses, DressType.Trousers);
                    break;
                case 3:
                    ShowItemIcon(Role.I.shoes, DressType.Shoe);
                    break;
            }
        }

        /// <summary>
        /// չʾ��Ʒͼ��
        /// </summary>
        /// <param name="array">��װ����</param>
        /// <param name="type">��װ����</param>
        public void ShowItemIcon(GameObject[] array, DressType type)
        {
            foreach (var o in buttons_Items)
                o.SetActive(false);
            for (int i = 0; i < array.Length; i++)
            {
                buttons_Items[i].SetActive(true);
                TextMeshProUGUI text= buttons_Items[i].GetComponentInChildren<TextMeshProUGUI>();
                text.color = itemColors[i];
                switch (type)
                {
                    case DressType.Headdress:
                        text.text = "H";
                        break;
                    case DressType.Clothes:
                        text.text = "C";
                        break;
                    case DressType.Trousers:
                        text.text = "T";
                        break;
                    case DressType.Shoe:
                        text.text = "S";
                        break;
                }
            }
        }

        /// <summary>
        /// ѡ���װ��Ʒ��ť�¼�
        /// </summary>
        /// <param name="index">��װ����</param>
        public void OnClick_SelectItem(int index)
        {
            switch (currentDressType)
            {
                case DressType.Headdress:
                    Role.I.ChangeDress(Role.I.headdresses, DressType.Headdress, index);
                    break;
                case DressType.Clothes:
                    Role.I.ChangeDress(Role.I.clotheses, DressType.Clothes, index);
                    break;
                case DressType.Trousers:
                    Role.I.ChangeDress(Role.I.trouserses, DressType.Trousers, index);
                    break;
                case DressType.Shoe:
                    Role.I.ChangeDress(Role.I.shoes, DressType.Shoe, index);
                    break;
            }
        }
    }
}

