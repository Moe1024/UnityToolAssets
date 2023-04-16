using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
/// <summary>
/// NPC ����·���༭ & �����Χ
/// </summary>
public class NPCEditor : MonoBehaviour
{
    private int moveIndex;
    public float moveSpeed = 1f;//NPC�����ƶ��ٶ�
    private bool isMoveFirstDir = true;
    public bool isOpenDrawPath = true;//������·�����ܰ�ť
    public List<Vector3> moveList=new List<Vector3>();

    public float findDistance = 5f;//NPC����������
    public GameObject player;//��Ҫ���������Ҷ���

    void OnDrawGizmos()
    {
        //����·��
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
        //��Ⱦ·������
        for (int i = 1; i < moveList.Count; i++)
        {
            if (moveList.Count < 2) break;
            Gizmos.DrawLine(moveList[i - 1], moveList[i]);
        }

        //Բ�β����Χ
        //float awareSize = 3f;
        //Handles.color = Color.cyan;
        //Handles.DrawWireDisc(transform.position, transform.up, 5f, awareSize);

        //90������β����Χ
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
        //Բ�β����Χ
        //FindDiscRange();
        //90������β����Χ
        FindArcRange();
    }
    /// <summary>
    /// ÿ֡˳�����ƶ�
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
    /// Բ�β����Χ
    /// </summary>
    void FindDiscRange()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > findDistance) return;
        Debug.Log("Find Player!");
    }
    /// <summary>
    /// 90������β����Χ
    /// </summary>
    void FindArcRange()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > findDistance) return;
        //��������Ҫ�Ƚ�����������һ��
        var angleA = player.transform.position.normalized;
        var angleB = transform.position.normalized;
        var result = Vector3.Dot(angleA, angleB);
        float radians = Mathf.Acos(result);
        float angle = radians * Mathf.Rad2Deg;
        if (angle <= 45f) Debug.Log("Find Player!");
    }
}
