using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueNodeRenderer : NodeRenderer<Dialogue, DialogueNode, DialogueOption, Flag> {

  protected override void DrawBody(DialogueNode node) {
    bool diff = false;
    EditorStyles.textField.wordWrap = true;
    
    GUILayout.Label("Speaker");
    Speaker speakerNew = (Speaker)EditorGUILayout.ObjectField(node.speaker, typeof(Speaker), false);
    GUILayout.Label("Flag (Opt.)");
    Flag textFlagNew = (Flag)EditorGUILayout.ObjectField(node.textFlag, typeof(Flag), false);

    bool startNew = EditorGUILayout.ToggleLeft("Conv. Start", node.start);

    node.flagDropdownOpen = EditorGUILayout.Foldout(node.flagDropdownOpen, "Flags", true);
    if (node.flagDropdownOpen) {
      GUILayout.Label("Entry");
      int shouldRemoveEntryAtIndex = -1;
      for (int i = 0; i < node.entryFlags.Count; i++) {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("-")) {
          shouldRemoveEntryAtIndex = i;
        }
        Flag eventFlag = (Flag)EditorGUILayout.ObjectField(node.entryFlags[i], typeof(Flag), false);
        if (eventFlag != node.entryFlags[i]) {
          diff = true;
        }
        node.entryFlags[i] = eventFlag;
        EditorGUILayout.EndHorizontal();
      }
      Flag entryFlagNew = (Flag)EditorGUILayout.ObjectField(null, typeof(Flag), false);
      if (entryFlagNew) {
        node.entryFlags.Add(entryFlagNew);
        diff = true;
      }
      if (shouldRemoveEntryAtIndex != -1) {
        node.entryFlags.Remove(node.entryFlags[shouldRemoveEntryAtIndex]);
        diff = true;
      }
      GUILayout.Label("Exit");
      int shouldRemoveExitAtIndex = -1;
      for (int i = 0; i < node.exitFlags.Count; i++) {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("-")) {
          shouldRemoveExitAtIndex = i;
        }
        Flag eventFlag = (Flag)EditorGUILayout.ObjectField(node.exitFlags[i], typeof(Flag), false);
        if (eventFlag != node.exitFlags[i]) {
          diff = true;
        }
        node.exitFlags[i] = eventFlag;
        EditorGUILayout.EndHorizontal();
      }
      Flag exitFlagNew = (Flag)EditorGUILayout.ObjectField(null, typeof(Flag), false);
      if (exitFlagNew) {
        node.exitFlags.Add(exitFlagNew);
        diff = true;
      }
      if (shouldRemoveExitAtIndex != -1) {
        node.exitFlags.Remove(node.exitFlags[shouldRemoveExitAtIndex]);
        diff = true;
      }
    }

    GUILayout.Label("Dialogue");
    node.text = EditorGUILayout.TextArea(node.text, GUILayout.Height(90));

    GUILayout.Label("Options");
    // Check if the conversation node has a default next node.
    if (node.defaultOption == null || node.defaultOption.next == -1) {
      if (GUILayout.Button("Add Default")) {
        OnClickOption(node.defaultOption);
      }
    } else {
      if (GUILayout.Button("Remove Default")) {
        OnRemoveConnection(node.defaultOption);
      }
    }

    if (node.defaultOption.next != -1) {
      bool autoProceedNew = EditorGUILayout.ToggleLeft("Auto-Proceed", node.autoProceed);
      // Check if conversation needs to be saved.
      if (node.autoProceed != autoProceedNew) {
        node.autoProceed = autoProceedNew;
        diff = true;
      }
      if (node.autoProceed) {
        GUILayout.Label("Timer (seconds)");
        float lengthNew = EditorGUILayout.FloatField(node.length);
        // Check if conversation needs to be saved.
        if (node.length != lengthNew) {
          node.length = lengthNew;
          diff = true;
        }
      }
    }

    // Check if conversation needs to be saved.
    if (node.speaker != speakerNew) {
      node.speaker = speakerNew;
      diff = true;
    }
    if (node.textFlag != textFlagNew) {
      node.textFlag = textFlagNew;
      diff = true;
    }
    if (node.start != startNew) {
      if (startNew) graph.SetStartNode(node);
      diff = true;
    }
    if (diff) SaveGraph(graph);
  }

  protected override void DrawOptionControlsCenter(DialogueNode node, int i) {
    DialogueOption convOption = (DialogueOption)node.options[i];
    if (convOption) {
      convOption.response = EditorGUILayout.TextArea(convOption.response, GUILayout.Width(node.width - 80), GUILayout.Height(60));
    }
  }

}
