using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationHandler : MonoBehaviour {

    public bool requireTag = false;
    public string tag = "";

    public List<Conversation> conversations = new List<Conversation>();
    public string defaultConversation = "";
    DialogueManager manager;

    Conversation GetConversation (string name) {
        foreach (Conversation conversation in conversations) {
            if (conversation.name == name) {
                return conversation;
            }
        }
        return null;
    }

    void FindDialogueManager () {
        if (!manager) {
            DialogueManager[] managers = FindObjectsOfType (typeof (DialogueManager)) as DialogueManager[];
            if (managers.Length > 0) {
                manager = managers[0];
            }
        }
    }

    public void StartConversation () {
        FindDialogueManager();
        if (manager) {
            Conversation conversation = GetConversation(defaultConversation);
            if (conversation != null) {
                manager.StartConversation (conversation, gameObject);
            }
        }
    }

    public void StartConversation (string conversationName) {
        FindDialogueManager();
        if (manager) {
            if (manager.dialogueTag == tag || !requireTag) {
                Conversation conversation = GetConversation(conversationName);
                if (conversation != null) {
                    manager.StartConversation (conversation, gameObject);
                }
            }
        }
    }

}