using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogPieceBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
   
    public int QueueIndex;
    public GameObject MenuoptionPrefab;
    public GameObject SpeakerZoom;
    public Text SpeakerText;
    public GameObject ContentZoom;
    public Text ContentText;
    public GameObject MenuZoom;
    public   DialogPiece dialogPiece;
    public GameObject DefaultOption;
    private DialogOperationBehaviour Operator;
    public List<GameObject> formarDialogPieceNode;//前若干个对话节点
    public List<LinkLineBehaiour> formarDialogLinkLine;
    public GameObject EntranceTag; 
    public  List<GameObject> answermenu;

    public GameObject SelectedMenuOption;

    private void Awake()
    {
        answermenu=new List<GameObject>();
        dialogPiece=new DialogPiece();
        
        answermenu.Add(DefaultOption);
        if (DialogOperationBehaviour.instance != null)
        {
            Operator=DialogOperationBehaviour.instance;
        }
    }

    void Start()
    {
        
        Operator=DialogOperationBehaviour.instance;
        SelectedMenuOption = DefaultOption;
        formarDialogPieceNode=new List<GameObject>();
    }
    public void SetformarDialogPieceNode(GameObject gameObject)
    {
        if (gameObject == null)
        {
            formarDialogPieceNode = null;
            return;
        }
        if (gameObject.GetComponent<DialogPieceBehaviour>() != null)
        {
            formarDialogPieceNode.Add(gameObject);
        }
    }

    public void addMenuPiece()
    {
        MenuOptionBehaviour _p = answermenu[answermenu.Count - 1].GetComponent<MenuOptionBehaviour>();
        if (_p.AnswerContent.GetComponent<InputField>().text==""&&_p.tempLink==null&&_p!=DefaultOption)
        {
            return;
        }
        GameObject option = Instantiate(MenuoptionPrefab);
        option.GetComponent<MenuOptionBehaviour>().parent = gameObject;
        RectTransform opt = option.GetComponent<RectTransform>();
        float height = option.GetComponent<RectTransform>().rect.height;
        opt.SetParent( MenuZoom.GetComponent<RectTransform>());
        opt.anchoredPosition = DefaultOption.GetComponent<RectTransform>().anchoredPosition;
        opt.localPosition=new Vector3(0,-(answermenu.Count-0.5f)*height,0);
        opt.localScale = opt.parent.localScale;
        answermenu.Add(option);
    }

    public  void Selected(BaseEventData data)
    {
        //Debug.Log("我被摸了"+data.selectedObject);
       
        Operator.SetSelectedObject(gameObject);
    }

    public void MouseExit()
    {
        if (Operator.selectedObject == gameObject &&
            SelectedMenuOption.GetComponent<MenuOptionBehaviour>().tempLink== null) 
        {
            Operator.selectedObject = null;
        }
    }
    public void setNodeMatched()
    {
        Operator.GetComponent<DialogOperationBehaviour>().SetLinkMatchObject(gameObject);
        Debug.Log("pointerEnter");
    }

    public void DeleteNullAnswerOption()
    {
        int i = answermenu.Count-2;//最后一个留着
        
        while (i>0)
        {
            MenuOptionBehaviour _p = answermenu[i].GetComponent<MenuOptionBehaviour>();
            if (_p.AnswerContent.GetComponent<InputField>().text == "" && _p.tempLink == null)
            {
                Destroy(answermenu[i]);
                answermenu.RemoveAt(i);
              
            }
            i--;
        }

        if (DefaultOption.GetComponent<MenuOptionBehaviour>().tempLink == null)
        {
            if (answermenu[answermenu.Count - 1] != DefaultOption)
            {
                Destroy(answermenu[answermenu.Count - 1]);
                answermenu.RemoveAt(answermenu.Count - 1);
            }
        }

        RectTransform _rt;
        float height = DefaultOption.GetComponent<RectTransform>().rect.height;
        Vector3 _pos = DefaultOption.GetComponent<RectTransform>().localPosition;
        i = 1;
         while (answermenu.Count > 1 && i < answermenu.Count)
         {
             _rt = answermenu[i].GetComponent<RectTransform>();
             _rt.localPosition = _pos - new Vector3(0, i * height, 0);
             i++;
         }
    }

    public void SetSpeaker(string text)
    {
        SpeakerText.text = text;
    }

    public void DelelteSelf()
    {
        foreach (var v in formarDialogLinkLine)
        {
            v.LostTrack();
        }

        for (var v=0;v<answermenu.Count ; v++)
        {
            int count = answermenu.Count;
            MenuOptionBehaviour menuOption = answermenu[v].GetComponent<MenuOptionBehaviour>();
            if (menuOption.nextDialogPieceNode != null)
            {
                menuOption.nextDialogPieceNode.GetComponent<DialogPieceBehaviour>().formarDialogLinkLine
                    .Remove(menuOption.tempLink.GetComponent<LinkLineBehaiour>());
                menuOption.nextDialogPieceNode.GetComponent<DialogPieceBehaviour>().formarDialogPieceNode
                    .Remove(gameObject);
                Destroy(menuOption.tempLink);
            }

           
        }

        DialogManager.instance.DeleleteDialogPiece(gameObject);
        Destroy(gameObject);
    }
    public void SetContent(string Text)
    {
        ContentText.text = Text;
    }
}
