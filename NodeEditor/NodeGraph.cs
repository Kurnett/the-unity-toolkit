using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Nodes/Graph")]
public abstract class NodeGraph<T, J> : ScriptableObject where T : Node<J> where J : NodeOption {
  public int id;
  new public string name;
  public List<T> nodes = new List<T>();
  public T startNode;

  public int GenerateUniqueId() {
    bool idFound = false;
    int i = 0;
    while (!idFound) {
      if (GetNodeById(i) == null) {
        return i;
      }
      i++;
    }
    return -1;
  }

  public int GetStartNodeID() {
    foreach (T node in nodes) {
      if (node.start) {
        return node.id;
      }
    }
    return -1;
  }

  public T GetNodeById(int id) {
    if (id != -1) {
      T node = nodes.Find(nodeInit => nodeInit.id == id);
      if (node != null) {
        return node;
      }
      ClearInvalidOptionIDs(id);
    }
    return null;
  }

  public void ClearInvalidOptionIDs(int id) {
    foreach (T node in nodes) {
      if (node.defaultOption != null && node.defaultOption.next == id) {
        node.defaultOption.next = -1;
      }
      foreach (J option in node.options) {
        if (option.next == id) {
          option.next = -1;
        }
      }
    }
  }

  public void SetStartNode(T startNode) {
    foreach (T node in nodes) {
      if (startNode == node) {
        node.start = true;
      } else {
        node.start = false;
      }
    }
  }

  public virtual void AddNode(Vector2 position) {
    T node = (T)ScriptableObject.CreateInstance(typeof(T));
    node.Construct(
      GenerateUniqueId(),
      position
    );
    AssetDatabase.AddObjectToAsset(node, this);
    nodes.Add(node);
  }

  public void RemoveNode(T node) {
    foreach (J option in node.options) {
      AssetDatabase.RemoveObjectFromAsset(option);
    }
    AssetDatabase.RemoveObjectFromAsset(node);
    nodes.Remove(node);
  }
}