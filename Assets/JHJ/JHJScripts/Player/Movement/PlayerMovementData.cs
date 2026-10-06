using UnityEngine;

namespace JHJ.Scripts.Player.Movement
{
    /// <summary>
    /// 플레이어 이동/카메라 관련 수치를 전부 모아둔 SO.
    /// 코드 수정 없이 에디터에서 값 조정 가능 (기획자도 직접 튜닝 가능).
    /// 우클릭 -> Create -> Game -> Player -> Movement Data 로 생성.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Game/Player/Movement Data")]
    public class PlayerMovementData : ScriptableObject
    {
        [Header("이동")]
        public float walkSpeed = 4f;
        public float sprintMultiplier = 1.6f;
        [Tooltip("목표 속도까지 도달하는 데 걸리는 시간 느낌 (작을수록 즉각 반응)")]
        public float acceleration = 12f;

        [Header("점프 / 중력")]
        public float jumpHeight = 1.2f;
        public float gravity = -20f;

        [Header("수영")]
        public float swimSpeed = 2.5f;
        public float swimVerticalSpeed = 2f;

        [Header("시점 회전 (마우스 감도)")]
        public float lookSensitivity = 2f;
        [Tooltip("카메라가 위/아래로 꺾을 수 있는 최대 각도")]
        public float maxLookAngle = 85f;
    }
}