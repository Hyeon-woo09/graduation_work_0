using System;
using System.Collections.Generic;
using UnityEngine;

namespace JHJ.Scripts.Player.Oxygen
{
    /// <summary>
    /// 플레이어의 산소 수치를 관리하는 컴포넌트.
    /// 물(머리)에 들어가면 감소하고, 물 밖으로 나오는 즉시 100% 회복됨.
    /// 기본 수치는 OxygenData에서 가져오고, 스킬트리 업그레이드 등은
    /// AddModifier/RemoveModifier로 나중에 얼마든지 끼워넣을 수 있음.
    /// </summary>
    public class OxygenSystem : MonoBehaviour, IOxygenAffectable
    {
        [Header("기본 데이터")]
        [SerializeField] private OxygenData data;

        public event Action<float, float> OnOxygenChanged; // (현재값, 최대값)
        public event Action OnOxygenDepleted;               // 산소 0이 됐을 때 (익사 데미지 등은 추후 여기 연결)
        public event Action OnEnterWater;
        public event Action OnExitWater;

        public float CurrentOxygen { get; private set; }
        public float EffectiveMaxOxygen { get; private set; }
        public bool IsInWater { get; private set; }

        private readonly Dictionary<string, OxygenModifier> _modifiers = new Dictionary<string, OxygenModifier>();
        private bool _depletedEventFired;

        private void Awake()
        {
            RecalculateStats();
            CurrentOxygen = EffectiveMaxOxygen;
        }

        private void Update()
        {
            if (data == null || !IsInWater) return;

            float previousOxygen = CurrentOxygen;

            float depletionRate = data.depletionRatePerSecond * GetDepletionMultiplier();
            CurrentOxygen -= depletionRate * Time.deltaTime;
            CurrentOxygen = Mathf.Clamp(CurrentOxygen, 0f, EffectiveMaxOxygen);

            if (!Mathf.Approximately(previousOxygen, CurrentOxygen))
                OnOxygenChanged?.Invoke(CurrentOxygen, EffectiveMaxOxygen);

            if (CurrentOxygen <= 0f && !_depletedEventFired)
            {
                _depletedEventFired = true;
                OnOxygenDepleted?.Invoke();
            }
        }

        /// <summary>WaterZone 감지 컴포넌트가 플레이어 머리가 물에 들어오거나 나갈 때 호출함</summary>
        public void SetInWater(bool isInWater)
        {
            if (IsInWater == isInWater) return;

            IsInWater = isInWater;

            if (isInWater)
            {
                OnEnterWater?.Invoke();
            }
            else
            {
                // 물 밖으로 나오는 즉시 전량 회복
                CurrentOxygen = EffectiveMaxOxygen;
                _depletedEventFired = false;
                OnOxygenChanged?.Invoke(CurrentOxygen, EffectiveMaxOxygen);
                OnExitWater?.Invoke();
            }
        }

        /// <summary>
        /// 보정치 추가 (스킬트리 업그레이드용). 같은 id로 다시 호출하면 기존 걸 교체함.
        /// 예: oxygenSystem.AddModifier(new OxygenModifier("skill_oxygen_1", OxygenModifierType.MaxOxygenFlat, 30f));
        /// </summary>
        public void AddModifier(OxygenModifier modifier)
        {
            _modifiers[modifier.Id] = modifier;
            RecalculateStats();
        }

        /// <summary>보정치 제거 (스킬 초기화 등에 사용)</summary>
        public void RemoveModifier(string id)
        {
            if (_modifiers.Remove(id))
                RecalculateStats();
        }

        private void RecalculateStats()
        {
            if (data == null) return;

            float maxOxygenBonus = 0f;
            foreach (var modifier in _modifiers.Values)
            {
                if (modifier.Type == OxygenModifierType.MaxOxygenFlat)
                    maxOxygenBonus += modifier.Value;
            }

            EffectiveMaxOxygen = data.maxOxygen + maxOxygenBonus;
            CurrentOxygen = Mathf.Min(CurrentOxygen, EffectiveMaxOxygen);
        }

        private float GetDepletionMultiplier()
        {
            float multiplier = 1f;
            foreach (var modifier in _modifiers.Values)
            {
                if (modifier.Type == OxygenModifierType.DepletionRateMultiplier)
                    multiplier *= modifier.Value;
            }
            return multiplier;
        }
    }
}