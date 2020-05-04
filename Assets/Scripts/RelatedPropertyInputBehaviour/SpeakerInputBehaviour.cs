using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerInputBehaviour : RelatedPropertyInputBehaviour.RelatedPropertyInputBehaviour
{
    // Start is called before the first frame update
   
   
    private void Start()
    {
      init();
    }

    public  override void selectObjectChanged()
    {
       base.selectObjectChanged();
       DisplayContent.text = dialogPieceBehaviour.SpeakerText.text;
       
    }
    // Update is called once per frame
    public override void ContentChanged(string _string)
    {
        if (TargetObject==null||dialogPieceBehaviour == null)
        {
            Debug.Log("null target");
            return;
        }
        dialogPieceBehaviour.SetSpeaker(DisplayContent.text); 
    }
}
