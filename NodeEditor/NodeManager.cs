using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeManager<T, J, K> : MonoBehaviour where T : NodeGraph<J, K> where J : Node<K> where K : NodeOption {

  protected T currentGraph;
  protected J currentNode;

  virtual public bool SetNodeGraph(T graph) {
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

  protected virtual void SetNode(J node) {
    OnNodeGraphUIUpdate(node);
  }

  public void SelectNodeOption(int option) {
    SetNode(currentNode.options[option - 1].next);
  }

  J GetNode(int id) {
    foreach (J node in currentGraph.nodes) {
      if (node.id == id) {
        return node;
      }
    }
    return null;
  }

  virtual protected void OnNodeGraphUIUpdate(J node) { }

}