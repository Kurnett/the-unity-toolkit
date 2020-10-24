using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeGraphRenderer<T, J, K> where T : NodeGraph<J, K> where J : Node<K> where K : NodeOption {

  // protected K editor;

  // public NodeGraphRenderer(K editorInit) {
  //   editor = editorInit;
  // }

  public virtual void DrawNodeGraph(T graph) {
    if (graph.nodes != null) {
      foreach (J node in graph.nodes) {
        NodeRenderer<T, J, K> r = new NodeRenderer<T, J, K>(graph);
        r.DrawNode(node);
      }
    }
  }
}