using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : NodeManager<Dialogue, DialogueNode, DialogueOption, NodeSideEffect> {

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
    return SetNodeGraph(dialogue);
  }

  virtual public void EndDialogue() {
    ClearDialogue();
    conversationDidEnd = true;
  }

  void ClearDialogue() {
    base.ClearNodeGraph();
  }

  // protected override void SetNode(int id) {
  //   base.SetNode(id);
  // }

  // protected override void SetNode(DialogueNode node) {
  //   base.SetNode(node);
  // }

  virtual protected void CheckNodeProgress() {
    DialogueNode currentConvNode = (DialogueNode)currentNode;
    if (currentConvNode != null) {
      if (
        (currentNode.autoProceed && currentNode.defaultOption.next == -1)
        ||
        (!currentNode.autoProceed && currentNode.options.Count == 0)
      ) {
        EndDialogue();
      } else if (currentConvNode.autoProceed) {
        SetNode(currentConvNode.defaultOption.next);
      }
    }
  }

  public void Respond(int option) {
    base.SelectNodeOption(option);
  }

  override protected void OnNodeGraphUIUpdate(DialogueNode node) {
    OnDialogueUIUpdate(node);
  }

  virtual public void OnDialogueUIUpdate(DialogueNode node) { }

}