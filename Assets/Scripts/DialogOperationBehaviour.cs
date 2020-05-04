using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogOperationBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public static DialogOperationBehaviour instance;
    public delegate void selectObjectChangedHandler();

   
    
   
    public event selectObjectChangedHandler selectObjectChanged; 
    public  GameObject selectedObject;
    
    #region EventFunc
    private void Awake()
    {
        instance  = this ;
    }

    private void Start()
    {
        selectedObject = null;
       

    }
    

    #endregion

    #region DialogOperation

    public void AddDialogPiece()
    { 
        
       selectedObject= DialogManager.instance.AddDialogPiece();
    }

    public void DelelteDialogPiece()
    {
        if (selectedObject)
        {
            selectedObject.GetComponent<DialogPieceBehaviour>().DelelteSelf();
        }

        selectedObject = null;
    }

    #endregion
    #region DialogNodeMatch

    
    public void SetSelectedObject(GameObject _selectedObject)
    {

        
        if (_selectedObject)
        {
            if (_selectedObject.GetComponent<DialogPieceBehaviour>() == null)
            {
                Debug.LogError("couldn't find request component:" + typeof(DialogPieceBehaviour));
                return;
            }

            selectedObject = _selectedObject;
            Debug.Log(selectedObject.name);
            if (selectObjectChanged != null) selectObjectChanged();
        }
    }

    public void SetLinkMatchObject(GameObject linkMatchedObject)
    {
        DialogPieceBehaviour t = linkMatchedObject.GetComponent<DialogPieceBehaviour>();
        if (selectedObject)
        {
            t.SetformarDialogPieceNode(selectedObject);
            selectedObject.GetComponent<DialogPieceBehaviour>().SelectedMenuOption.GetComponent<MenuOptionBehaviour>()
                .nextDialogPieceNode = linkMatchedObject;
            selectedObject.GetComponent<DialogPieceBehaviour>().SelectedMenuOption.GetComponent<MenuOptionBehaviour>().LinkSucceed();
            //why set null? for Cancel link easier;
            selectedObject = null;
        }
    }
    #endregion

   
   
}
