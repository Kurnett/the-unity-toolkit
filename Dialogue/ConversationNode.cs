using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConversationNode : Node {
  // Conversation Data
  public Speaker speaker;
  public string text;
  public float length;
  public bool autoProceed;
  public bool endConversation;

  public string title = "";

  protected override void DrawHeader() {
    bool diff = false;
    EditorStyles.textField.wordWrap = true;

    // Adds spacing to let users click and drag.
    GUILayout.Box("", GUIStyle.none);
    Speaker speakerNew = (Speaker)EditorGUILayout.ObjectField(speaker, typeof(Speaker), false);

    bool startNew = EditorGUILayout.ToggleLeft("Start Conv.", start);
    bool endConversationNew = EditorGUILayout.ToggleLeft("End Conv.", endConversation);
    bool autoProceedNew = EditorGUILayout.ToggleLeft("Auto-Proceed", autoProceed);
    GUILayout.Label("Auto-Length");
    float lengthNew = EditorGUILayout.FloatField(length);

    // Check if the conversation node has a default next node.

    if (defaultOption == null || defaultOption.next == -1) {
      if (GUILayout.Button("+")) {
        OnClickOption(defaultOption);
      }
    } else {
      if (GUILayout.Button("-")) {
        defaultOption.RemoveConnection();
      }
    }

    GUILayout.Label("Dialogue");
    text = EditorGUILayout.TextArea(text, GUILayout.Height(90));
    // Check if conversation needs to be saved.
    if (speaker != speakerNew) {
      speaker = speakerNew;
      diff = true;
    }
    if (start != startNew) {
      if (startNew) graph.SetStartNode(this);
      diff = true;
    }
    if (endConversation != endConversationNew) {
      endConversation = endConversationNew;
      diff = true;
    }
    if (autoProceed != autoProceedNew) {
      autoProceed = autoProceedNew;
      diff = true;
    }
    if (length != lengthNew) {
      length = lengthNew;
      diff = true;
    }
    if (diff) SaveGraph(graph);
  }

  protected override void DrawOptionControlsCenter(NodeOption option) {
    ConversationOption convOption = (ConversationOption)option;
    convOption.response = EditorGUILayout.TextArea(convOption.response, GUILayout.Width(140), GUILayout.Height(60));
  }

  protected override void AddOption() {
    ConversationOption newOption = (ConversationOption)ScriptableObject.CreateInstance(typeof(ConversationOption));
    newOption.Construct((NodeGraph)graph, SaveGraph);
    AssetDatabase.AddObjectToAsset(newOption, graph);
    options.Add(newOption);
  }
}