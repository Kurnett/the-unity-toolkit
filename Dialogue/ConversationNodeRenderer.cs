using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConversationNodeRenderer : NodeRenderer<ConversationNode> {

  public ConversationNodeRenderer(ConversationNode nodeInit) : base(nodeInit) { }

  protected override void DrawHeader() {
    bool diff = false;
    EditorStyles.textField.wordWrap = true;

    // Adds spacing to let users click and drag.
    GUILayout.Box("", GUIStyle.none);
    Speaker speakerNew = (Speaker)EditorGUILayout.ObjectField(node.speaker, typeof(Speaker), false);

    bool startNew = EditorGUILayout.ToggleLeft("Start Conv.", node.start);
    bool endConversationNew = EditorGUILayout.ToggleLeft("End Conv.", node.endConversation);
    bool autoProceedNew = EditorGUILayout.ToggleLeft("Auto-Proceed", node.autoProceed);
    GUILayout.Label("Auto-Length");
    float lengthNew = EditorGUILayout.FloatField(node.length);

    // Check if the conversation node has a default next node.

    if (node.defaultOption == null || node.defaultOption.next == -1) {
      if (GUILayout.Button("+")) {
        node.OnClickOption(node.defaultOption);
      }
    } else {
      if (GUILayout.Button("-")) {
        node.defaultOption.RemoveConnection();
      }
    }

    GUILayout.Label("Dialogue");
    node.text = EditorGUILayout.TextArea(node.text, GUILayout.Height(90));
    // Check if conversation needs to be saved.
    if (node.speaker != speakerNew) {
      node.speaker = speakerNew;
      diff = true;
    }
    if (node.start != startNew) {
      if (startNew) node.graph.SetStartNode(this.node);
      diff = true;
    }
    if (node.endConversation != endConversationNew) {
      node.endConversation = endConversationNew;
      diff = true;
    }
    if (node.autoProceed != autoProceedNew) {
      node.autoProceed = autoProceedNew;
      diff = true;
    }
    if (node.length != lengthNew) {
      node.length = lengthNew;
      diff = true;
    }
    if (diff) node.SaveGraph(node.graph);
  }

  protected override void DrawOptionControlsCenter(int i) {
    ConversationOption convOption = (ConversationOption)node.options[i];
    convOption.response = EditorGUILayout.TextArea(convOption.response, GUILayout.Width(140), GUILayout.Height(60));
  }

}
