using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueEditor : NodeEditor<
  DialogueRenderer,
  DialogueNodeRenderer,
  Dialogue,
  DialogueNode,
  DialogueOption,
  NodeSideEffect
> {

  [MenuItem("Window/Dialogue Editor")]
  private static void OpenWindow() {
    DialogueEditor window = GetWindow<DialogueEditor>();
    window.titleContent = new GUIContent("Dialogue Editor");
  }

  protected override string GetNoSelectionMessage() {
    return "Select a conversation to get started";
  }
}
