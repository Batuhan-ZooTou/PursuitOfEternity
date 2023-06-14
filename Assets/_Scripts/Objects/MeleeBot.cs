using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum AIState
{
    patrol,
    follow,
    hitCube,
}
public class MeleeBot : MonoBehaviour
{
    public AIState state;
    NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] float force;
    [SerializeField] float range;
    [SerializeField] float punchTimer;
    [SerializeField] ObjectGrabable cube;
    [SerializeField] bool canPunch=true;
     Vector3 centrePoint;
    [SerializeField] Socket socket;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        centrePoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (socket.cubeInside && state!=AIState.follow)
        {
            state = AIState.hitCube;
            
        }
        switch (state)
        {
            case AIState.patrol:
                if (agent.remainingDistance <= agent.stoppingDistance) //done with path
                {
                    Vector3 point;
                    if (RandomPoint(centrePoint, range, out point)) //pass in our centre point and radius of area
                    {
                        Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                        agent.SetDestination(point);
                    }
                }
                break;
            case AIState.follow:
                agent.destination = player.position;

                break;
            case AIState.hitCube:
                agent.destination = socket.transform.position;
                if (Vector3.Distance(transform.position, socket.transform.position) < 1f)
                {
                    Debug.Log("hitcube");
                    cube.RobotHit();
                    Vector3 point;
                    if (RandomPoint(centrePoint, range, out point)) //pass in our centre point and radius of area
                    {
                        Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                        agent.SetDestination(point);
                    }
                    state = AIState.patrol;
                }
                break;
            default:
                break;
        }
        

    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!canPunch)
        {
            return;
        }
        if (other.TryGetComponent(out Rigidbody rb))
        {
            if (Vector3.Distance(transform.position, rb.transform.position) < 0.4f)
            {
                canPunch = false;
                rb.velocity = Vector3.up * force;
                Invoke(nameof(ResetPunch), punchTimer);
            }
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (state==AIState.follow)
        {
            return;
        }
        
        if (other.TryGetComponent(out PlayerController playerController))
        {
            state = AIState.follow;

        }
        else
        {
            state = AIState.patrol;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            Vector3 point;
            if (RandomPoint(centrePoint, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.SetDestination(point);
            }
            state = AIState.patrol;
        }
    }
    public void ResetPunch()
    {
        canPunch = true;
    }
}
