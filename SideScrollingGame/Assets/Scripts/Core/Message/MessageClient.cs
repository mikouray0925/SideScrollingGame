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

    public void ReceiveMsg(Message msg) {
        if (!gameObject.activeSelf) {
            return;
        }
        if (msg.filterByLayerMask && !GameManager.InLayerMask(gameObject, msg.layerMask)) {
            return;
        }
        if (msg.filterByTag && gameObject.tag != msg.targetTag) {
            return;
        }

        msg.onClientReceive(this);
        if (doBroadcast) BroadcastMessage(msg.methodName, msg.value, msg.option);
        else SendMessage(msg.methodName, msg.value, msg.option);
    }

    private void OnDestroy() {
        MessageCenter.allClients.Remove(this);
    }
}
