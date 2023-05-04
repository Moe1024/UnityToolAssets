using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBTree;

/// <summary>
/// 节点数管理者
/// </summary>
public class TreeNodeMgr : MonoBehaviour
{
    public static TreeNodeMgr I; private void Awake() { if (I != null) { Destroy(I); } I = this; }

    public Enemy enemy;
    public Player player;



    public void Ani_Walk_Front()
    {
        StartCoroutine(Ani_Walk_Front_Delay());
    }

    IEnumerator Ani_Walk_Front_Delay()
    {
        yield return new WaitForSeconds(1f);
        if (enemy.around == EnemyAround.None) enemy.animator.Play("Walk_Front");
        enemy.isMove = true;
    }

    void OnDrawGizmos()
    {
        if (enemy.around != EnemyAround.None)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(player.transform.position, enemy.transform.position);
        }
    }
    public void CloseAttack()
    {
        enemy.animator.Play("Attack");
        enemy.animator.applyRootMotion = true;
        enemy.timer_Attack = 0;
        enemy.isAttack = true;

        StartCoroutine(CloseAttack_Delay());
    }

    IEnumerator CloseAttack_Delay()
    {
        yield return new WaitForSeconds(0.7f);
        player.transform.LookAt(enemy.transform.position);
        player.animator.applyRootMotion = true;
        player.animator.Play("Heavy", 0, 0);

        yield return new WaitForSeconds(1.7f);

        enemy.animator.applyRootMotion = false;
        enemy.animator.Play("Back");


        var normal = (enemy.transform.position - player.transform.position).normalized;
        while (Vector3.Distance(enemy.transform.position, player.transform.position) < enemy.distance_Around / 7 * 6f)
        {
            yield return 0;
            enemy.transform.LookAt(player.transform.position);
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, normal* Vector3.Distance(enemy.transform.position, player.transform.position)*2, Time.deltaTime * 5f);
        }

        enemy.isAttack = false;
        if (enemy.around != EnemyAround.None) enemy.around = EnemyAround.None;
    }
}
