using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public float collectRadius;
    [SerializeField] private LayerMask _whatIsItem;
    [SerializeField] private int _collectCount;
    
    private Collider[] _colliders;
    
    private Player _player;

    private void Awake()
    {
        _colliders = new Collider[_collectCount];
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position, collectRadius, _colliders, _whatIsItem);

        for (int i = 0; i < count; i++)
        {
            if (_colliders[i].TryGetComponent(out Item item))
            {
                if (item.PickUpItem(transform))
                {
                    item.OnCollected += HandleOnCollected;
                }
            }
        }
    }

    private void HandleOnCollected(Item item)
    {
        item.OnCollected -= HandleOnCollected;

        switch (item.ItemData.itemType)
        {
            case ItemType.Coin:
                PlayerManager.Instance.AddCoin(1);
                break;
            case ItemType.Heal:
                break;
            case ItemType.PowerUp:
                break;
        }
        
        _player.PlayerVFXCompo.PlayCollectParticle();
    }
}
