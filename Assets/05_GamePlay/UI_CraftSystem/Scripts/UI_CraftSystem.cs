using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CraftSystem : MonoBehaviour
{
    private UI_CraftSystemScrollerController _controller;

    void Start()
    {
        _controller = GetComponent<UI_CraftSystemScrollerController>();

        _controller.SetData();
    }

    void Update()
    {
        
    }
}
