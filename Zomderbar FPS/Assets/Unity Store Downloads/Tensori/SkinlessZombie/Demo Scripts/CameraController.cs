using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]private float cameraSpeed = 1.5f;
    [SerializeField]private float rotationSpeed = 30f;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private Transform target;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {

        float yValue = Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime;
         transform.position = new Vector3(transform.position.x, transform.position.y + yValue, transform.position.z);

        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(target.position, -Vector3.up, rotationSpeed * Time.deltaTime);
        }

        float fieldOfview = mainCam.fieldOfView;
        fieldOfview += (Input.GetAxis("Mouse ScrollWheel") * zoomSpeed) * -1;
        mainCam.fieldOfView = fieldOfview;

    }
}
