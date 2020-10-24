using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConversationRenderer : NodeGraphRenderer<DialogueEditor, Conversation, ConversationNode> {

  public ConversationRenderer(DialogueEditor editorInit) : base(editorInit) { }

  public override void DrawNodeGraph(Conversation graph) {
    if (graph.nodes != null) {
      foreach (ConversationNode node in graph.nodes) {
        ConversationNodeRenderer r = new ConversationNodeRenderer(editor, graph);
        r.DrawNode(node);
      }
    }
  }
}