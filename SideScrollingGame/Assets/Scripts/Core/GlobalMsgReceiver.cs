using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMsgReceiver : MonoBehaviour
{
    [SerializeField] bool doBroadcast = false;
    
    private void Awake() {
        MessageCenter.allReceivers.Add(this);
    }

    public void ReceiveMsg(string methodName, object value = null, SendMessageOptions option = SendMessageOptions.DontRequireReceiver) {
        if (gameObject.activeSelf) {
            if (doBroadcast) BroadcastMessage(methodName, value, option);
            else SendMessage(methodName, value, option);
        }
    }

    /*
    public void ReceiveMsg(string methodName, SendMessageOptions option = SendMessageOptions.DontRequireReceiver) {
        if (gameObject.activeSelf) {
            if (doBroadcast) BroadcastMessage(methodName, option);
            else SendMessage(methodName, option);
        }
    }
    */

    private void OnDestroy() {
        MessageCenter.allReceivers.Remove(this);
    }
}
