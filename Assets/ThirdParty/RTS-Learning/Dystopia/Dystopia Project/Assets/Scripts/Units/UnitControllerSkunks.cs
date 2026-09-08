using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitControllerSkunks : MonoBehaviour
{
    [SerializeField] LayerMask clickable;

    NavMeshAgent agent;
    Camera main;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveToTarget();
        }
    }

    public void MoveToTarget()
    {
        RaycastHit hit;
        Debug.Log("Casting a ray");
        Debug.Log("Clickable layer is " + clickable.value);

        if (Physics.Raycast(main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            int collisionMask = (1 << hit.transform.gameObject.layer);

            if ((clickable.value & collisionMask) > 0)
            {
                agent.destination = hit.point;
                Debug.Log("layer collided is " + hit.transform.gameObject.layer);
            }
            else
                Debug.Log("Clicked a layer that has no navigation");
        }
    }
}
