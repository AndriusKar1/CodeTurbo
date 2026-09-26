using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float cameraSmoothness;
    public float rotSmoothness;

    public Vector3 cameraOffset;
    public Vector3 rotOffset;

    public Transform vehicleTarget;

    void FixedUpdate()
    {
        FollowTarget(); 
    }

    void FollowTarget() 
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement() 
    {
        Vector3 targetPos = new Vector3();
        targetPos = vehicleTarget.TransformPoint(cameraOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, cameraSmoothness * Time.deltaTime);
    }

    void HandleRotation() 
    {
        var direction = vehicleTarget.position - transform.position;
        var rotation  =  Quaternion.LookRotation(direction + rotOffset, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSmoothness * Time.deltaTime);
    }

}
