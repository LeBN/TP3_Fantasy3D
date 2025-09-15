using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerCameraBehavior : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject target;
    [SerializeField] InputSystem_Actions inputs;
    private float zoom = 0;
    private float relPos = 0;

    private const float ZOOM_FACTOR = 1;

    private void Awake()
    {
        inputs = new InputSystem_Actions();
        
        inputs.UI.ScrollWheel.performed += ctx => updateZoom(ctx.ReadValue<Vector2>().y);

        inputs.UI.Enable();
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    void updateZoom(float zoomChange)
    {
        zoom = zoomChange * ZOOM_FACTOR;
        relPos = cam.transform.localPosition.z;
        if (relPos + zoom < -1f && relPos + zoom > -6f)
        {
            cam.transform.Translate(new Vector3(0, 0, zoomChange));
        }
    }
}
