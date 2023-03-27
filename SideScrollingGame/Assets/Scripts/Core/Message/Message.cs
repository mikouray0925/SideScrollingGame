using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Message
{
    public string methodName;
    public object value = null;
    public SendMessageOptions option = SendMessageOptions.DontRequireReceiver;
    public bool filterByLayerMask = false;
    public LayerMask layerMask;
    public bool filterByTag = false;
    public string targetTag;
    public delegate void OperateWithClient(MessageClient client);
    public OperateWithClient onClientReceive;

    public Message(string _methodName, object _value = null) {
        methodName = _methodName;
        value = _value;
    }

    /*
    public static Message ActivatingMsg {
        get {
            Message msg = new Message("");
            msg.onClientReceive += ActivateClient;
            return msg;
        }
        private set {}
    }
    */

    public static Message DeactivatingOrder {
        get {
            Message msg = new Message("");
            msg.onClientReceive += DeactivateClient;
            return msg;
        }
        private set {}
    }

    /*
    static void ActivateClient(MessageClient client) {
        client.gameObject.SetActive(true);
    }
    */

    static void DeactivateClient(MessageClient client) {
        client.gameObject.SetActive(false);
        Debug.Log("deactivate");
    }
}
