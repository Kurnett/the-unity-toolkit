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

  virtual public bool StartConversation(Dialogue conversation) {
    return SetNodeGraph(conversation);
  }

  virtual public void EndConversation() {
    base.CloseNodeGraph();
    Invoke("ClearConversation", 1);
  }

  void ClearConversation() {
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
      if (currentConvNode.endConversation) {
        EndConversation();
      } else if (currentConvNode.autoProceed) {
        SetNode(currentConvNode.defaultOption.next);
      }
    }
  }

  public void Respond(int option) {
    base.SelectNodeOption(option);
  }

  override protected void OnNodeGraphUIUpdate(DialogueNode node) {
    OnConversationUIUpdate(node);
  }

  virtual public void OnConversationUIUpdate(DialogueNode node) { }

}