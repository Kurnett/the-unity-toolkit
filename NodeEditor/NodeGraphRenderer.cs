using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeGraphRenderer<K, T, J> where K : NodeEditor<T, J> where T : NodeGraph where J : Node {

  protected K editor;

  public NodeGraphRenderer(K editorInit) {
    editor = editorInit;
  }

  public virtual void DrawNodeGraph(T graph) {
    if (graph.nodes != null) {
      foreach (Node node in graph.nodes) {
        NodeRenderer<K, T, J> r = new NodeRenderer<K, T, J>(editor, graph);
        r.DrawNode((J)node);
      }
    }
  }
}