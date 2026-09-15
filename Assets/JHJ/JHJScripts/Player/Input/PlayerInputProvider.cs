using UnityEngine;

namespace JHJ.Scripts.Player.Input
{
    /// <summary>
    /// 레거시 Input Manager(Input.GetAxis)를 사용한 IPlayerInputProvider 구현체.
    /// 별도 세팅(Input Actions 에셋, 바인딩 등) 없이 바로 작동함.
    ///
    /// 세팅: 플레이어 오브젝트에 그냥 Add Component 하면 끝.
    /// 유니티 프로젝트 기본 Input Manager에 이미 Horizontal/Vertical/Mouse X/Mouse Y/Jump 축이
    /// 기본 내장되어 있어서 추가 설정이 필요 없음 (Edit -> Project Settings -> Input Manager에서 확인 가능).
    /// </summary>
    public class PlayerInputProvider : MonoBehaviour, IPlayerInputProvider
    {
        [Header("키 설정")]
        [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool SprintHeld { get; private set; }

        private void Update()
        {
            MoveInput = new Vector2(
                UnityEngine.Input.GetAxisRaw("Horizontal"),
                UnityEngine.Input.GetAxisRaw("Vertical")
            );

            LookInput = new Vector2(
                UnityEngine.Input.GetAxis("Mouse X"),
                UnityEngine.Input.GetAxis("Mouse Y")
            );

            JumpPressed = UnityEngine.Input.GetButtonDown("Jump");
            SprintHeld = UnityEngine.Input.GetKey(sprintKey);
        }
    }
}
