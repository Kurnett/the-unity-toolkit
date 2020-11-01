using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : MonoBehaviour {

  public List<Dialogue> conversations = new List<Dialogue>();
  public Dialogue defaultConversation;
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

  virtual public void StartConversation() {
    if (manager) {
      if (defaultConversation != null) {
        manager.StartConversation(defaultConversation);
      }
    }
  }

  virtual public void StartConversation(Dialogue conversation) {
    if (manager) {
      manager.StartConversation(conversation);
    }
  }

}