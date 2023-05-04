using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIBehaviourTree : MonoBehaviour
{
    public static AIBehaviourTree I; private void Awake() { if (I != null) { Destroy(I); } I = this; }

    public NodeBase rootNode;

    void Start()
    {
        CreatNodes();

        //Do(rootNode);
    }
    void Update()
    {
        Do(rootNode);
    }

    void CreatNodes()
    {
        //创造子节点
        NodeBase childNode = new Node_Posture();
        //添加子节点
        rootNode.childs.Add(childNode);

        NodeBase childNode_02 = new Node_Around();
        childNode.childs.Add(childNode_02);

        childNode = new Node_Attack();
        childNode_02.childs.Add(childNode);

        childNode_02= new Node_AttackWay_01();
        NodeBase childNode_03 = new Node_AttackWay_02();
        childNode.childs.Add(childNode_02);
        childNode.childs.Add(childNode_03);


    }


    /// <summary>
    /// 执行节点
    /// </summary>
    /// <param name="node"></param>
    public void Do(NodeBase node)
    {
        if (node.childs.Count > 0)
        {
            foreach (var o in node.childs)
                if (o.condition())
                {
                    Do(o);
                    return;
                }
            if (!node.isDone) node.action();
            node.update();
        }
        else
        {
            if (!node.isDone)
                node.action();
            node.update();
        }
    }
}

