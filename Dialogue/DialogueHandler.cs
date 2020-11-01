using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : MonoBehaviour {

  public List<Dialogue> dialogues = new List<Dialogue>();
  public Dialogue defaultDialogue;
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

  virtual public void StartDialogue() {
    if (manager) {
      if (defaultDialogue != null) {
        manager.StartDialogue(defaultDialogue);
      }
    }
  }

  virtual public void StartDialogue(Dialogue dialogue) {
    if (manager) {
      manager.StartDialogue(dialogue);
    }
  }

}