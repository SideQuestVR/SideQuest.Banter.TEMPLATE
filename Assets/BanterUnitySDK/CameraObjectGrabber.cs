using UnityEngine;

public class CameraObjectGrabber : MonoBehaviour
{
    public float force = 600;
    public float damping = 6;
    public LayerMask grabbableLayer; // Make sure this matches your "Grabbable" layer

    private Transform jointTrans;
    private float dragDepth;
    private PlayerTriggerEvent currentGrabbedObjectEvents; // Reference to the PlayerTriggerEvent script of the currently grabbed object

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInputBegin(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleInputEnd(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && jointTrans != null)
        {
            HandleInput(Input.mousePosition);
        }
    }

    private void HandleInputBegin(Vector3 screenPosition)
    {
        var ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, grabbableLayer))
        {
            var playerTriggerEvent = hit.transform.GetComponent<PlayerTriggerEvent>();
            if (playerTriggerEvent != null)
            {
                currentGrabbedObjectEvents = playerTriggerEvent;
                // Assuming right grab for simplicity, modify as needed
                currentGrabbedObjectEvents.OnGrabRight();
            }

            dragDepth = CameraToPointDepth(Camera.main, hit.point);
            jointTrans = AttachJoint(hit.rigidbody, hit.point);
        }
    }

    private void HandleInput(Vector3 screenPosition)
    {
        var worldPos = ScreenToWorldPlanePoint(Camera.main, dragDepth, screenPosition);
        jointTrans.position = worldPos;
    }

    private void HandleInputEnd(Vector3 screenPosition)
    {
        if (jointTrans != null)
        {
            if (currentGrabbedObjectEvents != null)
            {
                // Assuming right release for simplicity, modify as needed
                currentGrabbedObjectEvents.OnReleaseRight();
                currentGrabbedObjectEvents = null; // Clear the reference
            }

            Destroy(jointTrans.gameObject);
        }
    }

    private Transform AttachJoint(Rigidbody rb, Vector3 attachmentPosition)
    {
        GameObject go = new GameObject("Attachment Point");
        go.hideFlags = HideFlags.HideInHierarchy;
        go.transform.position = attachmentPosition;

        var newRb = go.AddComponent<Rigidbody>();
        newRb.isKinematic = true;

        var joint = go.AddComponent<ConfigurableJoint>();
        joint.connectedBody = rb;
        joint.configuredInWorldSpace = true;
        joint.xDrive = NewJointDrive(force, damping);
        joint.yDrive = NewJointDrive(force, damping);
        joint.zDrive = NewJointDrive(force, damping);
        joint.slerpDrive = NewJointDrive(force, damping);
        joint.rotationDriveMode = RotationDriveMode.Slerp;

        return go.transform;
    }

    private JointDrive NewJointDrive(float force, float damping)
    {
        JointDrive drive = new JointDrive();
        drive.mode = JointDriveMode.Position;
        drive.positionSpring = force;
        drive.positionDamper = damping;
        drive.maximumForce = Mathf.Infinity;
        return drive;
    }

    // Utility Methods from CameraPlane
    public static Vector3 ScreenToWorldPlanePoint(Camera camera, float zDepth, Vector3 screenCoord)
    {
        var point = camera.ScreenToViewportPoint(screenCoord);
        Vector2 angles = ViewportPointToAngle(camera, point);
        float xOffset = Mathf.Tan(angles.x) * zDepth;
        float yOffset = Mathf.Tan(angles.y) * zDepth;
        Vector3 cameraPlanePosition = new Vector3(xOffset, yOffset, zDepth);
        cameraPlanePosition = camera.transform.TransformPoint(cameraPlanePosition);
        return cameraPlanePosition;
    }

    public static float CameraToPointDepth(Camera cam, Vector3 point)
    {
        Vector3 localPosition = cam.transform.InverseTransformPoint(point);
        return localPosition.z;
    }

    public static Vector2 ViewportPointToAngle(Camera cam, Vector2 ViewportCord)
    {
        float adjustedAngle = AngleProportion(cam.fieldOfView / 2, cam.aspect) * 2;
        float xProportion = ((ViewportCord.x - .5f) / .5f);
        float yProportion = ((ViewportCord.y - .5f) / .5f);
        float xAngle = AngleProportion(adjustedAngle / 2, xProportion) * Mathf.Deg2Rad;
        float yAngle = AngleProportion(cam.fieldOfView / 2, yProportion) * Mathf.Deg2Rad;
        return new Vector2(xAngle, yAngle);
    }

    public static float AngleProportion(float angle, float proportion)
    {
        float opposite = Mathf.Tan(angle * Mathf.Deg2Rad);
        float oppositeProportion = opposite * proportion;
        return Mathf.Atan(oppositeProportion) * Mathf.Rad2Deg;
    }
}
