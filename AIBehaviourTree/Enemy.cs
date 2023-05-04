using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBTree
{
    public enum EnemyAround{None,Left,Right}

    /// <summary>
    /// AI���˵����ò���
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        public Animator animator;//AI��������Ķ�����

        public bool isMove;
        public bool isAttack;


        public EnemyAround around;

        [Header("Distance")]
        public float distance_Find = 10f;//��������
        public float distance_Around = 4.5f;//��Ȧ����

        [Header("Attack")]
        public float timer_Attack;//������ʱ��
        public float adjustTimer_Attack;//������ȴ��ʱ��
        public float timePoint_Attack_01=2f;//��������ʱ���һ
        public float probability_Attack_01 = 0.25f;//��һ��������
        public float timePoint_Attack_02=3.5f;//��������ʱ����
        public float probability_Attack_02 = 0.5f;//�����������
    }
}

