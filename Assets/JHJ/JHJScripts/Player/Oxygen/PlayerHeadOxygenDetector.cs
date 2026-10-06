using UnityEngine;
using JHJ.Scripts.Environment;

namespace JHJ.Scripts.Player.Oxygen
{
    /// <summary>
    /// 플레이어의 "머리" 위치에 붙는 감지 전용 컴포넌트.
    /// 몸통 전체 콜라이더가 아니라 이 오브젝트(머리)만 물(WaterZone)에 닿았을 때만
    /// 산소가 닳기 시작하도록 함. 즉 발/몸통이 물에 잠겨도 머리가 물 밖에 있으면 안전함.
    ///
    /// 세팅:
    /// 1. Player 오브젝트 자식으로 빈 오브젝트 생성 -> 이름 "Head", 머리 높이에 배치
    /// 2. Head에 Sphere Collider(작게) 추가 -> Is Trigger 체크 ON
    /// 3. 이 스크립트를 Head에 Add Component
    /// 4. Player 오브젝트(또는 최상위 부모)에 Rigidbody 하나 추가 -> Is Kinematic 체크,
    ///    Use Gravity 체크 해제 (트리거 감지가 발동하려면 계층 어딘가에 Rigidbody가 반드시 있어야 함)
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class PlayerHeadOxygenDetector : MonoBehaviour
    {
        [Tooltip("비워두면 부모 계층에서 자동으로 찾음")]
        [SerializeField] private OxygenSystem oxygenSystem;

        private void Awake()
        {
            if (oxygenSystem == null)
                oxygenSystem = GetComponentInParent<OxygenSystem>();

            if (oxygenSystem == null)
                Debug.LogError("[PlayerHeadOxygenDetector] OxygenSystem을 찾지 못했습니다. Player 부모 오브젝트에 있는지 확인하세요.", this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<WaterZone>() == null) return;
            oxygenSystem?.SetInWater(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<WaterZone>() == null) return;
            oxygenSystem?.SetInWater(false);
        }
    }
}