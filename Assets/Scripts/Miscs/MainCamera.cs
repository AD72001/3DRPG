using UnityEngine;

public class MainCamera : MonoBehaviour
{
	public Transform target;

	private Vector3 velocity = Vector3.zero;

	public float smoothSpeed = 0.125f;
	public Vector3 offset;

	void LateUpdate ()
	{
		Vector3 desiredPosition = target.position + offset;
		Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed*Time.deltaTime);
		transform.position = smoothedPosition;

		transform.LookAt(target);
	}

}
