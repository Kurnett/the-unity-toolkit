using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler<DIALOGUE, DIALOGUE_MANAGER> : MonoBehaviour
  where DIALOGUE : Dialogue
  where DIALOGUE_MANAGER : DialogueManager {

  public DIALOGUE_MANAGER manager;

  [SerializeField]
  List<FlagEffectHandler> effectHandlers = new List<FlagEffectHandler>();
  [SerializeField]
  List<FlagEffectHandler> textFlags = new List<FlagEffectHandler>();

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
      foreach (FlagEffectHandler handler in effectHandlers) {
        handler.HandleFlag(effect);
      }
    }
  }

  virtual public void GetTextFlag(Flag flag) {
      foreach (FlagEffectHandler handler in effectHandlers) {
        handler.HandleFlag(flag);
      }
  }

}