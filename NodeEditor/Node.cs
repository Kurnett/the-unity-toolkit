using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Node<T> : ScriptableObject where T : NodeOption {
  public int id;
  public bool start;
  public List<T> options = new List<T>();
  public T defaultOption;

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
  }

  public void MoveOption(T option, int diff) {
    int index = options.IndexOf(option);
    int newIndex = Mathf.Clamp(index + diff, 0, options.Count - 1);
    if (index != newIndex) {
      options.RemoveAt(index);
      options.Insert(index + diff, option);
      GUI.changed = true;
    }
  }

  public virtual void AddOption() {
    T newOption = (T)ScriptableObject.CreateInstance(typeof(T));
    // newOption.Construct(SaveGraph);
    AssetDatabase.AddObjectToAsset(newOption, this);
    options.Add(newOption);
  }

  public virtual void RemoveOption(T option) {
    AssetDatabase.RemoveObjectFromAsset(option);
    options.Remove(option);
  }

  public virtual Boolean PointIsInBoundingBox(float x, float y) {
    return x > rect.x && x < rect.x + width && y > rect.y && y < rect.y + height;
  }
}