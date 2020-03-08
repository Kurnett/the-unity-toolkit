using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Refactor required tags (i.e. "spirit world" or "not spirit world") to be handled with inheritance.

public class ConversationHandler : MonoBehaviour {

  public bool requireTag = false;
  public string tag = "";

  public List<Conversation> conversations = new List<Conversation>();
  public string defaultConversation = "";
  HOCH_DialogueManager manager;

  Conversation GetConversation(string name) {
    foreach (Conversation conversation in conversations) {
      if (conversation.name == name) {
        return conversation;
      }
    }
    return null;
  }

  void FindDialogueManager() {
    if (!manager) {
      HOCH_DialogueManager[] managers = FindObjectsOfType(typeof(HOCH_DialogueManager)) as HOCH_DialogueManager[];
      if (managers.Length > 0) {
        manager = managers[0];
      }
    }
  }

  public void StartConversation() {
    FindDialogueManager();
    if (manager) {
      Conversation conversation = GetConversation(defaultConversation);
      if (conversation != null) {
        manager.StartConversation(conversation, gameObject);
      }
    }
  }

  public void StartConversation(string conversationName) {
    FindDialogueManager();
    if (manager) {
      if (manager.dialogueTag == tag || !requireTag) {
        Conversation conversation = GetConversation(conversationName);
        if (conversation != null) {
          manager.StartConversation(conversation, gameObject);
        }
      }
    }
  }

}