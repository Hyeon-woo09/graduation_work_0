using UnityEngine;
using UnityEngine.UI;
using JHJ.Scripts.Player.Input;
using JHJ.Scripts.Environment;

namespace JHJ.Scripts.Player.Inventory
{
    /// <summary>
    /// 크로스헤어(카메라 정면)가 가리키고 있는 CollectibleItem을 매 프레임 확인해서
    /// - 있으면 크로스헤어 색을 강조색으로 바꾸고
    /// - F를 누르면 그걸 채집함.
    ///
    /// 세팅: Player 루트 오브젝트에 Add Component.
    /// Camera Transform에 실제 카메라 연결, Crosshair Image에 화면 중앙 크로스헤어 UI 연결.
    /// </summary>
    public class PlayerCollector : MonoBehaviour
    {
        [Header("참조")]
        [Tooltip("IPlayerInputProvider를 구현한 컴포넌트")]
        [SerializeField] private MonoBehaviour inputProviderSource;
        [Tooltip("비워두면 같은 오브젝트에서 자동으로 찾음")]
        [SerializeField] private InventorySystem inventorySystem;
        [Tooltip("크로스헤어가 가리키는 방향의 기준이 되는 카메라. 비워두면 Camera.main 사용")]
        [SerializeField] private Transform cameraTransform;
        [Tooltip("화면 중앙 크로스헤어 UI Image. 비워두면 색 변경 기능만 꺼짐 (채집 자체는 정상 작동)")]
        [SerializeField] private Image crosshairImage;

        [Header("설정")]
        [Tooltip("이 거리 안에서 조준한 것만 채집 가능")]
        [SerializeField] private float lookRange = 3f;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color highlightColor = Color.yellow;

        private IPlayerInputProvider _input;
        private CollectibleItem _currentTarget;

        private void Awake()
        {
            _input = inputProviderSource as IPlayerInputProvider;
            if (_input == null)
                Debug.LogError("[PlayerCollector] Input Provider Source가 IPlayerInputProvider를 구현하지 않았습니다.", this);

            if (inventorySystem == null)
                inventorySystem = GetComponent<InventorySystem>();

            if (inventorySystem == null)
                Debug.LogError("[PlayerCollector] InventorySystem을 찾지 못했습니다.", this);

            if (cameraTransform == null && UnityEngine.Camera.main != null)
                cameraTransform = UnityEngine.Camera.main.transform;

            if (cameraTransform == null)
                Debug.LogError("[PlayerCollector] Camera Transform이 연결되지 않았고 Camera.main도 찾지 못했습니다.", this);

            if (crosshairImage != null)
                crosshairImage.color = normalColor;
        }

        private void Update()
        {
            _currentTarget = FindLookedAtCollectible();
            UpdateCrosshairColor();

            if (_input != null && _input.InteractPressed && _currentTarget != null)
                _currentTarget.Collect(inventorySystem);
        }

        private void UpdateCrosshairColor()
        {
            if (crosshairImage == null) return; 
            crosshairImage.color = _currentTarget != null ? highlightColor : normalColor;
        }

        private CollectibleItem FindLookedAtCollectible()
        {
            if (cameraTransform == null) return null;

            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, lookRange))
                return hit.collider.GetComponent<CollectibleItem>();

            return null;
        }

        private void OnDrawGizmosSelected()
        {
            if (cameraTransform == null) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(cameraTransform.position, cameraTransform.position + cameraTransform.forward * lookRange);
        }
    }
}