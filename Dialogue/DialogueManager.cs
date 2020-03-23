using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Add ability to start conversation on specific node.

public class DialogueManager : MonoBehaviour {

  public Conversation currentConversation;
  public ConversationNode currentNode;
  protected float currentNodeStartTime;

  public List<Conversation> conversations = new List<Conversation>();

  void Start() {
    currentConversation = null;
  }

  void Update() {
    if (currentConversation != null) {
      OnConversationResponse();
      CheckNodeProgress();
    }
  }

  virtual public bool StartConversation(Conversation conversation) {
    if (currentConversation == null) {
      currentConversation = conversation;
      int startNodeID = conversation.GetStartNodeID();
      if (startNodeID != -1) {
        SetNode(conversation.GetStartNodeID());
        return true;
      } else {
        Debug.LogWarning("Conversation has no start node.");
      }
    }
    return false;
  }

  virtual public void EndConversation() {
    Invoke("ClearConversation", 1);
  }

  void ClearConversation() {
    currentConversation = null;
  }

  void SetNode(int id) {
    currentNode = GetNode(id);
    currentNodeStartTime = Time.time;
    OnConversationUIUpdate(currentNode);
  }

  void SetNode(ConversationNode node) {
    currentNodeStartTime = Time.time;
    OnConversationUIUpdate(node);
  }

  virtual protected void CheckNodeProgress() {
    if (currentNode != null) {
      if (currentNode.endConversation) {
        EndConversation();
      } else if (currentNode.autoProceed) {
        SetNode(currentNode.defaultOption.next);
      }
    }
  }

  public void Respond(int option) {
    SetNode(currentNode.options[option - 1].next);
  }

  Conversation GetConversation(int id) {
    foreach (Conversation conversation in conversations) {
      if (conversation.id == id) {
        return conversation;
      }
    }
    return null;
  }

  ConversationNode GetNode(int id) {
    foreach (Node node in currentConversation.nodes) {
      ConversationNode cNode = (ConversationNode)node;
      if (cNode.id == id) {
        return cNode;
      }
    }
    return null;
  }

  virtual public void OnConversationUIUpdate(ConversationNode node) { }
  virtual public void OnConversationResponse() { }

}