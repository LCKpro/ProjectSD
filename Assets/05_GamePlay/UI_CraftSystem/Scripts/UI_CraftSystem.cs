using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CraftSystem : MonoBehaviour
{
    private UI_CraftSystemScrollerController _controller = null;

    void Start()
    {
    }

    public void OnCraft(BuildingType type)
    {
        if(_controller == null)
            _controller = GetComponent<UI_CraftSystemScrollerController>();
        _controller.SetData(type);
    }
}
