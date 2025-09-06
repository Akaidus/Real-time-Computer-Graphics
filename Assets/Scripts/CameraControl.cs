using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] GameObject targetToLookAt;
    [SerializeField] new Camera camera;
    [SerializeField] Vector3 position  = new(0, 15, -32);
    [SerializeField] Vector3 rotation = new(27, 0, 0);
    [Header("Rotation")]
    [SerializeField] float mouseDragSpeed = 0.5f;
    [SerializeField] float inertiaDamping = 0.5f;
    float inertiaTime;
    [Header("Auto Rotate")]
    [SerializeField] bool autoRotate = false;
    [SerializeField] Vector3 autoRotateSpeed = new(1, 1, 1);
    bool isDragging = false;
    Vector3 lastPosition;
    Vector3 lastRotation;
    Vector2 rotationInertia;

    
    void Awake()
    {
        lastPosition = position;
        lastRotation = rotation;
    }

    private void OnEnable()
    {
        playerInput.actions["Mouse Down"].started += ctx => isDragging = true;
        playerInput.actions["Mouse Down"].canceled += ctx => { isDragging = false; inertiaTime = 1f; };
        playerInput.actions["Look"].started += ctx => MouseDrag(ctx);
    }

    void MouseDrag(InputAction.CallbackContext ctx)
    {
        if (autoRotate) return;
        if (!isDragging) return;

        var mouseX = ctx.ReadValue<Vector2>().x;
        var mouseY = ctx.ReadValue<Vector2>().y;

        rotationInertia = new Vector2(mouseX, mouseY);

        // Rotate around targetToLookAt based on mouseX and mouseY relative to camera's vectors.
        camera.transform.RotateAround(targetToLookAt.transform.position, camera.transform.up, mouseX * mouseDragSpeed);
        camera.transform.RotateAround(targetToLookAt.transform.position, camera.transform.right, -mouseY * mouseDragSpeed);

        lastPosition = camera.transform.position;
        lastRotation = camera.transform.eulerAngles;
    }

    void Update()
    {
        if (isDragging) return;

        if(inertiaTime > 0)
        {
            camera.transform.RotateAround(targetToLookAt.transform.position, camera.transform.up, rotationInertia.x * mouseDragSpeed * inertiaTime);
            camera.transform.RotateAround(targetToLookAt.transform.position, camera.transform.right, -rotationInertia.y * mouseDragSpeed * inertiaTime);

            lastPosition = camera.transform.position;
            lastRotation = camera.transform.eulerAngles;

            inertiaTime -= Time.deltaTime * inertiaDamping;
        }

        if (autoRotate)
        {
            inertiaTime = 0;
            camera.transform.RotateAround(targetToLookAt.transform.position, camera.transform.up, autoRotateSpeed[0] * Time.deltaTime);
            camera.transform.RotateAround(targetToLookAt.transform.position, camera.transform.right, autoRotateSpeed[1] * Time.deltaTime);
            camera.transform.RotateAround(targetToLookAt.transform.position, camera.transform.forward, autoRotateSpeed[2] * Time.deltaTime);
            lastPosition = camera.transform.position;
            lastRotation = camera.transform.eulerAngles;
        }
        if (autoRotate) return;
        camera.transform.position = lastPosition;
        camera.transform.eulerAngles = lastRotation;

    }
}
