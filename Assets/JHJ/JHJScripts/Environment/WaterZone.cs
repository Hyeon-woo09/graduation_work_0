using UnityEngine;

namespace JHJ.Scripts.Environment
{
    /// <summary>
    /// 물 영역이라는 걸 표시만 하는 마커 컴포넌트.
    /// 실제 "산소가 닳기 시작하는" 판단은 여기서 안 하고, 플레이어 머리에 붙는
    /// PlayerHeadOxygenDetector가 이 컴포넌트를 찾아서 스스로 판단함 (머리 기준 감지).
    ///
    /// 세팅:
    /// 1. 물 영역 오브젝트(바다 에셋 등)에 Box Collider(또는 원하는 모양) 추가
    /// 2. 그 Collider의 Is Trigger 체크 ON (Add Component 시 자동으로 켜짐)
    /// 3. 이 스크립트 Add Component
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class WaterZone : MonoBehaviour
    {
        private void Reset()
        {
            var col = GetComponent<Collider>();
            if (col != null)
                col.isTrigger = true;
        }
    }
}