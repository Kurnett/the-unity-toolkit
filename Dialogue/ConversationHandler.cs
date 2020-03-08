using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationHandler : MonoBehaviour {

  public List<Conversation> conversations = new List<Conversation>();
  public Conversation defaultConversation;
  public DialogueManager manager;

  void Start() {
    FindDialogueManager();
  }

  virtual public void FindDialogueManager() {
    if (!manager) {
      DialogueManager[] managers = FindObjectsOfType(typeof(DialogueManager)) as DialogueManager[];
      if (managers.Length > 0) {
        manager = managers[0];
      }
    }
  }

  virtual public bool StartConversation() {
    if (manager) {
      if (defaultConversation != null) {
        manager.StartConversation(defaultConversation);
        return true;
      }
    }
    return false;
  }

  virtual public bool StartConversation(Conversation conversation) {
    if (manager) {
      manager.StartConversation(conversation);
      return true;
    }
    return false;
  }

}