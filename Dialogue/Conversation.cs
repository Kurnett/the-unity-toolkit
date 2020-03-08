using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Dialogue/Conversation")]
public class Conversation : ScriptableObject {
  public int id;
  new public string name;
  public List<ConversationNode> dialogue = new List<ConversationNode>();
  public ConversationNode startNode;

  public int GenerateUniqueId() {
    bool idFound = false;
    int i = 0;
    while (!idFound) {
      if (GetNodeById(i) == null) {
        return i;
      }
      i++;
    }
    return -1;
  }

  public int GetStartNodeID() {
    foreach (ConversationNode node in dialogue) {
      if (node.startConversation) {
        return node.id;
      }
    }
    return -1;
  }

  public ConversationNode GetNodeById(int id) {
    if (id != -1) {
      ConversationNode node = dialogue.Find(nodeInit => nodeInit.id == id);
      if (node != null) {
        return node;
      }
      ClearInvalidOptionIDs(id);
    }
    return null;
  }

  public void ClearInvalidOptionIDs(int id) {
    foreach (ConversationNode node in dialogue) {
      if (node.autoOption.next == id) {
        node.autoOption.next = -1;
      }
      foreach (ConversationOption option in node.options) {
        if (option.next == id) {
          option.next = -1;
        }
      }
    }
  }

  public void SetStartNode(ConversationNode startNode) {
    foreach (ConversationNode node in dialogue) {
      if (startNode == node) {
        node.startConversation = true;
      } else {
        node.startConversation = false;
      }
    }
  }
}

[System.Serializable]
public class ConversationNode {
  // Conversation Data
  public int id;
  public Speaker speaker;
  public string text;
  public float length;
  public bool autoProceed;
  public ConversationOption autoOption;
  public bool startConversation;
  public bool endConversation;
  public List<ConversationOption> options = new List<ConversationOption>();

  // Editor Data
  public ConversationNode node;
  public Rect rect;
  public Rect containerRect;

  public bool isDragged;
  public bool isSelected;

  public float width = 200f;
  public float height = 30f;

  // Needs initialization
  public Conversation conversation;
  public Action<ConversationOption> OnClickOption;
  public Action<ConversationNode> OnClickNode;
  public Action<ConversationNode> OnRemoveNode;
  public Action<Conversation> SaveConversation;

  public string title = "";

  public ConversationNode(
    int id,
    Vector2 position,
    Conversation conversation,
    Action<ConversationOption> OnClickOption,
    Action<ConversationNode> OnClickNode,
    Action<ConversationNode> OnClickRemoveNode,
    Action<Conversation> SaveConversation
  ) {
    this.id = id;
    rect = new Rect(position.x, position.y, width, height);
    autoOption = new ConversationOption();
    options = new List<ConversationOption>();
    Initialize(conversation, OnClickOption, OnClickNode, OnClickRemoveNode, SaveConversation);
  }

  public void Initialize(
    Conversation conversation,
    Action<ConversationOption> OnClickOption,
    Action<ConversationNode> OnClickNode,
    Action<ConversationNode> OnClickRemoveNode,
    Action<Conversation> SaveConversation
  ) {
    this.conversation = conversation;
    this.OnClickNode = OnClickNode;
    this.OnClickOption = OnClickOption;
    this.OnRemoveNode = OnClickRemoveNode;
    this.SaveConversation = SaveConversation;

    if (autoOption == null) { autoOption = new ConversationOption(); }
    autoOption.Initialize(conversation, SaveConversation);
    for (int i = 0; i < options.Count; i++) {
      options[i].Initialize(conversation, SaveConversation);
    }
  }

  public void Drag(Vector2 delta) {
    rect.position += delta;
  }

  public void Draw() {
    bool diff = false;

    GUILayout.BeginArea(new Rect(rect.x, rect.y, 250f, Screen.height));
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
    if (autoOption.next == -1) {
      if (GUILayout.Button("+")) {
        OnClickOption(autoOption);
      }
    } else {
      if (GUILayout.Button("-")) {
        autoOption.RemoveConnection();
      }
    }
    EditorGUILayout.EndHorizontal();

    GUILayout.Label("Dialogue");
    text = EditorGUILayout.TextArea(text);

    // TODO: Restyle option list GUI to make better use of the limited space.
    for (int i = 0; i < options.Count; i++) {
      ConversationOption option = (ConversationOption)options[i];
      EditorGUILayout.BeginHorizontal();

      EditorGUILayout.BeginVertical();
      if (GUILayout.Button("Up")) { MoveOption(option, -1); }
      if (GUILayout.Button("Down")) { MoveOption(option, 1); }
      EditorGUILayout.EndVertical();

      option.response = EditorGUILayout.TextArea(option.response);
      if (GUILayout.Button("R")) { options.Remove(option); }
      if (option.next == -1) {
        if (GUILayout.Button("+")) { OnClickOption(option); }
      } else {
        if (GUILayout.Button("-")) { option.RemoveConnection(); }
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
    autoOption.rect = autoNextRect;
    ConversationNode autoNextNode = conversation.GetNodeById(autoOption.next);
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
      ConversationNode nextNode = conversation.GetNodeById(option.next);
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
      if (startConversationNew) conversation.SetStartNode(this);
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
    if (diff) SaveConversation(conversation);
  }

  public bool ProcessEvents(Event e) {
    Rect dragRect = new Rect(rect.x + containerRect.x, rect.y + containerRect.y, containerRect.width, containerRect.height);
    switch (e.type) {
      case EventType.MouseDown:
        if (e.button == 0) {
          if (dragRect.Contains(e.mousePosition)) {
            isDragged = true;
            GUI.changed = true;
            isSelected = true;
            OnClickNode(this);
          } else {
            GUI.changed = true;
            isSelected = false;
          }
        }

        if (e.button == 1 && dragRect.Contains(e.mousePosition)) {
          ProcessContextMenu();
          e.Use();
        }
        break;
      case EventType.MouseUp:
        isDragged = false;
        break;
      case EventType.MouseDrag:
        if (e.button == 0 && isDragged) {
          Drag(e.delta);
          e.Use();
          return true;
        }
        break;
    }
    return false;
  }

  private void ProcessContextMenu() {
    GenericMenu genericMenu = new GenericMenu();
    genericMenu.AddItem(new GUIContent("Remove conversation node"), false, OnClickRemoveNode);
    genericMenu.ShowAsContext();
  }

  private void OnClickRemoveNode() {
    if (OnRemoveNode != null) {
      OnRemoveNode(this);
    }
  }

  private void AddOption() {
    ConversationOption newOption = new ConversationOption(conversation, SaveConversation);
    options.Add(newOption);
  }

  void MoveOption(ConversationOption option, int diff) {
    int index = options.IndexOf(option);
    int newIndex = Mathf.Clamp(index + diff, 0, options.Count - 1);
    if (index != newIndex) {
      options.RemoveAt(index);
      options.Insert(index + diff, option);
      GUI.changed = true;
    }
  }
}

[System.Serializable]
public class ConversationOption {
  public string response;
  public int next;
  public Rect rect;

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

  public void Initialize(
    Conversation conversation,
    Action<Conversation> SaveConversation
  ) {
    this.conversation = conversation;
    this.SaveConversation = SaveConversation;
  }

  public ConversationOption(string response, int next) {
    this.response = response;
    this.next = next;
  }

  public void CreateConnection(ConversationNode node) {
    this.next = node.id;
    SaveConversation(conversation);
  }

  public void RemoveConnection() {
    this.next = -1;
    SaveConversation(conversation);
  }
}