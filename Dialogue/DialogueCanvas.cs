using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueCanvas : MonoBehaviour {

    public Text speaker;
    public Text dialogue;
    public Text options;
    public Text proceed;

    public void UpdateConversation (ConversationNode node) {
        speaker.text = node.speaker.name;
        dialogue.text = node.text;

        if (node.options.Count > 0) {
            proceed.text = "";
        } else {
            proceed.text = node.endConversation ? "Press F to leave." : "Press F to continue...";
        }

        string optionList = "";
        int i = 1;
        foreach (ConversationOption option in node.options) {
            optionList += i + ": " + option.response + "\n";
            i++;
        }

        options.text = optionList;
    }

}