using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
  [SerializeField]
  AudioClip defaultTrack;
  AudioSource src;

  void Start () {
    src = gameObject.GetComponent<AudioSource>();
    if (src != null && defaultTrack != null) {
      src.clip = defaultTrack;
      src.Play();
    }
  }
}
