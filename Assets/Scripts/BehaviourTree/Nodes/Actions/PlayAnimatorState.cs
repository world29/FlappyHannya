﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateNodeMenu("BehaviourTree/Action/PlayAnimatorState")]
    public class PlayAnimatorState : Action
    {
        public string m_stateName;

        public override NodeStatus Evaluate(GameObject go)
        {
            Animator animator = go.gameObject.GetComponent<Animator>();
            if (animator == null || !animator.isActiveAndEnabled)
            {
                Debug.LogWarning("Animator is not set");
                return NodeStatus.FAILURE;
            }

            animator.Play(m_stateName);

            return NodeStatus.SUCCESS;
        }
    }
}