using UnityEngine;
using UnityEngine.UI;

namespace RelatedPropertyInputBehaviour
{
    public class RelatedPropertyInputBehaviour : MonoBehaviour
    {
        // Start is called before the first frame update
        protected DialogOperationBehaviour Operator;
        public InputField DisplayContent;
        protected GameObject TargetObject;
        protected DialogPieceBehaviour dialogPieceBehaviour;

        protected void init()
        {
            Operator = DialogOperationBehaviour.instance;
            TargetObject = null;
            Operator.selectObjectChanged +=  new DialogOperationBehaviour.selectObjectChangedHandler(this.selectObjectChanged);
            dialogPieceBehaviour = null;
        }
        public  virtual void selectObjectChanged()
        {
            TargetObject = Operator.selectedObject;
            dialogPieceBehaviour = TargetObject.GetComponent<DialogPieceBehaviour>();
            Debug.Log("object Change to"+Operator.selectedObject.name);
        }
        // Update is called once per frame
        public virtual void ContentChanged(string _string)
        {
            
        }
    }
}
