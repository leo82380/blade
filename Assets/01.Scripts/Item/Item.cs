using DG.Tweening;
using ObjectPooling;
using System;
using System.Collections;
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
    private bool _dropEnd = false;

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
        }else
        {
            Debug.Log("땅이 없습니다.");
            destination = position; //바닥이 없으면 걍 위로 올라갔다가 떨어지도록 함.
        }

        float jumpPower = Random.Range(1.5f, 2f);
        float duration = Random.Range(0.7f, 1.2f);
        int jumpCount = 1;
        transform.DOJump(destination, jumpPower, jumpCount, duration)
            .OnComplete(() => _dropEnd = true);
    }

    public bool PickUpItem(Transform pickerTrm)
    {
        if (_alreadyCollected || _dropEnd == false) return false;

        _alreadyCollected = true;
        StartCoroutine(PickUpCoroutine(pickerTrm));
        return true;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
    }

    private IEnumerator PickUpCoroutine(Transform pickerTrm)
    {
        _collider.enabled = false;
        Vector3 startPos = transform.position;
        //처음 먹을 때 플레이어와 코인간의 거리를 측정
        float distance = (pickerTrm.position - startPos).magnitude;

        float totalTime = distance * 0.1f;
        float current = 0;
        
        while(current / totalTime <= 1)
        {
            current += Time.deltaTime;

            transform.position = Vector3.Lerp(startPos, pickerTrm.position, current / totalTime);
            yield return null;
        }

        if(_audioSource.clip != null)
        {
            _audioSource.Play();
            yield return new WaitForSeconds(_audioSource.clip.length + 0.1f);
        }else
        {
            yield return null;
        }

        OnCollected?.Invoke(this);
        PoolManager.Instance.Push(this);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public override void ResetItem()
    {
        _collider.enabled = true;
        _alreadyCollected = false;
        _dropEnd = false;
    }
}