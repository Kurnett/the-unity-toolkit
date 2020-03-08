using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Add ability to start conversation on specific node.
// TODO: Move all UI logic to inherited class.

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
      CheckResponse();
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
    gameObject.SendMessage("CleanupConversationUI");
    Invoke("ClearConversation", 1);
  }

  void ClearConversation() {
    currentConversation = null;
  }

  void SetNode(int id) {
    currentNode = GetNode(id);
    currentNodeStartTime = Time.time;
    // TODO: Convert SendMessage to method call in inherited class.
    gameObject.SendMessage("UpdateConversationUI", currentNode);
  }

  void SetNode(ConversationNode node) {
    currentNodeStartTime = Time.time;
    // TODO: Move SendMessage to inherited class.
    gameObject.SendMessage("UpdateConversationUI", node);
  }

  void CheckNodeProgress() {
    if (currentNode != null) {
      if (pressKeyToSkip ? Input.GetButtonDown("Interact") : Time.time - currentNodeStartTime > currentNode.length) {
        if (currentNode.endConversation) {
          EndConversation();
        } else if (currentNode.autoProceed) {
          SetNode(currentNode.autoOption.next);
        }
      }
    }
  }

  // TODO: Add handling for clicking on response options in the UI.
  // TODO: Convert number key response handling from conditional statements to a loop.
  // TODO: Move custom response handling to inherited class.

  void CheckResponse() {
    if (Input.GetKeyDown(KeyCode.Alpha1)) {
      Respond(1);
    }
    if (Input.GetKeyDown(KeyCode.Alpha2)) {
      Respond(2);
    }
    if (Input.GetKeyDown(KeyCode.Alpha3)) {
      Respond(3);
    }
    if (Input.GetKeyDown(KeyCode.Alpha4)) {
      Respond(4);
    }
    if (Input.GetKeyDown(KeyCode.Alpha5)) {
      Respond(5);
    }
  }

  void Respond(int option) {
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
    foreach (ConversationNode node in currentConversation.dialogue) {
      if (node.id == id) {
        return node;
      }
    }
    return null;
  }

}