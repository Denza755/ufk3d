using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem; // Добавляем поддержку новой системы ввода

namespace Unity.FantasyKingdom
{
    public class FPSCameraController : MonoBehaviour
    {
        [Header("Ссылки на компоненты")]
        public CinemachineCamera cinemachineCamera;
        public Transform playerTransform; // Сюда перетащите PT_Boy_Modular_Free_Pack (1)
        
        [Header("Настройки зума")]
        public float zoomSpeed = 0.05f; 
        public float minDistance = 0f;   
        public float maxDistance = 15f;  

        [Header("Настройки вращения мыши")]
        public float mouseSensitivity = 0.5f;

        private CinemachineThirdPersonFollow thirdPersonFollow;
        private float xRotation = 0f;
        private float yRotation = 0f;
        private Vector3 headOffset = new Vector3(0f, 1.5f, 0f); // Смещение на уровень глаз

        void Start()
        {
            // Находим мальчика на сцене автоматически, если забыли привязать вручную
            if (playerTransform == null)
            {
                GameObject player = GameObject.Find("PT_Boy_Modular_Free_Pack (1)");
                if (player != null) playerTransform = player.transform;
            }

            if (cinemachineCamera != null)
            {
                thirdPersonFollow = cinemachineCamera.GetComponent<CinemachineThirdPersonFollow>();
            }

            // Блокируем курсор
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            // Системный хак: принудительно удерживаем якорь на уровне глаз мальчика в каждом кадре,
            // перезаписывая любые баги стандартных скриптов сцены
            if (playerTransform != null)
            {
                transform.position = playerTransform.position + headOffset;
            }

            if (thirdPersonFollow == null) return;

            HandleZoom();
            HandleRotation();
        }

        void HandleZoom()
        {
            float scrollInput = 0f;
            if (Mouse.current != null) scrollInput = Mouse.current.scroll.ReadValue().y;

            if (scrollInput != 0)
            {
                float delta = Mathf.Sign(scrollInput) * zoomSpeed;
                float newDistance = thirdPersonFollow.CameraDistance - delta;
                thirdPersonFollow.CameraDistance = Mathf.Clamp(newDistance, minDistance, maxDistance);
            }
        }

        void HandleRotation()
        {
            if (Mouse.current == null) return;

            float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity;
            float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -70f, 70f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    
    
    }
}



