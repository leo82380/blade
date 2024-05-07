using DG.Tweening;
using ObjectPooling;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Item : PoolableMono
{
    [field: SerializeField] public ItemDataSO ItemData { get; set; }

    [SerializeField] private float _rotateSpeed;
    [SerializeField] private LayerMask _whatIsGround;

    private AudioSource _audioSource;
    private SphereCollider _collider;

    public Action<Item> OnCollected;

    private bool _alreadyCollected = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _collider = GetComponent<SphereCollider>();
        _audioSource.clip = ItemData.getSound;
    }

    public void SetItemData(Vector3 position, Vector3 direction)
    {
        Vector3 startPosition = position + new Vector3(0, 1, 0);
        transform.position = startPosition;
        float randomMin = 1.5f;
        float randomMax = 3.5f;
        Vector3 destination = startPosition 
                              + direction * Random.Range(randomMin, randomMax);

        if(Physics.Raycast(destination, Vector3.down, out RaycastHit hit, 10f, _whatIsGround))
        {
            destination = hit.point + new Vector3(0, _collider.radius);
        }
        else
        {
            Debug.Log("땅이 없습니다.");
            destination = position; //바닥이 없으면 걍 위로 올라갔다가 떨어지도록 함.
        }

        float jumpPower = Random.Range(1.5f, 2f);
        float duration = Random.Range(0.7f, 1.2f);
        int jumpCount = 1;
        transform.DOJump(destination, jumpPower, jumpCount, duration);
    }

    public void PickUpItem(Transform pickTrm)
    {
        
    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public override void ResetItem()
    {
        
    }
}