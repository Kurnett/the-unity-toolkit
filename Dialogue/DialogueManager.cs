using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : NodeManager<Conversation, ConversationNode, ConversationOption> {

  protected float currentNodeStartTime;

  public List<Conversation> conversations = new List<Conversation>();

  void Start() {
    currentGraph = null;
  }

  void Update() {
    if (currentGraph != null) {
      CheckNodeProgress();
    }
  }

  virtual public bool StartConversation(Conversation conversation) {
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
    currentNodeStartTime = Time.time;
    base.SetNode(id);
  }

  protected override void SetNode(ConversationNode node) {
    currentNodeStartTime = Time.time;
    base.SetNode(node);
  }

  virtual protected void CheckNodeProgress() {
    ConversationNode currentConvNode = (ConversationNode)currentNode;
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

  Conversation GetConversation(int id) {
    foreach (Conversation conversation in conversations) {
      if (conversation.id == id) {
        return conversation;
      }
    }
    return null;
  }

  override protected void OnNodeGraphUIUpdate(ConversationNode node) {
    OnConversationUIUpdate(node);
  }

  virtual public void OnConversationUIUpdate(ConversationNode node) { }

}