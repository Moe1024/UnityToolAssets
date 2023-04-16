using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 会反射的射线
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class ReflectedRaycast : MonoBehaviour
{
    public int reflections = 5;//反射次数
    public float maxLength = 10;//射线最大长度

    private LineRenderer lineRenderer;
	private Ray ray;
	private RaycastHit hit;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		//创造第一根射线，且设置射线总的长度
		ray = new Ray(transform.position, transform.forward);
		lineRenderer.positionCount = 1;
		lineRenderer.SetPosition(0, transform.position);
		float remainingLength = maxLength;

		for (int i = 0; i < reflections; i++)
		{
			if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
			{
				//如果射线碰到物体则根据法线反射射线，反射的射线为再生成的一条射线
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                remainingLength -= Vector3.Distance(ray.origin, hit.point);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
            }
			else
			{
				//如果射线没碰到物体则一直按照原来的射线一直延申下去
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
            }
		}
	}
}