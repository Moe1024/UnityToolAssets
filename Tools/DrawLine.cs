using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Draw
{
    /// <summary>
    /// 画线工具
    /// </summary>
    public class DrawLine : MonoBehaviour
    {
        public GameObject brush;//笔刷

        public float drawOffset = 0.1f;//笔触点之间的偏移

        LineRenderer currentLineRenderer;
        GameObject brushInstance;
        Vector2 lastPoint;
        Ray ray;
        RaycastHit hit;


        void Update()
        {
            Drawing();
        }

        public void Drawing()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Destroy(brushInstance);
                CreateBrush();
            }
            else if (Input.GetMouseButton(0))
            {
                PointToMousePos();
            }
            if (Input.GetMouseButtonUp(0))
            {
                currentLineRenderer = null;
            }
        }

        void CreateBrush()//创造画笔
        {

            brushInstance = Instantiate(brush);
            currentLineRenderer = brushInstance.GetComponent<LineRenderer>();


        }

        void AddAPoint(Vector3 pointPos)//陆续增加绘画笔触点
        {
            currentLineRenderer.positionCount++;
            int positionIndex = currentLineRenderer.positionCount - 1;
            currentLineRenderer.SetPosition(positionIndex, pointPos);

        }

        void PointToMousePos()//计算鼠标与作画的关系
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 tmepPoint = Input.mousePosition;

            if (lastPoint != null)
            {
                if (Vector2.Distance(tmepPoint, lastPoint) >= drawOffset)
                    lastPoint = tmepPoint;
                else
                    return;
            }
            else
            {
                lastPoint = tmepPoint;
            }

            if (Physics.Raycast(ray, out hit))//画笔只要碰到任何的3D碰撞体都会画线
            {
                Vector3 mousePos = hit.point;
                AddAPoint(mousePos);
            }

        }
    }
}
