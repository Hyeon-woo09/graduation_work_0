using UnityEngine;

namespace JHJ.Scripts.Player.Inventory
{
    /// <summary>
    /// 아이템 하나의 기본 정보. 나중에 상점(유현우 담당)에서도 같은 데이터를 참조하게 될 것.
    /// 우클릭 -> Create -> Game -> Inventory -> Item Data 로 생성.
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Game/Inventory/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Tooltip("코드에서 식별용으로 쓸 고유 ID (예: item_wood, item_ore)")]
        public string itemId;

        public string itemName;
    }
}
