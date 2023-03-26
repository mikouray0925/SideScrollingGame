using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter : MonoBehaviour
{
    public static HashSet<GlobalMsgReceiver> allReceivers = new HashSet<GlobalMsgReceiver>();

    public static void SpreadGlobalMsg(string methodName, object value = null, SendMessageOptions option = SendMessageOptions.DontRequireReceiver) {
        foreach (GlobalMsgReceiver receiver in allReceivers) {
            receiver.ReceiveMsg(methodName, value, option);
        } 
    }
    
    /*
    public static void SpreadGlobalMsg(string methodName, SendMessageOptions option = SendMessageOptions.DontRequireReceiver) {
        foreach (GlobalMsgReceiver receiver in allReceivers) {
            receiver.ReceiveMsg(methodName, option);
        } 
    }
    */
}
