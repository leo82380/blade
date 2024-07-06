﻿using UnityEngine;

public class TestReceiever : MonoBehaviour
{
    private void Start()
    {
        EventManager.AddListener<ResourceAddEvent>(HandleResourceAdd);
    }

    private void HandleResourceAdd(ResourceAddEvent evt)
    {
        Debug.Log($"Recv : {evt.amount}, {evt.message}");
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<ResourceAddEvent>(HandleResourceAdd);
    }
}