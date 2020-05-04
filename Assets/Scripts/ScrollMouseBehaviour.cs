using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollMouseBehaviour : MonoBehaviour
{
    //这段代码用于鼠标滚轮控制缩放
    // Start is called before the first frame update
    private RectTransform rectTransform;
    private float scale = 1.0f;
    private float nextscale;
    void Start()
    {
         rectTransform = gameObject.GetComponent<ScrollRect>().content;
         nextscale = Mathf.Lerp(rectTransform.localScale.x, scale, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
         scale= Mathf.Max(0.1f,scale+Input.GetAxis("Mouse ScrollWheel"));
         nextscale = Mathf.Lerp(rectTransform.localScale.x, scale, 0.3f);
         rectTransform.localScale = new Vector3(nextscale, nextscale, nextscale);
    }
}
