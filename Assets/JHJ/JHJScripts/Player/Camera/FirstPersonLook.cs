using UnityEngine;
using JHJ.Scripts.Player.Input;
using JHJ.Scripts.Player.Movement;

namespace JHJ.Scripts.Player.Camera
{
    /// <summary>
    /// 1인칭 마우스 시점 처리.
    /// 좌우 회전은 플레이어 몸통(transform) 자체를 돌리고,
    /// 상하 회전은 카메라(자식 오브젝트)만 돌림 - 표준 FPS 카메라 패턴.
    ///
    /// 세팅:
    /// 1. 플레이어 루트 오브젝트에 이 스크립트 Add Component
    /// 2. Camera Pivot 필드에 실제 Camera가 붙어있는 자식 트랜스폼 연결
    ///    (플레이어 루트 자체가 아니라, 눈높이에 있는 자식 오브젝트)
    /// </summary>
    public class FirstPersonLook : MonoBehaviour
    {
        [Header("참조")]
        [Tooltip("IPlayerInputProvider를 구현한 컴포넌트")]
        [SerializeField] private MonoBehaviour inputProviderSource;
        [SerializeField] private PlayerMovementData movementData;
        [Tooltip("카메라가 실제로 붙어있는 자식 트랜스폼 (상하 회전 담당)")]
        [SerializeField] private Transform cameraPivot;

        private IPlayerInputProvider _input;
        private float _pitch; // 카메라 상하 각도 누적값

        private void Awake()
        {
            _input = inputProviderSource as IPlayerInputProvider;
            if (_input == null)
                Debug.LogError("[FirstPersonLook] Input Provider Source가 IPlayerInputProvider를 구현하지 않았습니다.", this);

            if (cameraPivot == null)
                Debug.LogError("[FirstPersonLook] Camera Pivot이 연결되지 않았습니다.", this);
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (_input == null || movementData == null || cameraPivot == null) return;

            Vector2 look = _input.LookInput * movementData.lookSensitivity;

            // 좌우: 몸통 전체를 Y축으로 회전
            transform.Rotate(Vector3.up * look.x);

            // 상하: 카메라만 X축으로 회전 (각도 제한 있음)
            _pitch -= look.y;
            _pitch = Mathf.Clamp(_pitch, -movementData.maxLookAngle, movementData.maxLookAngle);
            cameraPivot.localEulerAngles = new Vector3(_pitch, 0f, 0f);
        }
    }
}
