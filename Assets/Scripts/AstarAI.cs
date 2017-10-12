using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarAI : MonoBehaviour {
    [SerializeField]
    private Transform target;

    private Vector3 currentTargetPos;

    private Seeker seeker;

    private Path path;

    private CharacterController controller;

    private int currentWaypointIndex;
    
    [SerializeField]
    private float startNextWaypointDistance;

    [SerializeField]
    private float moveSpeed;

	// Use this for initialization
	void Start () {
		if(target)
        {
            currentTargetPos = target.position;
        }
        seeker = GetComponent<Seeker>();
        if(seeker)
        {
            seeker.StartPath(transform.position, currentTargetPos, OnPathComplete);
        }
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(target && target.position != currentTargetPos)
        {
            currentTargetPos = target.position;
            if(seeker && seeker.IsDone())
            {
                seeker.StartPath(transform.position, currentTargetPos, OnPathComplete);
            }
        }

        if(path != null && controller && currentWaypointIndex < path.vectorPath.Count)
        {
            Vector3 dir = (path.vectorPath[currentWaypointIndex] - transform.position).normalized;
            controller.SimpleMove(dir * moveSpeed);
            if ((transform.position - path.vectorPath[currentWaypointIndex]).sqrMagnitude <= startNextWaypointDistance * startNextWaypointDistance)
            {
                currentWaypointIndex++;
            }
        }
	}

    private void OnPathComplete(Path path)
    {
        if(!path.error)
        {
            this.path = path;
            currentWaypointIndex = 0;
        }
    }
}
