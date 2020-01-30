﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Add ability to start conversation on specific node.

public class DialogueManager : MonoBehaviour {

  GameObject player;

  public string dialogueTag = "";

  public bool pressKeyToSkip = true;

  public Conversation currentConversation;
  public ConversationNode currentNode;
  float currentNodeStartTime;

  public List<Conversation> conversations = new List<Conversation>();

  void Start() {
    player = GameObject.FindWithTag("Player");
    currentConversation = null;
  }

  void Update() {
    if (currentConversation != null) {
      CheckResponse();
      CheckNodeProgress();
    }
  }

  public bool StartConversation(string name) {
    if (currentConversation == null) {
      currentConversation = GetConversation(name);
      SetNode(currentConversation.GetStartNodeID());
      return true;
    }
    return false;
  }

  public bool StartConversation(int id) {
    if (currentConversation == null) {
      currentConversation = GetConversation(id);
      SetNode(currentConversation.GetStartNodeID());
      player.SendMessage("StartDialogue");
      return true;
    }
    return false;
  }

  public bool StartConversation(Conversation conversation, GameObject npc) {
    if (currentConversation == null) {
      currentConversation = conversation;
      SetNode(conversation.GetStartNodeID());
      player.SendMessage("StartDialogue", npc);
      return true;
    }
    return false;
  }

  void EndConversation() {
    gameObject.SendMessage("CleanupConversationUI");
    player.SendMessage("EndDialogue");
    Invoke("ClearConversation", 1);
  }

  void ClearConversation() {
    currentConversation = null;
  }

  void SetNode(int id) {
    currentNode = GetNode(id);
    currentNodeStartTime = Time.time;
    gameObject.SendMessage("UpdateConversationUI", currentNode);
  }

  void SetNode(ConversationNode node) {
    currentNodeStartTime = Time.time;
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