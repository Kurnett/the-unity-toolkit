using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDialogue : MonoBehaviour {

    public string conversationName = "";
    ConversationHandler handler;

    void Start () {
        handler = gameObject.GetComponent<ConversationHandler>();
    }

    public void Interact () {
        if (conversationName == "") {
            handler.StartConversation();
        } else {
            handler.StartConversation(conversationName);
        }
    }
}