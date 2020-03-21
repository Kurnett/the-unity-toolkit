using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeOption : ScriptableObject {
  public int next;
  public Rect rect;

  // Needs initialization
  public NodeGraph graph;
  protected Action<NodeGraph> SaveGraph;

  virtual public void Construct(
    NodeGraph graph,
    Action<NodeGraph> SaveGraph
  ) {
    this.next = -1;
    this.graph = graph;
    this.SaveGraph = SaveGraph;
  }

  public void Initialize(
    NodeGraph graph,
    Action<NodeGraph> SaveGraph
  ) {
    this.graph = graph;
    this.SaveGraph = SaveGraph;
  }

  public void CreateConnection(Node node) {
    this.next = node.id;
    SaveGraph(graph);
  }

  public void RemoveConnection() {
    this.next = -1;
    SaveGraph(graph);
  }

  public override string ToString() {
    return "next: " + next.ToString();
  }
}