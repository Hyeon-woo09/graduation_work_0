using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JHJ.Scripts.Player.Oxygen
{
    /// <summary>
    /// 화면 왼쪽 중단에 뜨는 산소 게이지 UI.
    /// 물에 들어가면 페이드 인, 나오면 페이드 아웃.
    /// Slider 대신 Image(Filled 타입)를 씀 - Fill Rect/Handle Rect 설정 실수 여지가 없어서 더 확실함.
    ///
    /// 세팅:
    /// 1. Canvas 안에 Panel 생성 -> 화면 왼쪽 중단에 배치
    /// 2. 그 Panel에 CanvasGroup 추가, 시작 Alpha = 0
    /// 3. Panel 자식으로 UI -> Image 하나 생성 (게이지 바 역할)
    ///    -> 그 Image의 Image Type을 "Filled"로 변경
    ///    -> Fill Method는 Horizontal, Fill Origin은 Left로 설정 (왼쪽부터 차오르는 바)
    /// 4. 이 스크립트를 Panel에 Add Component, Fill Image 필드에 방금 만든 Image 연결
    /// </summary>
    public class OxygenUI : MonoBehaviour
    {
        [Header("참조")]
        [Tooltip("비워둬도 됨 - 씬에서 자동으로 찾음 (프리팹을 잘못 연결하는 실수 방지)")]
        [SerializeField] private OxygenSystem oxygenSystem;
        [SerializeField] private CanvasGroup canvasGroup;
        [Tooltip("Image Type이 Filled로 설정된 게이지 바 이미지")]
        [SerializeField] private Image fillImage;
        [Tooltip("없으면 비워둬도 됨")]
        [SerializeField] private TMP_Text oxygenText;

        [Header("페이드 설정")]
        [SerializeField] private float fadeDuration = 0.3f;

        private Coroutine _fadeCoroutine;

        private void Awake()
        {
            if (canvasGroup != null)
                canvasGroup.alpha = 0f;

            var sceneInstance = FindFirstObjectByType<OxygenSystem>();
            if (sceneInstance != null)
                oxygenSystem = sceneInstance;
            else
                Debug.LogError("[OxygenUI] 씬에서 OxygenSystem을 찾지 못했습니다.", this);
        }

        private void OnEnable()
        {
            if (oxygenSystem == null) return;

            oxygenSystem.OnOxygenChanged += HandleOxygenChanged;
            oxygenSystem.OnEnterWater += HandleEnterWater;
            oxygenSystem.OnExitWater += HandleExitWater;
        }

        private void OnDisable()
        {
            if (oxygenSystem == null) return;

            oxygenSystem.OnOxygenChanged -= HandleOxygenChanged;
            oxygenSystem.OnEnterWater -= HandleEnterWater;
            oxygenSystem.OnExitWater -= HandleExitWater;
        }

        private void HandleOxygenChanged(float current, float max)
        {
            if (fillImage != null)
                fillImage.fillAmount = max > 0f ? current / max : 0f;

            if (oxygenText != null)
                oxygenText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
        }

        private void HandleEnterWater() => FadeTo(1f);
        private void HandleExitWater() => FadeTo(0f);

        private void FadeTo(float targetAlpha)
        {
            if (canvasGroup == null) return;

            if (_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);

            _fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha));
        }

        private IEnumerator FadeRoutine(float targetAlpha)
        {
            float startAlpha = canvasGroup.alpha;
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
        }
    }
}