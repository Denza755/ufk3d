using UnityEngine;
using UnityEngine.InputSystem;

namespace Unity.FantasyKingdom

{
    public class PlayerWASDController : MonoBehaviour
    {
        [Header("Настройки движения")]
        public float moveSpeed = 5f;
        public float gravity = 9.81f;

        [Header("Ссылки")]
        public Transform cameraTransform; // Сюда перетащим Main Camera

        private CharacterController characterController;
        private Vector2 moveInput;
        private Vector3 velocity;
        private PlayerInput playerInput;
/*
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            
            // Если камера не привязана вручную, находим главную камеру сцены
            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }
*/        
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();

            // Принудительно включаем карту действий Player при старте
            if (playerInput != null && playerInput.actions != null)
            {
                playerInput.actions.FindActionMap("Player")?.Enable();
            }
        
            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        void Update()
        {
            MovePlayer();
        }

        // Этот метод автоматически вызывается компонентом Player Input при нажатии WASD
        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
            
            Debug.Log($"OnMove вызван: {moveInput}"); // <-- Добавьте это            
            
        }

        void MovePlayer()
        {
            // Направление движения относительно разворота камеры
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Обнуляем Y, чтобы персонаж не взлетал при взгляде камеры вверх
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            // Считаем итоговый вектор направления
            Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;

            // Двигаем персонажа
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Поворачиваем персонажа лицом в сторону движения, если он идет
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }

            // Применяем простую гравитацию, чтобы персонаж не парил над землей
            if (characterController.isGrounded)
            {
                velocity.y = -0.5f;
            }
            else
            {
                velocity.y -= gravity * Time.deltaTime;
            }
            
            characterController.Move(velocity * Time.deltaTime);
        }

    }
}
