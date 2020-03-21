using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Add ability to start conversation on specific node.
// TODO: Remove unnecessary GetConversation overloads.
// TODO: Move pressKeyToSkip and inputs to inherited class.

public class DialogueManager : MonoBehaviour {

  public bool pressKeyToSkip = true;

  public Conversation currentConversation;
  public ConversationNode currentNode;
  float currentNodeStartTime;

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

  void CheckNodeProgress() {
    if (currentNode != null) {
      if (pressKeyToSkip ? Input.GetButtonDown("Interact") : Time.time - currentNodeStartTime > currentNode.length) {
        if (currentNode.endConversation) {
          EndConversation();
        } else if (currentNode.autoProceed) {
          SetNode(currentNode.defaultOption.next);
        }
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

  Conversation GetConversation(string name) {
    foreach (Conversation conversation in conversations) {
      if (conversation.name == name) {
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