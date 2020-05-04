using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentInputBehaviour : RelatedPropertyInputBehaviour.RelatedPropertyInputBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        init();
    }

   
    public  override void selectObjectChanged()
    {
        base.selectObjectChanged();
        DisplayContent.text = dialogPieceBehaviour.ContentText.text;

    }
    // Update is called once per frame
    public override void ContentChanged(string _string)
    {
        if (TargetObject==null||dialogPieceBehaviour == null)
        {
            Debug.Log("null target");
            return;
        }
        dialogPieceBehaviour.SetContent(DisplayContent.text);
    }
}
