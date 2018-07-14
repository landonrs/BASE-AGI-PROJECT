using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour {

	public float speed = 6.0F;
	public float rotationSpeed = 2f;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	bool dancing;

	public static readonly int KITCHEN_X_CENTER = 17;
	public static readonly int KITCHEN_Z_CENTER = 5;
	public static readonly int BEDROOM_X_CENTER = 17;
	public static readonly int BEDROOM_Z_CENTER = -6;

	private Transform neoPosition;

	NavMeshAgent agent;

	void Start(){
		agent = GetComponent<NavMeshAgent>();
		neoPosition = GetComponent<Transform> ();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
			{
				//moveToLocation(KITCHEN_X_CENTER, KITCHEN_Z_CENTER);
				Debug.DrawRay(Camera.main.transform.position, hit.point, Color.red, 3f);
			}
		}
	}

	public void moveToLocation(int x, int z){
		agent.SetDestination (new Vector3 (x, neoPosition.localPosition.y - 1, z));
	}

	public void moveToLocation(Vector2 targetLocation){
		// the y value for the vector 2 is actually the z value
		agent.SetDestination (new Vector3 (targetLocation.x, neoPosition.localPosition.y, targetLocation.y));
	}


}
