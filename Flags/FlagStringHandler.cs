using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class FlagStringHandler : ScriptableObject {

  public Flag flag;
  abstract public string HandleFlag(Flag flag);

}
