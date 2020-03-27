using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeManager : MonoBehaviour {

  public NodeGraph currentGraph;
  public Node currentNode;

  virtual public bool SetNodeGraph(NodeGraph graph) {
    if (currentGraph == null) {
      currentGraph = graph;
      int startNodeID = graph.GetStartNodeID();
      if (startNodeID != -1) {
        SetNode(startNodeID);
        return true;
      } else {
        Debug.LogWarning("Node graph has no start node.");
      }
    }
    return false;
  }

  virtual public void CloseNodeGraph() { }

  protected virtual void ClearNodeGraph() {
    currentGraph = null;
  }

  protected virtual void SetNode(int id) {
    currentNode = GetNode(id);
    OnNodeGraphUIUpdate(currentNode);
  }

  protected virtual void SetNode(Node node) {
    OnNodeGraphUIUpdate(node);
  }

  public void SelectNodeOption(int option) {
    SetNode(currentNode.options[option - 1].next);
  }

  Node GetNode(int id) {
    foreach (Node node in currentGraph.nodes) {
      if (node.id == id) {
        return node;
      }
    }
    return null;
  }

  virtual public void OnNodeGraphUIUpdate(Node node) { }

}