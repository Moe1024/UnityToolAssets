using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
/// <summary>
/// NPC 行走路径编辑 & 察觉范围
/// </summary>
public class NPCEditor : MonoBehaviour
{
    private int moveIndex;
    public float moveSpeed = 1f;//NPC自身移动速度
    private bool isMoveFirstDir = true;
    public bool isOpenDrawPath = true;//开启画路径功能按钮
    public List<Vector3> moveList=new List<Vector3>();

    public float findDistance = 5f;//NPC自身察觉距离
    public GameObject player;//需要被察觉的玩家对象

    void OnDrawGizmos()
    {
        //绘制路径
        if (Event.current.type == EventType.MouseUp && isOpenDrawPath)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var pos = hit.point;
                pos.y += 0.1f;
                moveList.Add(pos);
            }
        }
        //渲染路径线条
        for (int i = 1; i < moveList.Count; i++)
        {
            if (moveList.Count < 2) break;
            Gizmos.DrawLine(moveList[i - 1], moveList[i]);
        }

        //圆形察觉范围
        //float awareSize = 3f;
        //Handles.color = Color.cyan;
        //Handles.DrawWireDisc(transform.position, transform.up, 5f, awareSize);

        //90°角扇形察觉范围
        Handles.color = Color.cyan;
        Handles.DrawWireArc(transform.position, transform.up, transform.forward - transform.right, 90f, findDistance);
    }
    private void Start()
    {
        transform.position = moveList[0];
        moveIndex = 0;
    }
    private void Update()
    {
        Move();
        //圆形察觉范围
        //FindDiscRange();
        //90°角扇形察觉范围
        FindArcRange();
    }
    /// <summary>
    /// 每帧顺着线移动
    /// </summary>
    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveList[moveIndex], Time.deltaTime * moveSpeed);
        if (transform.position == moveList[moveIndex])
        {
            if (moveIndex + 1 > moveList.Count - 1) isMoveFirstDir = false;
            else if (moveIndex - 1 < 0) isMoveFirstDir = true;
            if (isMoveFirstDir)
                moveIndex++;
            else
                moveIndex--;
            transform.LookAt(moveList[moveIndex]);
        }
    }

    /// <summary>
    /// 圆形察觉范围
    /// </summary>
    void FindDiscRange()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > findDistance) return;
        Debug.Log("Find Player!");
    }
    /// <summary>
    /// 90°角扇形察觉范围
    /// </summary>
    void FindArcRange()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > findDistance) return;
        //计算点乘需要先将俩向量都归一化
        var angleA = player.transform.position.normalized;
        var angleB = transform.position.normalized;
        var result = Vector3.Dot(angleA, angleB);
        float radians = Mathf.Acos(result);
        float angle = radians * Mathf.Rad2Deg;
        if (angle <= 45f) Debug.Log("Find Player!");
    }
}
