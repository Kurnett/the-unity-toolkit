using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Node : ScriptableObject {
  public int id;
  public bool start;
  public List<NodeOption> options = new List<NodeOption>();
  public NodeOption defaultOption;

  // Editor Data
  public Rect rect;
  public Rect containerRect;
  public Rect[] optionRects;

  public bool isDragged;
  public bool isSelected;

  public float width = 200f;
  public float height = 30f;

  // Needs initialization
  public Action<Node> OnClickNode;
  public Action<Node> OnRemoveNode;
  public Action<NodeOption> OnClickOption;
  public Action<NodeGraph> SaveGraph;

  virtual public void Construct(int id, Vector2 position) {
    this.id = id;
    rect = new Rect(position.x, position.y, width, height);
  }

  public void MoveOption(NodeOption option, int diff) {
    int index = options.IndexOf(option);
    int newIndex = Mathf.Clamp(index + diff, 0, options.Count - 1);
    if (index != newIndex) {
      options.RemoveAt(index);
      options.Insert(index + diff, option);
      GUI.changed = true;
    }
  }

  public virtual void AddOption() {
    NodeOption newOption = (NodeOption)ScriptableObject.CreateInstance(typeof(NodeOption));
    // newOption.Construct(SaveGraph);
    AssetDatabase.AddObjectToAsset(newOption, this);
    options.Add(newOption);
  }

  public virtual void RemoveOption(NodeOption option) {
    AssetDatabase.RemoveObjectFromAsset(option);
    options.Remove(option);
  }

  public virtual Boolean PointIsInBoundingBox(float x, float y) {
    return x > rect.x && x < rect.x + width && y > rect.y && y < rect.y + height;
  }
}