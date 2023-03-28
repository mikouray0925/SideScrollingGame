using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageClient : MonoBehaviour
{
    public enum NextStepOfReceive {
        SendToSelf,
        Broadcast,
        SendUpwards
    }
    [SerializeField] public NextStepOfReceive nextStepOfReceive;
    
    private void Awake() {
        MessageCenter.allClients.Add(this);
    }

    public void ReceiveMsg(string methodName, object value = null, SendMessageOptions option = SendMessageOptions.DontRequireReceiver) {
        if (gameObject.activeSelf) {
            switch (nextStepOfReceive) {
            case NextStepOfReceive.SendToSelf:
                SendMessage(methodName, value, option);
                break;
            case NextStepOfReceive.Broadcast:
                BroadcastMessage(methodName, value, option);
                break;
            case NextStepOfReceive.SendUpwards:
                SendMessageUpwards(methodName, value, option);
                break;
            }
        }
    }

    public bool ReceiveMsg(Message msg) {
        if (!gameObject.activeSelf) {
            return false;
        }
        if (msg.filterByLayerMask && !GameManager.InLayerMask(gameObject, msg.layerMask)) {
            return false;
        }
        if (msg.filterByTag && gameObject.tag != msg.targetTag) {
            return false;
        }

        msg.onClientReceive(this);
        ReceiveMsg(msg.methodName, msg.value, msg.option);
        return true;
    }

    private void OnDestroy() {
        MessageCenter.allClients.Remove(this);
    }
}
