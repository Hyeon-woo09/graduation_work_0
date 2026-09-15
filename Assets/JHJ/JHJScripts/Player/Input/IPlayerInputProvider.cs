using UnityEngine;

namespace JHJ.Scripts.Player.Input
{
    /// <summary>
    /// 플레이어 입력을 추상화한 인터페이스.
    /// PlayerMotor, FirstPersonLook 등은 이 인터페이스에만 의존하므로
    /// 나중에 레거시 Input -> 새 Input System으로 바꾸더라도
    /// 이 인터페이스를 구현하는 클래스 하나만 교체하면 됨 (다른 코드 전혀 안 건드림).
    /// </summary>
    public interface IPlayerInputProvider
    {
        /// <summary>WASD 등 이동 입력 (X: 좌우, Y: 전후)</summary>
        Vector2 MoveInput { get; }

        /// <summary>마우스 이동량 (X: 좌우 시점, Y: 상하 시점)</summary>
        Vector2 LookInput { get; }

        /// <summary>점프 키가 이번 프레임에 눌렸는지</summary>
        bool JumpPressed { get; }

        /// <summary>달리기 키가 눌려있는지 (누르고 있는 동안 계속 true)</summary>
        bool SprintHeld { get; }
    }
}
