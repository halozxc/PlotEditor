using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EntranceTagBehaviour : MonoBehaviour
{
   
    //跟踪的物体
    public  GameObject entrance;
    //跟踪的物体的transform
    private RectTransform _targetRectTransform;
    //自身transform
    private RectTransform _rectTransform;
    [Range(0f,150f)]
    public float distance;
    void Start()
    {
       
        _rectTransform = entrance.GetComponent<RectTransform>();
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _targetRectTransform = entrance.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (entrance)
        {
            _rectTransform.anchoredPosition =
                _targetRectTransform.anchoredPosition - new Vector2((_targetRectTransform.rect.width+_rectTransform.rect.width)*0.5f+distance, 0);
        }
    }
}
