using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public float boostMultiplier = 2f;

    Camera mainCamera;
    float yaw = 0f;
    float pitch = 0f;
    public GameObject cameraInfoCanvas; 
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.None; // 시작 시는 마우스 자유
    }

    void Update()
    {
        // 우클릭 눌렀을 때만 자유시점 진입
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            cameraInfoCanvas.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            cameraInfoCanvas.SetActive(false);
        }

        // 우클릭 상태에서만 카메라 회전/이동
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // 마우스 이동으로 회전
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            mainCamera.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

            // 키보드로 이동
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            float moveY = 0f;

            if (Input.GetKey(KeyCode.E)) moveY += 1f;
            if (Input.GetKey(KeyCode.Q)) moveY -= 1f;

            Vector3 move = mainCamera.transform.right * moveX + mainCamera.transform.up * moveY + mainCamera.transform.forward * moveZ;

            float speed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
                speed *= boostMultiplier;

            mainCamera.transform.position += move * speed * Time.deltaTime;
        }
    }
}
