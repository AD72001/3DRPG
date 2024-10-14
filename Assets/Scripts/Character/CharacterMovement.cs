using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Player Movement
    public float moveSpeed;
    public float turnSpeed;
    Vector3 position;

    //Component
    [SerializeField] private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Locate the location of the click
            LocatePosition();
        }        

        // Move Characters to the location
        MoveToPosition();
    }

    void LocatePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            Debug.Log(position);
        }
    }

    void MoveToPosition()
    {
        if (Vector3.Distance(position, transform.position) > 1f)
        {
            Quaternion newRotation = Quaternion.LookRotation(position - transform.position);
            newRotation.x = 0;
            newRotation.z = 0;

            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
            controller.SimpleMove(transform.forward * moveSpeed);
        }
    }
}
