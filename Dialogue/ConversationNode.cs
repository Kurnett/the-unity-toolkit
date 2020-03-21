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

  // public ConversationNode(
  //    int id,
  //    Vector2 position,
  //    NodeGraph graph,
  //    Action<NodeOption> OnClickOption,
  //    Action<Node> OnClickNode,
  //    Action<Node> OnRemoveNode,
  //    Action<NodeGraph> SaveGraph
  //  ) : base(id, position, graph, OnClickOption, OnClickNode, OnRemoveNode, SaveGraph) { }

  // override public void Construct(
  //    int id,
  //    Vector2 position,
  //    NodeGraph graph,
  //    Action<NodeOption> OnClickOption,
  //    Action<Node> OnClickNode,
  //    Action<Node> OnRemoveNode,
  //    Action<NodeGraph> SaveGraph
  //  ) {
  //   base(id, position, graph, OnClickOption, OnClickNode, OnRemoveNode, SaveGraph);
  // }

  override public void Draw() {
    bool diff = false;
    EditorStyles.textField.wordWrap = true;

    GUILayout.BeginArea(new Rect(rect.x, rect.y, 250f, Screen.height * 3));
    containerRect = (Rect)EditorGUILayout.BeginVertical("Box");

    // Adds spacing to let users click and drag.
    GUILayout.Box("", GUIStyle.none);
    Speaker speakerNew = (Speaker)EditorGUILayout.ObjectField(speaker, typeof(Speaker), false);

    bool startNew = EditorGUILayout.ToggleLeft("Start Conv.", start);
    bool endConversationNew = EditorGUILayout.ToggleLeft("End Conv.", endConversation);
    bool autoProceedNew = EditorGUILayout.ToggleLeft("Auto-Proceed", autoProceed);
    Rect autoNextRect = EditorGUILayout.BeginHorizontal();
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
    EditorGUILayout.EndHorizontal();

    GUILayout.Label("Dialogue");
    text = EditorGUILayout.TextArea(text, GUILayout.Height(90));


    for (int i = 0; i < options.Count; i++) {
      ConversationOption option = (ConversationOption)options[i];
      EditorGUILayout.BeginHorizontal();

      EditorGUILayout.BeginVertical();
      if (GUILayout.Button("↑", GUILayout.Width(30))) { MoveOption(option, -1); }
      if (GUILayout.Button("↓", GUILayout.Width(30))) { MoveOption(option, 1); }
      EditorGUILayout.EndVertical();

      option.response = EditorGUILayout.TextArea(option.response, GUILayout.Width(140), GUILayout.Height(60));
      if (GUILayout.Button("R", GUILayout.Width(30))) { RemoveOption(option); }
      if (option.next == -1) {
        if (GUILayout.Button("+", GUILayout.Width(30))) { OnClickOption(option); }
      } else {
        if (GUILayout.Button("-", GUILayout.Width(30))) { option.RemoveConnection(); }
      }

      EditorGUILayout.EndHorizontal();
    }

    if (GUILayout.Button("Add Response")) {
      AddOption();
      GUI.changed = true;
    }

    EditorGUILayout.EndVertical();
    GUILayout.EndArea();

    // TODO: Figure out a way to position handles correctly with the new GUI system.
    if (defaultOption != null) {
      defaultOption.rect = autoNextRect;
      ConversationNode autoNextNode = graph.GetNodeById(defaultOption.next) as ConversationNode;
      if (autoNextNode != null) {
        Handles.DrawBezier(
          autoNextRect.center,
          autoNextNode.rect.position,
          autoNextRect.center - Vector2.left * 50f,
          autoNextNode.rect.position + Vector2.left * 50f,
          Color.white,
          null,
          2f
        );
      }
    }
    for (int i = 0; i < options.Count; i++) {
      ConversationOption option = (ConversationOption)options[i];
      ConversationNode nextNode = graph.GetNodeById(option.next) as ConversationNode;
      Rect addRect = new Rect(rect.x + 130f, rect.y, 30f, 30f);
      option.rect = addRect;

      if (nextNode != null) {
        Handles.DrawBezier(
          addRect.center,
          nextNode.rect.position,
          addRect.center - Vector2.left * 50f,
          nextNode.rect.position + Vector2.left * 50f,
          Color.white,
          null,
          2f
        );
      }
    }

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

  protected override void AddOption() {
    // ConversationOption newOption = new ConversationOption((NodeGraph)graph, SaveGraph);
    ConversationOption newOption = (ConversationOption)ScriptableObject.CreateInstance(typeof(ConversationOption));
    newOption.Construct((NodeGraph)graph, SaveGraph);
    AssetDatabase.AddObjectToAsset(newOption, graph);
    options.Add(newOption);
  }
}