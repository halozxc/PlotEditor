using System;
using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.material;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptionBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject LinkDot;//for linkline tracking position
    public GameObject parent;//DialogPieceNode
    public GameObject LinkLinePrefab;
    public GameObject AnswerContent;
    public GameObject nextDialogPieceNode;
    public GameObject tempLink;//用于连接
    private void Start()
    {
        nextDialogPieceNode = null;
    }

    public void optionselected()
    {
        parent.GetComponent<DialogPieceBehaviour>().SelectedMenuOption = gameObject;
    }
    public void OptionLinkCancel()
    {
        parent.GetComponent<DialogPieceBehaviour>().SelectedMenuOption = gameObject;
        
    }
    
    
    public void AddNewLink()
    {
        if (nextDialogPieceNode==null)
        {
            tempLink = Instantiate(LinkLinePrefab);
            
            LinkLineBehaiour linkline = tempLink.GetComponent<LinkLineBehaiour>();
            linkline.former = parent;
            linkline.formerRect = LinkDot;
            linkline.option = gameObject;
            parent.GetComponent<DialogPieceBehaviour>().Selected(null);    
        }
        
    }

    public void LinkSucceed()
    {
        if (tempLink != null) tempLink.GetComponent<LinkLineBehaiour>().LinkSucceedAndSetNext(nextDialogPieceNode);
        parent.GetComponent<DialogPieceBehaviour>().addMenuPiece();
    }

    public void Checknull()
    {
        if (AnswerContent.GetComponent<InputField>().text == ""||gameObject==parent.GetComponent<DialogPieceBehaviour>().DefaultOption)
        {
            tempLink = null;
            nextDialogPieceNode = null;
            parent.GetComponent<DialogPieceBehaviour>().DeleteNullAnswerOption();
        }
           
            return;
    }
    
}
