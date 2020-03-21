using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: Move DialogueUI to inherit from DialogueManager (HOCH_DialogueManager).

public class DialogueUI : MonoBehaviour {

  public GameObject dialogueCanvas;
  public GameObject currentCanvas;

  public void CreateConversationUI(ConversationNode node) {
    GenerateUI(node);
  }

  public void UpdateConversationUI(ConversationNode node) {
    if (!currentCanvas) { GenerateUI(node); }
    currentCanvas.SendMessage("UpdateConversation", node);
  }

  public void CleanupConversationUI() {
    DestroyUI();
  }

  void GenerateUI(ConversationNode node) {
    if (currentCanvas) {
      Destroy(currentCanvas);
    }
    currentCanvas = Instantiate(dialogueCanvas);
    UpdateConversationUI(node);
  }

  void DestroyUI() {
    if (currentCanvas) {
      Destroy(currentCanvas);
      currentCanvas = null;
    }
  }
}