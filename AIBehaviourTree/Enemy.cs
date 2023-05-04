using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBTree
{
    public enum EnemyAround{None,Left,Right}

    /// <summary>
    /// AI敌人的设置参数
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        public Animator animator;//AI敌人自身的动画器

        public bool isMove;
        public bool isAttack;


        public EnemyAround around;

        [Header("Distance")]
        public float distance_Find = 10f;//发觉距离
        public float distance_Around = 4.5f;//绕圈距离

        [Header("Attack")]
        public float timer_Attack;//攻击计时器
        public float adjustTimer_Attack;//攻击冷却计时器
        public float timePoint_Attack_01=2f;//攻击可能时间点一
        public float probability_Attack_01 = 0.25f;//点一攻击概率
        public float timePoint_Attack_02=3.5f;//攻击可能时间点二
        public float probability_Attack_02 = 0.5f;//点二攻击概率
    }
}

