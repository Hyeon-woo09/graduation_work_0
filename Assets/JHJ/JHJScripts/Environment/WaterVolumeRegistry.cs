using System.Collections.Generic;

namespace JHJ.Scripts.Environment
{
    /// <summary>
    /// 씬에 존재하는 모든 WaterVolume(물 구역)을 모아두는 정적 레지스트리.
    /// PlayerWaterState는 이 목록을 순회해서 "지금 내가 어느 물 구역 안에 있는지" 찾음.
    /// InteractableRegistry와 동일한 패턴.
    /// </summary>
    public static class WaterVolumeRegistry
    {
        private static readonly List<WaterVolume> _all = new List<WaterVolume>();

        public static IReadOnlyList<WaterVolume> All => _all;

        public static void Register(WaterVolume volume)
        {
            if (volume == null || _all.Contains(volume)) return;
            _all.Add(volume);
        }

        public static void Unregister(WaterVolume volume)
        {
            if (volume == null) return;
            _all.Remove(volume);
        }
    }
}
