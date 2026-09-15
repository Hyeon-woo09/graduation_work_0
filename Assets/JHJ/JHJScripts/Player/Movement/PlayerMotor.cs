using UnityEngine;
using JHJ.Scripts.Player.Input;

namespace JHJ.Scripts.Player.Movement
{
    /// <summary>
    /// CharacterController 기반 플레이어 이동 처리.
    /// 입력은 IPlayerInputProvider에서만 받아오고, 수치는 PlayerMovementData에서만 가져옴 -
    /// 이 클래스는 순수하게 "입력값 + 데이터 -> 실제 이동" 계산만 담당함 (SRP).
    ///
    /// 필요 조건: 같은 오브젝트에 CharacterController 컴포넌트 필요 (RequireComponent로 자동 추가됨).
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMotor : MonoBehaviour
    {
        [Header("참조")]
        [Tooltip("IPlayerInputProvider를 구현한 컴포넌트 (같은 오브젝트에 있는 PlayerInputProvider 등)")]
        [SerializeField] private MonoBehaviour inputProviderSource; // Inspector에 인터페이스를 직접 못 넣으니 MonoBehaviour로 받고 캐스팅
        [SerializeField] private PlayerMovementData movementData;

        private IPlayerInputProvider _input;
        private CharacterController _controller;

        private Vector3 _currentVelocity; // 수평 이동 속도 (가속/감속용)
        private float _verticalVelocity;  // 중력/점프용 수직 속도

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();

            _input = inputProviderSource as IPlayerInputProvider;
            if (_input == null)
                Debug.LogError("[PlayerMotor] Input Provider Source가 IPlayerInputProvider를 구현하지 않았습니다.", this);

            if (movementData == null)
                Debug.LogError("[PlayerMotor] Movement Data가 연결되지 않았습니다.", this);
        }

        private void Update()
        {
            if (_input == null || movementData == null) return;

            HandleHorizontalMovement();
            HandleVerticalMovement();

            Vector3 finalVelocity = _currentVelocity + Vector3.up * _verticalVelocity;
            _controller.Move(finalVelocity * Time.deltaTime);
        }

        private void HandleHorizontalMovement()
        {
            Vector2 moveInput = _input.MoveInput;
            Vector3 desiredDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;

            float targetSpeed = movementData.walkSpeed * (_input.SprintHeld ? movementData.sprintMultiplier : 1f);
            Vector3 targetVelocity = desiredDirection * targetSpeed;

            _currentVelocity = Vector3.Lerp(_currentVelocity, targetVelocity, movementData.acceleration * Time.deltaTime);
        }

        private void HandleVerticalMovement()
        {
            if (_controller.isGrounded)
            {
                if (_verticalVelocity < 0f)
                    _verticalVelocity = -2f; // 바닥에 붙어있게 살짝 눌러줌

                if (_input.JumpPressed)
                    _verticalVelocity = Mathf.Sqrt(movementData.jumpHeight * -2f * movementData.gravity);
            }
            else
            {
                _verticalVelocity += movementData.gravity * Time.deltaTime;
            }
        }
    }
}
