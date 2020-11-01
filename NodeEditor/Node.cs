using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Node<NODE_OPTION> : ScriptableObject where NODE_OPTION : NodeOption {
  public int id;
  public bool start;
  public List<NODE_OPTION> options = new List<NODE_OPTION>();
  public NODE_OPTION defaultOption;

  // Editor Data
  public Rect rect;
  public Rect containerRect;
  public Rect[] optionRects;

  public bool isDragged;
  public bool isSelected;

  public float width = 200f;
  public float height = 30f;

  virtual public void Construct(int id, Vector2 position) {
    this.id = id;
    rect = new Rect(position.x, position.y, width, height);
    if (defaultOption == null) {
      defaultOption = (NODE_OPTION)ScriptableObject.CreateInstance(typeof(NODE_OPTION));
    }
  }

  public void MoveOption(NODE_OPTION option, int diff) {
    int index = options.IndexOf(option);
    int newIndex = Mathf.Clamp(index + diff, 0, options.Count - 1);
    if (index != newIndex) {
      options.RemoveAt(index);
      options.Insert(index + diff, option);
      GUI.changed = true;
    }
  }

  public virtual void AddOption() {
    NODE_OPTION newOption = (NODE_OPTION)ScriptableObject.CreateInstance(typeof(NODE_OPTION));
    AssetDatabase.AddObjectToAsset(newOption, this);
    options.Add(newOption);
  }

  public virtual void RemoveOption(NODE_OPTION option) {
    AssetDatabase.RemoveObjectFromAsset(option);
    options.Remove(option);
  }

  public virtual Boolean PointIsInBoundingBox(float x, float y) {
    return x > rect.x && x < rect.x + width && y > rect.y && y < rect.y + height;
  }
}