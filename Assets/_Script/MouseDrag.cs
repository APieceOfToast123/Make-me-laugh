using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);

            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);

                // 考虑到摄像机的旋转
                Quaternion rotation = Quaternion.Euler(mainCamera.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);
                Vector3 localHitPoint = rotation * (hitPoint - transform.position) + transform.position;

                transform.position = localHitPoint;
            }
        }
    }
}
