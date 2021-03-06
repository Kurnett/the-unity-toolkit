using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueRenderer : NodeGraphRenderer<
  Dialogue,
  DialogueNode,
  DialogueOption,
  DialogueNodeRenderer,
  Flag
> { }