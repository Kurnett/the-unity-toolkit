using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDialogue : MonoBehaviour {

  public Conversation conversation;
  ConversationHandler handler;

  void Start() {
    handler = gameObject.GetComponent<ConversationHandler>();
  }

  public void Interact() {
    if (conversation == null) {
      handler.StartConversation();
    } else {
      handler.StartConversation(conversation);
    }
  }
}