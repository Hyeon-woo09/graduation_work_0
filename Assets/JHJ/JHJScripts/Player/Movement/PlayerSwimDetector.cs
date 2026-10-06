using UnityEngine;
using JHJ.Scripts.Environment;

namespace JHJ.Scripts.Player.Movement
{
    /// <summary>
    /// 플레이어 몸통(CharacterController)이 물에 들어왔는지 감지해서
    /// PlayerMotor의 수영 모드를 켜고 끄는 컴포넌트.
    ///
    /// 머리 전용 산소 감지(PlayerHeadOxygenDetector)와는 별개임 -
    /// 몸이 물에 잠기면 수영 모드로 전환되고, 머리까지 잠겨야 산소가 닳기 시작함.
    ///
    /// 세팅: Player 루트 오브젝트(CharacterController 있는 곳)에 Add Component 하면 끝.
    /// 같은 오브젝트의 CharacterController 콜라이더가 그대로 감지용으로 쓰임.
    /// (씬에 Rigidbody가 하나 있어야 트리거가 발동함 - 산소 시스템 세팅 때 추가한 Kinematic Rigidbody 재사용)
    /// </summary>
    public class PlayerSwimDetector : MonoBehaviour
    {
        [Tooltip("비워두면 같은 오브젝트에서 자동으로 찾음")]
        [SerializeField] private PlayerMotor playerMotor;

        private void Awake()
        {
            if (playerMotor == null)
                playerMotor = GetComponent<PlayerMotor>();

            if (playerMotor == null)
                Debug.LogError("[PlayerSwimDetector] PlayerMotor를 찾지 못했습니다.", this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<WaterZone>() == null) return;
            playerMotor?.SetSwimming(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<WaterZone>() == null) return;
            playerMotor?.SetSwimming(false);
        }
    }
}