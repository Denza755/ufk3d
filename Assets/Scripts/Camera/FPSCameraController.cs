using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem; // Добавляем поддержку новой системы ввода

namespace Unity.FantasyKingdom
{
    public class FPSCameraController : MonoBehaviour
    {
        [Header("Ссылки на компоненты")]
        public CinemachineCamera cinemachineCamera;
    
        [Header("Настройки зума")]
        public float zoomSpeed = 0.05f; 
        public float minDistance = 0f;   
        public float maxDistance = 15f;  

        [Header("Настройки вращения мыши")]
        public float mouseSensitivity = 0.5f;

        private CinemachineThirdPersonFollow thirdPersonFollow;
        private float xRotation = 0f;
        private float yRotation = 0f;

        void Start()
        {
            if (cinemachineCamera != null)
            {
                thirdPersonFollow = cinemachineCamera.GetComponent<CinemachineThirdPersonFollow>();
            }

            // Блокируем курсор мыши по центру экрана, чтобы он не вылетал из игры
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (thirdPersonFollow == null) return;

            HandleZoom();
            HandleRotation();
        }

        void HandleZoom()
        {
            float scrollInput = 0f;
            if (Mouse.current != null)
            {
                scrollInput = Mouse.current.scroll.ReadValue().y;
            }

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

            // Считываем движение мыши по осям X и Y
            float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity;
            float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -70f, 70f); // Ограничиваем взгляд вверх/вниз

            // Вращаем камеру
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }    
        
    }
}



