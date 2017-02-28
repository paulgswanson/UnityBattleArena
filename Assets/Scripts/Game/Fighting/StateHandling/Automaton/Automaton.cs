﻿using UnityEngine;

/* Automaton is an object that contains all the FSMs and other useful
 * attributes / methods necessary for it. */

public class Automaton : MonoBehaviour {
    [HideInInspector]
    public StateMachine StateMachine = null;
}
