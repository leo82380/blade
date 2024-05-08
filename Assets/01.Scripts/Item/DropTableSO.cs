using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item/DropTable")]
public class DropTableSO : ScriptableObject
{
    public int dropExp;
    public int dropGoldMin;
    public int dropGoldMax;

    public int GetDropGold()
    {
        return Random.Range(dropGoldMin, dropGoldMax);
    }
}
