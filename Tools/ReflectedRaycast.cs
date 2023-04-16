using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ᷴ�������
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class ReflectedRaycast : MonoBehaviour
{
    public int reflections = 5;//�������
    public float maxLength = 10;//������󳤶�

    private LineRenderer lineRenderer;
	private Ray ray;
	private RaycastHit hit;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		//�����һ�����ߣ������������ܵĳ���
		ray = new Ray(transform.position, transform.forward);
		lineRenderer.positionCount = 1;
		lineRenderer.SetPosition(0, transform.position);
		float remainingLength = maxLength;

		for (int i = 0; i < reflections; i++)
		{
			if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
			{
				//�������������������ݷ��߷������ߣ����������Ϊ�����ɵ�һ������
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                remainingLength -= Vector3.Distance(ray.origin, hit.point);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
            }
			else
			{
				//�������û����������һֱ����ԭ��������һֱ������ȥ
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
            }
		}
	}
}