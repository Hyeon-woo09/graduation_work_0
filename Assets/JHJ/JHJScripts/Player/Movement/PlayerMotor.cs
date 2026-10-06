using UnityEngine;
using JHJ.Scripts.Player.Input;

namespace JHJ.Scripts.Player.Movement
{
    /// <summary>
    /// CharacterController 기반 플레이어 이동 처리.
    /// 평소엔 걷기/뛰기/점프, 수영 모드일 땐 3D 자유 이동(Ascend/Descend 포함)으로 전환됨.
    /// 수영 모드 전환은 외부(PlayerSwimDetector)에서 SetSwimming()을 호출해서 제어함 -
    /// 이 클래스는 "지금 수영 중인가"만 알면 되고, 물 감지 로직은 전혀 모름 (책임 분리).
    ///
    /// 필요 조건: 같은 오브젝트에 CharacterController 컴포넌트 필요 (RequireComponent로 자동 추가됨).
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMotor : MonoBehaviour
    {
        [Header("참조")]
        [Tooltip("IPlayerInputProvider를 구현한 컴포넌트 (같은 오브젝트에 있는 PlayerInputProvider 등)")]
        [SerializeField] private MonoBehaviour inputProviderSource;
        [SerializeField] private PlayerMovementData movementData;

        private IPlayerInputProvider _input;
        private CharacterController _controller;

        private Vector3 _currentVelocity; // 수평 이동 속도 (가속/감속용)
        private float _verticalVelocity;  // 중력/점프/수영 상하이동용 수직 속도
        private bool _isSwimming;

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

            if (_isSwimming)
                HandleSwimmingMovement();
            else
                HandleGroundMovement();

            Vector3 finalVelocity = _currentVelocity + Vector3.up * _verticalVelocity;
            _controller.Move(finalVelocity * Time.deltaTime);
        }

        /// <summary>PlayerSwimDetector가 물에 들어오거나 나갈 때 호출함</summary>
        public void SetSwimming(bool isSwimming)
        {
            if (_isSwimming == isSwimming) return;

            _isSwimming = isSwimming;

            if (isSwimming)
                _verticalVelocity = 0f; // 물에 들어가는 순간 낙하 속도 초기화 (텀벙 안 튀게)
        }

        private void HandleGroundMovement()
        {
            Vector2 moveInput = _input.MoveInput;
            Vector3 desiredDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;

            float targetSpeed = movementData.walkSpeed * (_input.SprintHeld ? movementData.sprintMultiplier : 1f);
            Vector3 targetVelocity = desiredDirection * targetSpeed;

            _currentVelocity = Vector3.Lerp(_currentVelocity, targetVelocity, movementData.acceleration * Time.deltaTime);

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

        private void HandleSwimmingMovement()
        {
            Vector2 moveInput = _input.MoveInput;
            Vector3 horizontalDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
            Vector3 targetHorizontal = horizontalDirection * movementData.swimSpeed;

            _currentVelocity = Vector3.Lerp(_currentVelocity, targetHorizontal, movementData.acceleration * Time.deltaTime);

            float verticalInput = 0f;
            if (_input.AscendHeld) verticalInput += 1f;
            if (_input.DescendHeld) verticalInput -= 1f;

            float targetVertical = verticalInput * movementData.swimVerticalSpeed;
            _verticalVelocity = Mathf.Lerp(_verticalVelocity, targetVertical, movementData.acceleration * Time.deltaTime);
        }
    }
}