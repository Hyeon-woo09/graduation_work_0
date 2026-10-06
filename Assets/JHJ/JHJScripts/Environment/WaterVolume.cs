using UnityEngine;

namespace JHJ.Scripts.Environment
{
    /// <summary>
    /// 하나의 물 구역(일반 바다, 심해 등)을 나타내는 컴포넌트.
    /// 수면 높이(SurfaceHeight)와, 이 구역이 차지하는 가로/세로(XZ) 범위를 가짐.
    /// 콜라이더의 세로(Y) 깊이/바닥 모양은 전혀 신경 안 씀 - 판정은 XZ 범위 + 수면 높이만으로 함.
    /// 그래서 바닥 지형이 얼마나 깊든, 울퉁불퉁하든, 여러 구역이 서로 다른 수심을 가져도 전혀 문제없음.
    ///
    /// 세팅:
    /// 1. 빈 오브젝트를 물 구역 중심(터레인 위 해당 바다/심해 영역)에 배치
    /// 2. Y 위치를 그 구역의 수면 높이에 맞춤 (Use Transform Y 체크 시 이 값을 그대로 씀)
    /// 3. Box Collider 추가 -> 가로/세로(X/Z) 크기를 그 구역의 실제 범위만큼 넓게 설정
    ///    (Y Size는 대충 크게 잡아도 됨, 판정에 안 쓰임 - 그냥 Scene 뷰에서 보기 편하라고 있는 것)
    /// 4. 이 스크립트 Add Component
    ///
    /// 일반 바다, 심해 등 구역이 여러 개면 이 컴포넌트를 구역마다 하나씩 배치하면 됨
    /// (각자 다른 수면 높이/범위를 가질 수 있음).
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class WaterVolume : MonoBehaviour
    {
        [Tooltip("체크하면 이 오브젝트의 Y 위치를 수면 높이로 사용")]
        [SerializeField] private bool useTransformY = true;
        [SerializeField] private float manualSurfaceHeight;

        private BoxCollider _boundsCollider;

        public float SurfaceHeight => useTransformY ? transform.position.y : manualSurfaceHeight;

        private void Awake()
        {
            _boundsCollider = GetComponent<BoxCollider>();
        }

        private void OnEnable() => WaterVolumeRegistry.Register(this);
        private void OnDisable() => WaterVolumeRegistry.Unregister(this);

        /// <summary>이 지점이 이 물 구역의 가로/세로(XZ) 범위 안에 있는지 (깊이는 검사 안 함)</summary>
        public bool ContainsHorizontally(Vector3 worldPosition)
        {
            if (_boundsCollider == null) _boundsCollider = GetComponent<BoxCollider>();

            Bounds b = _boundsCollider.bounds;
            return worldPosition.x >= b.min.x && worldPosition.x <= b.max.x &&
                   worldPosition.z >= b.min.z && worldPosition.z <= b.max.z;
        }
    }
}
