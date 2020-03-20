﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Dialogue/Conversation")]
public class Conversation : NodeGraph { }

public class ConversationNode : Node {
  // Conversation Data
  public Speaker speaker;
  public string text;
  public float length;
  public bool autoProceed;
  public bool startConversation;
  public bool endConversation;

  public string title = "";

  public ConversationNode(
     int id,
     Vector2 position,
     NodeGraph graph,
     Action<NodeOption> OnClickOption,
     Action<Node> OnClickNode,
     Action<Node> OnRemoveNode,
     Action<NodeGraph> SaveGraph
   ) : base(id, position, graph, OnClickOption, OnClickNode, OnRemoveNode, SaveGraph) { }

  override public void Draw() {
      bool diff = false;
      EditorStyles.textField.wordWrap = true;

      GUILayout.BeginArea(new Rect(rect.x, rect.y, 250f, Screen.height * 3));
      containerRect = (Rect)EditorGUILayout.BeginVertical("Box");

      // Adds spacing to let users click and drag.
      GUILayout.Box("", GUIStyle.none);
      Speaker speakerNew = (Speaker)EditorGUILayout.ObjectField(speaker, typeof(Speaker), false);

      bool startConversationNew = EditorGUILayout.ToggleLeft("Start Conv.", startConversation);
      bool endConversationNew = EditorGUILayout.ToggleLeft("End Conv.", endConversation);
      bool autoProceedNew = EditorGUILayout.ToggleLeft("Auto-Proceed", autoProceed);
      Rect autoNextRect = EditorGUILayout.BeginHorizontal();
      GUILayout.Label("Auto-Length");
      float lengthNew = EditorGUILayout.FloatField(length);

      // Check if the conversation node has a default next node.
      if (defaultOption.next == -1) {
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
        if (GUILayout.Button("R", GUILayout.Width(30))) { options.Remove(option); }
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
      if (startConversation != startConversationNew) {
        if (startConversationNew) graph.SetStartNode(this);
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
}

public class ConversationOption : NodeOption {
  public string response;

  // Needs initialization
  public Conversation conversation;
  private Action<Conversation> SaveConversation;

  public ConversationOption() {
    this.response = "";
    this.next = -1;
    this.conversation = null;
  }

  public ConversationOption(
    Conversation conversation,
    Action<Conversation> SaveConversation
  ) {
    this.response = "";
    this.next = -1;
    this.conversation = conversation;
    this.SaveConversation = SaveConversation;
  }

  public ConversationOption(string response, int next) {
    this.response = response;
    this.next = next;
  }
}