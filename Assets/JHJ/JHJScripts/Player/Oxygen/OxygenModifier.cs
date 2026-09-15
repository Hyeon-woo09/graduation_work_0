namespace JHJ.Scripts.Player.Oxygen
{
    /// <summary>
    /// 산소 수치에 적용되는 보정치 하나. 스킬트리의 "산소 업그레이드" 등이
    /// 이 클래스를 만들어서 OxygenSystem.AddModifier()로 꽂아넣으면 됨.
    /// id는 나중에 같은 스킬을 다시 찍거나 해제할 때 식별용으로 씀.
    /// </summary>
    public enum OxygenModifierType
    {
        /// <summary>최대 산소량에 고정값 더하기 (예: +30)</summary>
        MaxOxygenFlat,

        /// <summary>산소 감소 속도에 곱하기 (예: 0.8 = 20% 덜 닳음)</summary>
        DepletionRateMultiplier,

        /// <summary>산소 회복 속도에 고정값 더하기</summary>
        RegenRateFlat,
    }

    public class OxygenModifier
    {
        public string Id { get; }
        public OxygenModifierType Type { get; }
        public float Value { get; }

        public OxygenModifier(string id, OxygenModifierType type, float value)
        {
            Id = id;
            Type = type;
            Value = value;
        }
    }
}
