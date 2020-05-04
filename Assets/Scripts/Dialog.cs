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
}
[Serializable]
public class DialogPiece
{
    public List<int> formarPiece;
    public int? nextPiece;
    
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