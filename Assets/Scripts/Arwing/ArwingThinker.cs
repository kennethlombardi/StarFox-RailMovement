﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class ArwingThinker : MonoBehaviour
{
    public ArwingBrain brain;

    private Dictionary<string, object> memory;
    [NonSerialized] public GameState.PlayerState player;

    public T Remember<T>(string key)
    {
        object result;
        if (!memory.TryGetValue(key, out result))
            return default(T);
        return (T)result;
    }

    public void Remember<T>(string key, T value)
    {
        memory[key] = value;
    }

    void Awake()
    {
        enabled = false;
    }

    void OnEnable()
    {
        if (!brain)
        {
            enabled = false;
            return;
        }

        memory = new Dictionary<string, object>();
        brain.Initialize(this);
    }

    void Update()
    {
        brain.Think(this);
    }

    public void Setup()
    {
        enabled = true;
    }

    private void SetColor(Color c)
    {
        foreach (var r in GetComponentsInChildren<MeshRenderer>())
        {
            r.material.color = c;
        }
    }
}