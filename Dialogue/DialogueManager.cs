using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : NodeManager<Dialogue, DialogueNode, DialogueOption, Flag> {

  // Necessary to avoid instantly restarting a conversation when the user
  // presses the "interact" button to end the conversation.
  bool conversationDidEnd = false;

  void Start() {
    currentGraph = null;
  }

  protected void Update() {
    if (conversationDidEnd) {
      conversationDidEnd = false;
    }
    if (currentGraph != null) {
      CheckNodeProgress();
    }
  }

  virtual public bool StartDialogue(Dialogue dialogue) {
    if (conversationDidEnd) {
      return false;
    }
    bool success = SetNodeGraph(dialogue);
    if (success) {
      OnDialogueUIUpdate(currentNode);
    }
    return success;
  }

  virtual public void EndDialogue() {
    ClearDialogue();
    conversationDidEnd = true;
  }

  void ClearDialogue() {
    base.ClearNodeGraph();
  }

  virtual protected void CheckNodeProgress() { }

  virtual protected void ProgressNode () {
    if (currentNode != null) {
      if (
        (currentNode.autoProceed && currentNode.defaultOption.next == -1)
        ||
        (!currentNode.autoProceed && currentNode.options.Count == 0 && currentNode.defaultOption.next == -1)
      ) {
        EndDialogue();
      } else if (currentNode.defaultOption.next != -1) {
        SetNode(currentNode.defaultOption.next);
      }
    }
  }

  public void Respond(int option) {
    if (currentNode.options[option - 1].next == -1) {
      EndDialogue();
    } else {
      base.SelectNodeOption(option);
    }
  }

  override protected void OnNodeGraphUIUpdate(DialogueNode node) {
    OnDialogueUIUpdate(node);
  }

  virtual public void OnDialogueUIUpdate(DialogueNode node) { }

  override protected void SetNode(int id) {
    base.SetNode(id);
  }

}