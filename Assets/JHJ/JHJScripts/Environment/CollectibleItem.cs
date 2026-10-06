using UnityEngine;
using JHJ.Scripts.Player.Inventory;

namespace JHJ.Scripts.Environment
{
    /// <summary>
    /// 월드에 놓는 채집 가능한 오브젝트.
    /// 더 이상 닿기만 해서 자동으로 안 먹힘 - PlayerCollector가 F를 누른 순간
    /// 이 컴포넌트의 Collect()를 호출해줘야 실제로 획득됨.
    ///
    /// 세팅: 채집 가능한 오브젝트에 Add Component, Item Data 연결하면 끝.
    /// Collider는 Trigger 여부 상관없음 - Physics.OverlapSphere로 찾는 방식이라
    /// 트리거 설정 자체가 중요하지 않음 (콜라이더만 있으면 됨).
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class CollectibleItem : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        [SerializeField] private int amount = 1;

        /// <summary>PlayerCollector가 F 눌렀을 때 이 메서드를 호출함</summary>
        public void Collect(InventorySystem inventory)
        {
            if (itemData == null)
            {
                Debug.LogWarning($"[CollectibleItem] {gameObject.name}에 Item Data가 연결되지 않았습니다.", this);
                return;
            }

            inventory.AddItem(itemData, amount);
            Destroy(gameObject);
        }
    }
}