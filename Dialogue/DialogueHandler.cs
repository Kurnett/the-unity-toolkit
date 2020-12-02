using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler<DIALOGUE, DIALOGUE_MANAGER> : MonoBehaviour
  where DIALOGUE : Dialogue
  where DIALOGUE_MANAGER : DialogueManager {

  public DIALOGUE_MANAGER manager;

  [SerializeField]
  List<FlagEventHandler> effectHandlers = new List<FlagEventHandler>();
  [SerializeField]
  List<FlagStringHandler> textFlagHandlers = new List<FlagStringHandler>();

  void Start() {
    FindDialogueManager();
  }

  virtual public void FindDialogueManager() {
    if (!manager) {
      DIALOGUE_MANAGER[] managers = FindObjectsOfType(typeof(DIALOGUE_MANAGER)) as DIALOGUE_MANAGER[];
      if (managers.Length > 0) {
        manager = managers[0];
      }
    }
  }

  virtual public void StartDialogue(DIALOGUE dialogue) {
    if (manager) {
      manager.StartDialogue(dialogue);
    }
  }

  virtual public void HandleSideEffects(List<Flag> effects) {
    foreach (Flag effect in effects) {
      foreach (FlagEventHandler handler in effectHandlers) {
        handler.HandleFlag(effect);
      }
    }
  }

  virtual public string GetTextFlag(Flag flag) {
    foreach (FlagStringHandler handler in textFlagHandlers) {
      if (handler.flag == flag) {
        return handler.HandleFlag(flag);
      }
    }
    return "";
  }

}