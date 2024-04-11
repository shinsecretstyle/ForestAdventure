using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public Transform Target;

    public Vector3 offset = new Vector3(0, 2, -3);
    float y = 0;

    public float rotationSpeed = 3.0f;
    public float distance = 5.0f;

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        currentX = Mouse.current.delta.x.ReadValue();
        currentY = Mouse.current.delta.y.ReadValue();
        currentY = Mathf.Clamp(currentY, -60, 60);


        transform.position = Target.position + offset;

        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = Target.position + offset;
        transform.rotation = rotation;
        //transform.position = Target.position + rotation * direction;
        //transform.LookAt(Target.position);
    }
}
