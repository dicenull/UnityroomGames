
using System;
using System.Collections.Generic;
using UnityEngine;

public class GetIt : MonoBehaviour
{
    private readonly Dictionary<Type, object> _services = new();
    public static GetIt Instance { get; private set; }

    GetIt()
    {
        if (Instance == null) Instance = this;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // サービスの登録
    public void Register<TInterface, TImplementation>(TImplementation implementation) where TImplementation : TInterface
    {
        _services[typeof(TInterface)] = implementation;
        _services[typeof(TImplementation)] = implementation;
        Debug.Log($"Service registered: {typeof(TInterface).Name}");
    }

    // サービスの取得
    public TInterface Get<TInterface>()
    {
        if (_services.TryGetValue(typeof(TInterface), out var service))
        {
            return (TInterface)service;
        }
        throw new KeyNotFoundException($"Service not found for type: {typeof(TInterface).Name}");
    }
}