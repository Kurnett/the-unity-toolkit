using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : NodeManager<Dialogue, DialogueNode, DialogueOption> {

  void Start() {
    currentGraph = null;
  }

  void Update() {
    if (currentGraph != null) {
      CheckNodeProgress();
    }
  }

  virtual public bool StartDialogue(Dialogue dialogue) {
    return SetNodeGraph(dialogue);
  }

  virtual public void EndDialogue() {
    base.CloseNodeGraph();
    Invoke("ClearDialogue", 1);
  }

  void ClearDialogue() {
    base.ClearNodeGraph();
  }

  protected override void SetNode(int id) {
    base.SetNode(id);
  }

  protected override void SetNode(DialogueNode node) {
    base.SetNode(node);
  }

  virtual protected void CheckNodeProgress() {
    DialogueNode currentConvNode = (DialogueNode)currentNode;
    if (currentConvNode != null) {
      if (currentConvNode.endDialogue) {
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