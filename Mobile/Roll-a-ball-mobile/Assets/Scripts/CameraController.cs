using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;
    private Vector3 newOf;

    void Start ()
	{
        newOf = player.transform.position;
        offset = transform.position;
		offset = transform.position - player.transform.position;
	}

	void LateUpdate ()
	{
     
        newOf.y = player.transform.position.y;
        transform.position = newOf + offset;
	}
}