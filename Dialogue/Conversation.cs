using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// TODO: Allow reordering response options.
// TODO: Restyle dialogue nodes to match normal Unity styles.

[CreateAssetMenu(menuName = "Dialogue/Conversation")]
public class Conversation : ScriptableObject {
  public int id;
  public string name;
  public List<ConversationNode> dialogue = new List<ConversationNode>();
  // TODO: Add error handling for conversations with no start node defined.
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

  public bool isDragged;
  public bool isSelected;

  public float width = 200f;
  public float height = 30f;

  public GUIStyle style;
  public GUIStyle defaultNodeStyle;
  public GUIStyle selectedNodeStyle;

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
    GUIStyle nodeStyle,
    GUIStyle selectedStyle,
    Conversation conversation,
    Action<ConversationOption> OnClickOption,
    Action<ConversationNode> OnClickNode,
    Action<ConversationNode> OnClickRemoveNode,
    Action<Conversation> SaveConversation
  ) {
    this.id = id;
    rect = new Rect(position.x, position.y, width, height);
    style = nodeStyle;
    var obj = this;
    defaultNodeStyle = nodeStyle;
    selectedNodeStyle = selectedStyle;
    this.conversation = conversation;
    this.OnClickNode = OnClickNode;
    this.OnClickOption = OnClickOption;
    this.OnRemoveNode = OnClickRemoveNode;
    this.SaveConversation = SaveConversation;
    autoOption = new ConversationOption();
    options = new List<ConversationOption>();
  }

  public void Initialize(
    Conversation conversation,
    GUIStyle nodeStyle,
    GUIStyle selectedStyle,
    Action<ConversationOption> OnClickOption,
    Action<ConversationNode> OnClickNode,
    Action<ConversationNode> OnClickRemoveNode,
    Action<Conversation> SaveConversation
  ) {
    this.conversation = conversation;
    defaultNodeStyle = nodeStyle;
    selectedNodeStyle = selectedStyle;
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
    EditorStyles.textField.wordWrap = true;
    float spacing = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2;
    float offset = 220f;
    GUI.Box(new Rect(rect.x, rect.y, 150f, 30f), "", style);

    Speaker speakerNew = (Speaker)EditorGUI.ObjectField(new Rect(rect.x, rect.y + 30f, 150f, 20f), speaker, typeof(Speaker), false);
    if (speaker != speakerNew) {
      speaker = speakerNew;
      diff = true;
    }

    // TODO: Prevent a conversation from having multiple default starting points.
    bool startConversationNew = GUI.Toggle(new Rect(rect.x, rect.y + 65f, 100f, 30f), startConversation, "Start Conv.");
    if (startConversation != startConversationNew) {
      startConversation = startConversationNew;
      diff = true;
    }

    bool endConversationNew = GUI.Toggle(new Rect(rect.x, rect.y + 85f, 100f, 30f), endConversation, "End Conv.");
    if (endConversation != endConversationNew) {
      endConversation = endConversationNew;
      diff = true;
    }

    bool autoProceedNew = GUI.Toggle(new Rect(rect.x, rect.y + 105f, 100f, 30f), autoProceed, "Auto-Proceed");
    if (autoProceed != autoProceedNew) {
      autoProceed = autoProceedNew;
      diff = true;
    }

    GUI.Box(new Rect(rect.x, rect.y + 120f, 80f, 30f), "Length");
    float lengthNew = EditorGUI.FloatField(new Rect(rect.x + 80f, rect.y + 120f, 40f, 30f), length);
    if (length != lengthNew) {
      length = lengthNew;
      diff = true;
    }

    text = EditorGUI.TextField(new Rect(rect.x, rect.y + 160f, 150f, 60f), text);
    Rect autoNextRect = new Rect(rect.x + 150f, rect.y + 160f, 30f, 60f);
    if (autoOption.next == -1) {
      if (GUI.Button(autoNextRect, "+")) {
        OnClickOption(autoOption);
      }
    } else {
      if (GUI.Button(autoNextRect, "-")) {
        autoOption.RemoveConnection();
      }
    }
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
      Rect removeRect = new Rect(rect.x - 30f, rect.y + offset + (spacing * i), 30f, spacing);
      Rect addRect = new Rect(rect.x + 130f, rect.y + offset + (spacing * i), 30f, spacing);
      Rect responseRect = new Rect(rect.x, rect.y + offset + (spacing * i), 130f, spacing);
      option.response = EditorGUI.TextField(responseRect, option.response);
      // TODO: Move conversation option rectangle definitions to centralized location.
      option.rect = addRect;
      GUI.Box(removeRect, "R");
      if (option.next == -1) {
        if (GUI.Button(addRect, "+")) {
          OnClickOption(option);
        }
      } else {
        if (GUI.Button(addRect, "-")) {
          option.RemoveConnection();
        }
      }

      ConversationNode nextNode = conversation.GetNodeById(option.next);

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
    GUI.Box(new Rect(rect.x, rect.y + offset + (spacing * options.Count), 130f, spacing), "Add Response");

    if (diff) SaveConversation(conversation);
  }

  public bool ProcessEvents(Event e) {
    float spacing = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2;
    Rect dragRect = new Rect(rect.x, rect.y, 150f, 30f);
    Rect addOptionRect = new Rect(rect.x, rect.y + 220f + (spacing * options.Count), 130f, spacing);
    switch (e.type) {
      case EventType.MouseDown:
        ConversationOption removedOption = null;
        for (int i = 0; i < options.Count; i++) {
          ConversationOption option = options[i];
          Rect optionRect = new Rect(rect.x - 30f, rect.y + 220f + (spacing * i), 30f, spacing);
          if (optionRect.Contains(e.mousePosition)) {
            removedOption = option;
          }
        }
        if (e.button == 0) {
          if (dragRect.Contains(e.mousePosition)) {
            isDragged = true;
            GUI.changed = true;
            isSelected = true;
            style = selectedNodeStyle;
            OnClickNode(this);
          } else if (addOptionRect.Contains(e.mousePosition)) {
            AddOption();
            GUI.changed = true;
          } else if (removedOption != null) {
            options.Remove(removedOption);
            GUI.changed = true;
          } else {
            GUI.changed = true;
            isSelected = false;
            style = defaultNodeStyle;
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

  private void ReorderOption() {
    // TODO: Add option reordering functionality.
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