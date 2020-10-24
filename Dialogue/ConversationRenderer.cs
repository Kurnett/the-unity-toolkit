using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConversationRenderer : NodeGraphRenderer<Conversation, ConversationNode> {
  public override void DrawNodeGraph(Conversation graph) {
    if (graph.nodes != null) {
      foreach (ConversationNode node in graph.nodes) {
        ConversationNodeRenderer r = new ConversationNodeRenderer(node);
        r.DrawNode(node);
      }
    }
  }
}