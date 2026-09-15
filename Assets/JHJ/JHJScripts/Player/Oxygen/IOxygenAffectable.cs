namespace JHJ.Scripts.Player.Oxygen
{
    /// <summary>
    /// "산소 시스템의 영향을 받는 대상"이라는 계약.
    /// WaterZone은 이 인터페이스만 보고 물에 들어온 대상에게 알려주므로,
    /// 나중에 플레이어 말고 다른 것(수중 몬스터 등)에도 재사용 가능.
    /// 태그/레이어 대신 인터페이스로 판단해서, 태그 설정 실수로 안 먹는 문제를 원천 차단함.
    /// </summary>
    public interface IOxygenAffectable
    {
        void SetInWater(bool isInWater);
    }
}
