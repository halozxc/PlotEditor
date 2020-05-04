using System;

using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

using Object = UnityEngine.Object;

public class DialogManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static string defaultContent = "unknown";
   public static string defaultBranchName = "default";
   public GameObject DialogPiecePrefab;
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
   public  Dialog BehaviourToDialog()
   {
       Dialog dialog=new Dialog();
       int i = 0;
       foreach (var v in dialogPieceBehaviours)
       {
           DialogPiece dialogPiece=new DialogPiece();
           dialogPiece.content = v.ContentText.text;
           dialogPiece.speaker = v.SpeakerText.text;
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

   public void SaveToFile()
   {
       
       Dialog a=new Dialog();
       a = BehaviourToDialog();
       string FileContent = JsonUtility.ToJson(a);
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
   public Dialog GetAndLoadDialogFile(string FileNmae)
    { 
        
        return null;
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