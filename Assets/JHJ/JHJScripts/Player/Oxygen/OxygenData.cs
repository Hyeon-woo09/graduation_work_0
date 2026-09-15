using UnityEngine;

namespace JHJ.Scripts.Player.Oxygen
{
    /// <summary>
    /// 산소 시스템 기본 수치. 스킬트리 업그레이드는 이 값을 직접 안 건드리고
    /// OxygenModifier로 보정해서 적용함 (기본값은 항상 그대로 유지됨).
    /// 우클릭 -> Create -> Game -> Player -> Oxygen Data 로 생성.
    /// </summary>
    [CreateAssetMenu(fileName = "OxygenData", menuName = "Game/Player/Oxygen Data")]
    public class OxygenData : ScriptableObject
    {
        [Header("기본 수치")]
        public float maxOxygen = 100f;
        [Tooltip("물 속에 있을 때 초당 감소량")]
        public float depletionRatePerSecond = 5f;
        [Tooltip("물 밖에 있을 때 초당 회복량")]
        public float regenRatePerSecond = 15f;
    }
}
