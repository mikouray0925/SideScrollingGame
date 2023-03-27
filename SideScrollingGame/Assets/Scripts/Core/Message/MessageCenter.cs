using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter : MonoBehaviour
{
    public static HashSet<MessageClient> allClients = new HashSet<MessageClient>();

    public static void SpreadGlobalMsg(string methodName, object value = null) {
        foreach (MessageClient client in allClients) {
            client.ReceiveMsg(methodName, value, SendMessageOptions.DontRequireReceiver);
        } 
    }

    public static void SpreadGlobalMsg(Message msg) {
        msg.option = SendMessageOptions.DontRequireReceiver;
        foreach (MessageClient client in allClients) {
            client.ReceiveMsg(msg);
        } 
    }
}
