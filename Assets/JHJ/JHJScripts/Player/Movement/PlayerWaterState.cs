using UnityEngine;
using JHJ.Scripts.Environment;
using JHJ.Scripts.Player.Oxygen;

namespace JHJ.Scripts.Player.Movement
{
    /// <summary>
    /// 트리거/콜라이더 충돌이 아니라 "좌표 비교"로 물 상태를 판단하는 컴포넌트.
    /// 매 프레임: 1) 내 XZ 위치가 어느 WaterVolume 범위 안에 있는지 찾고,
    ///           2) 그 구역의 수면 높이와 머리/몸통 Y좌표를 비교함.
    /// 바닥 깊이, 지형 굴곡, 물 구역 개수와 완전히 무관하게 안정적으로 작동함.
    ///
    /// 세팅: Player 루트 오브젝트에 Add Component.
    /// Head Transform에 머리 위치(기존 Head 오브젝트) 연결, 나머지는 자동으로 찾음.
    /// </summary>
    public class PlayerWaterState : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private Transform headTransform;
        [Tooltip("비워두면 자동으로 찾음")]
        [SerializeField] private OxygenSystem oxygenSystem;
        [SerializeField] private PlayerMotor playerMotor;

        [Header("설정")]
        [Tooltip("몸통이 물에 잠겼다고 판단할 기준 - 발밑 기준 이 높이만큼 위까지 잠기면 수영 모드로 전환")]
        [SerializeField] private float bodySubmergeOffset = 0.9f;

        private void Awake()
        {
            if (oxygenSystem == null)
                oxygenSystem = GetComponent<OxygenSystem>();

            if (playerMotor == null)
                playerMotor = GetComponent<PlayerMotor>();

            if (headTransform == null)
                Debug.LogError("[PlayerWaterState] Head Transform이 연결되지 않았습니다.", this);
        }

        private void Update()
        {
            WaterVolume current = FindCurrentWaterVolume();

            if (current == null)
            {
                oxygenSystem?.SetInWater(false);
                playerMotor?.SetSwimming(false);
                return;
            }

            float surfaceY = current.SurfaceHeight;

            bool headUnderwater = headTransform != null && headTransform.position.y < surfaceY;
            bool bodyInWater = (transform.position.y + bodySubmergeOffset) < surfaceY;

            oxygenSystem?.SetInWater(headUnderwater);
            playerMotor?.SetSwimming(bodyInWater);
        }

        private WaterVolume FindCurrentWaterVolume()
        {
            foreach (var volume in WaterVolumeRegistry.All)
            {
                if (volume.ContainsHorizontally(transform.position))
                    return volume;
            }
            return null;
        }
    }
}
