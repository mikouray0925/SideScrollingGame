using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageClient : MonoBehaviour
{
    [SerializeField] bool doBroadcast = false;
    
    private void Awake() {
        MessageCenter.allClients.Add(this);
    }

    public void ReceiveMsg(string methodName, object value = null, SendMessageOptions option = SendMessageOptions.DontRequireReceiver) {
        if (gameObject.activeSelf) {
            if (doBroadcast) BroadcastMessage(methodName, value, option);
            else SendMessage(methodName, value, option);
        }
    }

    private void OnDestroy() {
        MessageCenter.allClients.Remove(this);
    }
}
