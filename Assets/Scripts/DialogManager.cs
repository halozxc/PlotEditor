using System;

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

using Object = UnityEngine.Object;

public class DialogManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static string defaultContent = "unknown";
  
   public GameObject DialogPiecePrefab;
   public GameObject MenuOptionPrefab;
   public GameObject LinkLinePrefab;
   public RectTransform ScrollZone;
   public static DialogManager instance;
   private RectTransform DialogPieceRect;
   public  List<GameObject> DialogPieces;
   public  List<DialogPieceBehaviour> dialogPieceBehaviours;
   public GameObject EntranceDialog
   {
       get { return DialogPieces[0];}
      
   }

   #region EventFunc
 private void Awake()
 {
        instance = this;
       if (DialogPiecePrefab.GetComponent<DialogPieceBehaviour>() == null)
       {
           Debug.LogError("couldn't find request component:"+typeof(DialogPieceBehaviour));
           return;
       }
       DialogPieces=new List<GameObject>();
       dialogPieceBehaviours=new List<DialogPieceBehaviour>();
       GameObject _gameObject = GameObject.Find("DialogPiecePanel");
       if (_gameObject != null)
       {
           DialogPieces.Add(_gameObject);
           dialogPieceBehaviours.Add(_gameObject.GetComponent<DialogPieceBehaviour>());
       }
       else
       {
           _gameObject=Instantiate(DialogPiecePrefab);
           DialogPieces.Add(_gameObject);
           dialogPieceBehaviours.Add(_gameObject.GetComponent<DialogPieceBehaviour>());
           RectTransform _transform = _gameObject.GetComponent<RectTransform>();
          _transform.SetParent(ScrollZone.GetComponent<Transform>());
         
           _transform.localPosition = new Vector3(0, 0, 0);
           _transform.localScale=new Vector3(1,1,1);
       }
   }
   

   #endregion

   #region File
 public void selizeTest()
   {
       SaveToFile();
   }

 public void OpenTest()
 {

     string path = MyFile.AddGame();
     if (path != null)
     {
         StreamReader reader=new StreamReader(path);
         string str = reader.ReadToEnd();
         Dialog dialog = JsonConvert.DeserializeObject<Dialog>(str);
         DialogToBehaviour(dialog);   
     }
     
     
 }
   public  Dialog BehaviourToDialog()
   {
       Dialog dialog=new Dialog();
       int i = 0;
       foreach (var v in dialogPieceBehaviours)
       {
           DialogPiece dialogPiece=new DialogPiece();
           dialogPiece.content = v.ContentText.text;
           dialogPiece.speaker = v.SpeakerText.text;
           dialogPiece.NodePosition = DialogPieces[v.QueueIndex].GetComponent<RectTransform>().anchoredPosition;
           if (v.formarDialogPieceNode.Count != 0)
           {
                foreach (var w in v.formarDialogPieceNode)
                          {
                              dialogPiece.formarPiece.Add(w.GetComponent<DialogPieceBehaviour>().QueueIndex);    
                          }
           }



           if (v.answermenu[0].GetComponent<MenuOptionBehaviour>().nextDialogPieceNode != null)
           {
               dialogPiece.nextPiece = new int();
               dialogPiece.nextPiece = v.answermenu[0].GetComponent<MenuOptionBehaviour>().nextDialogPieceNode
                   .GetComponent<DialogPieceBehaviour>().QueueIndex;
           }

           if(v.answermenu.Count>1)
           for (int w=1 ;w< v.answermenu.Count-1;w++)
           {
               
               DialogAnswer dialogAnswer=new DialogAnswer();
               
               dialogAnswer.answerContent =
                  v.answermenu[w].GetComponent<MenuOptionBehaviour>().AnswerContent.GetComponent<InputField>().text;
               dialogAnswer.nextPiece = v.answermenu[w].GetComponent<MenuOptionBehaviour>().nextDialogPieceNode
                   .GetComponent<DialogPieceBehaviour>().QueueIndex;
               dialogPiece.dialoganswer.Add(dialogAnswer);
           }
            dialog.dialogStream.Add(dialogPiece);

            i++;
       }
        Debug.Log(JsonUtility.ToJson(dialog));
       return dialog;
   }

   public void DialogToBehaviour(Dialog dialog)
   {
       while (dialogPieceBehaviours.Count > 0)
       {
           dialogPieceBehaviours[0].DelelteSelf();
       }

//恢复节点与连接
       foreach (var v in dialog.dialogStream)
       {
           GameObject piece = Instantiate(DialogPiecePrefab);
           DialogPieceBehaviour behaviour = piece.GetComponent<DialogPieceBehaviour>();
           RectTransform transform = piece.GetComponent<RectTransform>();
           transform.SetParent(ScrollZone);
           transform.localScale = new Vector3(1, 1, 1);
           transform.anchoredPosition = v.NodePosition;
           behaviour.ContentText.text = v.content;
           behaviour.SpeakerText.text = v.speaker;
           behaviour.QueueIndex = DialogPieces.Count;
           DialogPieces.Add(piece);
           dialogPieceBehaviours.Add(behaviour);
       }

       foreach (var v in dialogPieceBehaviours)
       {
           int? x = dialog.dialogStream[v.QueueIndex].nextPiece;
           if (x != null)
           {
               MenuOptionBehaviour menu = v.DefaultOption.GetComponent<MenuOptionBehaviour>();
               menu.parent = DialogPieces[v.QueueIndex];
               menu.AddNewLink();
               menu.nextDialogPieceNode = DialogPieces[dialog.dialogStream[v.QueueIndex].nextPiece.Value];
               menu.tempLink.GetComponent<LinkLineBehaiour>().next = menu.nextDialogPieceNode;
               menu.nextDialogPieceNode.GetComponent<DialogPieceBehaviour>().formarDialogLinkLine.Add(menu.tempLink.GetComponent<LinkLineBehaiour>());
               menu.nextDialogPieceNode.GetComponent<DialogPieceBehaviour>().formarDialogPieceNode.Add(menu.parent);
               
           }
            
                       
           for (int i = 0; i < dialog.dialogStream[v.QueueIndex].dialoganswer.Count; i++)
           {
            v.addMenuPiece();
            MenuOptionBehaviour menu = v.answermenu[v.answermenu.Count - 1].GetComponent<MenuOptionBehaviour>();
            menu.parent = DialogPieces[v.QueueIndex];
            menu.AnswerContent.GetComponent<InputField>().text =
                dialog.dialogStream[v.QueueIndex].dialoganswer[i].answerContent;
            if (dialog.dialogStream[v.QueueIndex].dialoganswer[i].nextPiece != null)
            {
                menu.AddNewLink();
                menu.nextDialogPieceNode =
                    DialogPieces[dialog.dialogStream[v.QueueIndex].dialoganswer[i].nextPiece.Value];
               menu.tempLink.GetComponent<RectTransform>().SetParent(ScrollZone);
               menu.tempLink.GetComponent<RectTransform>().localScale=new Vector3(1,1,1);
                menu.tempLink.GetComponent<LinkLineBehaiour>().next = menu.nextDialogPieceNode;
                menu.nextDialogPieceNode.GetComponent<DialogPieceBehaviour>().formarDialogLinkLine.Add(menu.tempLink.GetComponent<LinkLineBehaiour>());
                menu.nextDialogPieceNode.GetComponent<DialogPieceBehaviour>().formarDialogPieceNode.Add(menu.parent);
                
            }
           }
       }
   }

   public void SaveToFile()
   {
       
       Dialog a=new Dialog();
       a = BehaviourToDialog();
       string FileContent = JsonConvert.SerializeObject(a);
       string pth = MyFile.SaveGame();
       if (pth != null)
       {
           
          StreamWriter writer=new StreamWriter(pth);
          writer.Write(FileContent);
          writer.Flush();
          writer.Close();
       }
      
   }
   

   #endregion
  
   public GameObject AddDialogPiece()
   {
       GameObject _p = Instantiate(DialogPiecePrefab);
       DialogPieces.Add(_p);
       dialogPieceBehaviours.Add(_p.GetComponent<DialogPieceBehaviour>());
       _p.GetComponent<DialogPieceBehaviour>().QueueIndex = DialogPieces.Count - 1;
       RectTransform _rect = _p.GetComponent<RectTransform>();
       _rect.SetParent(ScrollZone);
       _rect.anchoredPosition=new Vector2(0,0);
       _rect.localScale=new Vector3(1,1,1);
       
       return _p;
        
   }

   public bool DeleleteDialogPiece(GameObject gameObject)
   {
       return dialogPieceBehaviours.Remove(gameObject.GetComponent<DialogPieceBehaviour>())&& DialogPieces.Remove(gameObject);
   }
   
}

[Serializable]
public class DialogEditorCommand
{
    public Object sourceobject;
    public Object targectobject;

    public delegate void CExcuteHandler(Object source,Object target);

    public delegate void CUndoHandler(Object source,Object target);

    public event CExcuteHandler Excute;
    public event CUndoHandler Undo;
    public virtual void CommandExecute()
    {
        Excute(sourceobject, targectobject);
    }

    public virtual void CommandUndo()
    {
        Undo(sourceobject, targectobject);
    }
}

public class AddPieceCommand : DialogEditorCommand
{
    public AddPieceCommand()
    {
        sourceobject = null;
        targectobject = null;
    }
    public  override void CommandExecute()
    {
        
        base.CommandExecute();
    }

    public override void CommandUndo()
    {
        base.CommandUndo();
    }
    
}