using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBTree;

/// <summary>
/// 基础节点，为所有节点的父类，提供可继承的变量和方法
/// </summary>
[Serializable]
public class NodeBase : MonoBehaviour
{
    [Serializable] public delegate void Action();
    [Serializable] public delegate bool Condition();
    public Condition condition;
    public Action action;
    public Action update;
    public bool isDone;
    public List<NodeBase> childs = new List<NodeBase>();

    public Enemy enemy;
    public Player player;

    public NodeBase()
    {
        condition = ConditionFunc;
        action = ActionFunc;
        update = UpdateFunc;
    }

    public virtual bool ConditionFunc()
    {
        return false;
    }
    public virtual void ActionFunc()
    {
        isDone = true;
    }
    public virtual void UpdateFunc()
    {
    }
}

public class Node_Idle : NodeBase
{
    public override void ActionFunc()
    {
        base.ActionFunc();
        TreeNodeMgr.I.enemy.animator.Play("Idle");
    }
}
public class Node_Posture : NodeBase
{
    public Node_Posture()
    {
        enemy = TreeNodeMgr.I.enemy;
        player = TreeNodeMgr.I.player;
    }
    public override bool ConditionFunc()
    {
        if(Vector3.Distance(enemy.transform.position, player.transform.position) <= enemy.distance_Find)
            return true;
        enemy.animator.Play("Idle");
        enemy.isMove = false;
        isDone = false;
        return false;
    }
    public override void ActionFunc()
    {
        base.ActionFunc();
        enemy.animator.SetBool("ToPosture", true);
        TreeNodeMgr.I.Ani_Walk_Front();
    }
    public override void UpdateFunc()
    {
        if (!enemy.isAttack) enemy.transform.LookAt(player.transform.position);
        if (!enemy.isAttack && enemy.isMove) enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, player.transform.position, Time.deltaTime * 3f);
    }
}
public class Node_Around : NodeBase
{
    public Node_Around()
    {
        enemy = TreeNodeMgr.I.enemy;
        player = TreeNodeMgr.I.player;
    }
    public override bool ConditionFunc()
    {
        if (Vector3.Distance(enemy.transform.position, player.transform.position) <= enemy.distance_Around && !enemy.isAttack)
        {
            return true;
        }
        else
        {
            isDone = false;
            return false;
        }
    }
    public override void ActionFunc()
    {
        base.ActionFunc();
        if (enemy.around == EnemyAround.None)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                enemy.around = EnemyAround.Left;
                enemy.animator.Play("Walk_Left");
            }
            else
            {
                enemy.around = EnemyAround.Right;
                enemy.animator.Play("Walk_Right");

            }
        }
    }
    public override void UpdateFunc()
    {
        if (enemy.around == EnemyAround.Left)
            enemy.transform.RotateAround(player.transform.position, Vector3.up, Time.deltaTime * 15);
        else if (enemy.around == EnemyAround.Right)
            enemy.transform.RotateAround(player.transform.position, Vector3.up, -Time.deltaTime * 15);
        else
            ActionFunc();
    }
}

public class Node_Attack : NodeBase
{
    public Node_Attack()
    {
        enemy = TreeNodeMgr.I.enemy;
        player = TreeNodeMgr.I.player;
    }
    public override bool ConditionFunc()
    {
        if (!enemy.isAttack)
        {
            enemy.timer_Attack += Time.deltaTime;
            if (enemy.adjustTimer_Attack > 0) enemy.adjustTimer_Attack -= Time.deltaTime;
            var timer = float.Parse(enemy.timer_Attack.ToString("#0.0"));
            if ((timer % enemy.timePoint_Attack_01 == 0f && timer != 0 && enemy.adjustTimer_Attack <= 0) ||
                (timer % enemy.timePoint_Attack_02 == 0f && timer != 0 && enemy.adjustTimer_Attack <= 0))
            {
                return true;
            }
        }
        if(enemy.isAttack) return true;
        return false;
    }
}
public class Node_AttackWay_01 : NodeBase
{
    public Node_AttackWay_01()
    {
        enemy = TreeNodeMgr.I.enemy;
        player = TreeNodeMgr.I.player;
    }
    public override bool ConditionFunc()
    {
        var timer = float.Parse(enemy.timer_Attack.ToString("#0.0"));
        if (timer % enemy.timePoint_Attack_01 == 0f && timer != 0 && enemy.adjustTimer_Attack <= 0)
            return true;
        else
            return false;
    }
    public override void ActionFunc()
    {
        enemy.adjustTimer_Attack = 0.1f;
        var random = UnityEngine.Random.Range(1, 101);
        if (enemy.probability_Attack_01 * 100 >= random)
        {
            TreeNodeMgr.I.CloseAttack();
        }
    }
}
public class Node_AttackWay_02 : NodeBase
{
    public Node_AttackWay_02()
    {
        enemy = TreeNodeMgr.I.enemy;
        player = TreeNodeMgr.I.player;
    }
    public override bool ConditionFunc()
    {
        var timer = float.Parse(enemy.timer_Attack.ToString("#0.0"));
        if (timer % enemy.timePoint_Attack_02 == 0f && timer != 0 && enemy.adjustTimer_Attack <= 0)
            return true;
        else
            return false;
    }
    public override void ActionFunc()
    {
        enemy.adjustTimer_Attack = 0.1f;
        var random = UnityEngine.Random.Range(1, 101);
        if (enemy.probability_Attack_02 * 100 >= random)
        {
            TreeNodeMgr.I.CloseAttack();
        }
    }
}