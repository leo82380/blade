using UnityEngine;

public enum ItemType
{
    Coin,
    Heal,
    PowerUp
}

[CreateAssetMenu(menuName = "SO/Item/item")]
public class ItemDataSO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;

    public AudioClip getSound;
    [ColorUsage(true, true)] public Color popupColor;
}
