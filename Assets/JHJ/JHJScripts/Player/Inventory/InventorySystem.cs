using System;
using System.Collections.Generic;
using UnityEngine;

namespace JHJ.Scripts.Player.Inventory
{
    /// <summary>
    /// 플레이어가 보유한 아이템 수량을 관리.
    /// 지금은 UI 연결 전이라 획득 시 콘솔에 로그만 찍음.
    /// OnItemAdded 이벤트는 나중에 UI 붙일 때 그대로 구독해서 쓰면 됨 (지금 미리 만들어둠).
    /// </summary>
    public class InventorySystem : MonoBehaviour
    {
        /// <summary>아이템이 추가될 때마다 발생 (아이템, 그 아이템의 새 총 보유량)</summary>
        public event Action<ItemData, int> OnItemAdded;

        private readonly Dictionary<ItemData, int> _items = new Dictionary<ItemData, int>();

        public void AddItem(ItemData item, int amount = 1)
        {
            if (item == null || amount <= 0) return;

            _items.TryGetValue(item, out int current);
            int newTotal = current + amount;
            _items[item] = newTotal;

            Debug.Log($"[InventorySystem] 획득: {item.itemName} x{amount} (총 {newTotal}개)");

            OnItemAdded?.Invoke(item, newTotal);
        }

        public int GetCount(ItemData item)
        {
            return item != null && _items.TryGetValue(item, out int count) ? count : 0;
        }
    }
}
