using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
[Serializable]
public class Dialog
{
   
    public List<DialogPiece> dialogStream;
    //标记着那些分支开始的标志
   

    public Dialog()
    {
        dialogStream=new List<DialogPiece>();
       
    }

    public object this[int vQueueIndex]
    {
        get { throw new NotImplementedException(); }
    }
}
[Serializable]
public class DialogPiece
{
    public List<int> formarPiece;
    public int? nextPiece;

    public  APosition NodePosition;
    //对话内容
    public  string content;
    //说话的那个人的名字
    public  string speaker;
    //音频开始(s)
    public float audioClipStart;
    //音频结束(s)
    public float audioClipEnd;
    public List<DialogAnswer> dialoganswer;
    

    public DialogPiece()
    {
        //什么都不知道就unknown啦
        
       
        content = DialogManager.defaultContent;
        speaker =DialogManager.defaultContent;
        audioClipEnd = 0;
        audioClipStart = 0;
        formarPiece = new List<int>();
        nextPiece = null;
       
        dialoganswer=new List<DialogAnswer>();

    }

    
    
    public DialogPiece( List<int> former ,int  next,string speaker,string content,float audioClipStart,float audioClipEnd)
    {
       
        this.content = content; 
        this.speaker = speaker;
        this.audioClipEnd = audioClipEnd;
        this.audioClipStart = audioClipStart;
        formarPiece = former;
        nextPiece = next;
       
        dialoganswer=new List<DialogAnswer>();
    }

    public bool HasAudio()
    {
        return audioClipEnd - audioClipStart != 0;
    }

   
}

[Serializable]
public class DialogAnswer
{
    public string answerContent;
     public int? nextPiece;

     public  DialogAnswer()
     {
         answerContent = "";
         nextPiece = null;
     }
}

public struct APosition
{
    public  float x;
    public float y;

    APosition(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    
    public static implicit operator Vector2(APosition position)
    {
        return new Vector2(position.x,position.y);
    }

    public static implicit operator APosition(Vector2 vector2)
    {
        return new APosition(vector2.x,vector2.y);
    }
}