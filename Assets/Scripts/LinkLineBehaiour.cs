using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LinkLineBehaiour : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject option;//option belong's to
    public GameObject former;//formernode
    public GameObject next;//nextnode
    public GameObject formerRect;//formerNodeForTrack
    public GameObject NextRect;//nextNodeForTrack
    private RectTransform formerTrans;//transform of formerRect
    private RectTransform nextTrans;//transform of formerRect
    private RectTransform parent;//dialogPiece‘parent
    private RectTransform transform;
    private void Start()
    {
        transform = gameObject.GetComponent<RectTransform>();
        if(former&&formerRect.GetComponent<RectTransform>()){
            formerTrans = formerRect.GetComponent<RectTransform>();
            if (nextTrans != null)
            {
                nextTrans = next.GetComponent<RectTransform>();
            }
           
            
            gameObject.GetComponent<RectTransform>().SetParent(GameObject.Find("ScrollView").GetComponent<RectTransform>());
            parent = (RectTransform) gameObject.GetComponent<RectTransform>().parent;
            Vector3 pivot = formerTrans.position;
            Vector3 spos = Camera.main.WorldToScreenPoint(pivot);
            Vector2 _tempV2 =new Vector2(spos.x, spos.y);
            Vector2 _uiPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, _tempV2, Camera.main, out _uiPos);
            transform.anchoredPosition = _uiPos;
        }
        else
        {
            Debug.LogError(former.name+" or "+next.name+" has no component:"+typeof(RectTransform));
            LostTrack();
            return;
        }
        
        
    }

    private void Update()
    {
       if(next==null){ //跟随鼠标
           Vector2 _uiPos1,_uiPos2;
           _uiPos1 = transform.anchoredPosition;
           RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Camera.main.WorldToScreenPoint(Input.mousePosition), Camera.main, out _uiPos2);
           transform.localScale=new Vector3(1,1,1);
           Quaternion angle=new Quaternion();
           angle.eulerAngles=new Vector3(0,0,57.3f*Mathf.Atan((_uiPos2.y-_uiPos1.y)/(_uiPos2.x-_uiPos1.x)));//180/PI=57.3
           transform.sizeDelta = new Vector2(Mathf.Max(Vector2.Distance(_uiPos1, _uiPos2)-20,0),1);
           transform.rotation = angle;
       }
       else if (former != null) //跟随两个对话node
       {
           if (NextRect == null)
           {
               NextRect = next.GetComponent<DialogPieceBehaviour>().EntranceTag;
               nextTrans = NextRect.GetComponent<RectTransform>();
           }
           Vector3 pivot1 = formerTrans.position;
           Vector3 pivot2 = nextTrans.position;
           Vector3 spos1 = Camera.main.WorldToScreenPoint(pivot1);
           Vector3 spos2 = Camera.main.WorldToScreenPoint(pivot2);
              
           Vector2 _tempV1 =new Vector2(spos1.x, spos1.y);
           Vector2 _tempV2 =new Vector2(spos2.x, spos2.y);
           Vector2 _uiPos1,_uiPos2;
           RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, _tempV1, Camera.main, out _uiPos1);
           RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, _tempV2, Camera.main, out _uiPos2);
           transform.anchoredPosition = _uiPos1;
           Quaternion angle=new Quaternion();
           angle.eulerAngles=new Vector3(0,0,57.3f*Mathf.Atan((_uiPos2.y-_uiPos1.y)/(_uiPos2.x-_uiPos1.x)));//180/PI=57.3
           transform.rotation = angle;
           transform.sizeDelta=new Vector2(Vector2.Distance(_uiPos1,_uiPos2),1);
           
       }
       else
       {
           LostTrack();
       }
       

    }

    public void LinkSucceedAndSetNext(GameObject next)
    {
        this.next = next;
        next.GetComponent<DialogPieceBehaviour>().formarDialogLinkLine.Add(this);
    }
    public void ChangeLink()
    {
        Debug.Log("pointerdown");
        if (next!=null)
        {
            next.GetComponent<DialogPieceBehaviour>().formarDialogPieceNode.Remove(former);
            next = null;
            NextRect = null;
            nextTrans = null;
            option.GetComponent<MenuOptionBehaviour>().nextDialogPieceNode = null;
        }
    }
   
  

    // Update is called once per frame
    public void LostTrack()
    {
        option.GetComponent<MenuOptionBehaviour>().Checknull();
        Destroy(gameObject);
    }
}
