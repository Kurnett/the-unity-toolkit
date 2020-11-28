using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectiveEditor : NodeEditor<
  ObjectiveGraphRenderer,
  ObjectiveNodeRenderer,
  ObjectiveGraph,
  ObjectiveNode,
  ObjectiveOption,
  NodeSideEffect
> {

  [MenuItem("Window/Objective Editor")]
  private static void OpenWindow() {
    ObjectiveEditor window = GetWindow<ObjectiveEditor>();
    window.titleContent = new GUIContent("Objective Editor");
  }

  protected override string GetNoSelectionMessage() {
    return "Select an objective graph to get started";
  }
}
