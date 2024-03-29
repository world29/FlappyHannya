﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

public class EnemyHyottocoController : MonoBehaviour
{
    public Camera m_targetCamera;
    public UnityEvent m_onConstraintActivated;

    ParentConstraint m_constraint;

    void Start()
    {
        m_constraint = GetComponent<ParentConstraint>();

        m_constraint.constraintActive = false;
    }

    private void Update()
    {
    }

    private void LateUpdate()
    {
        // カメラx軸とこのオブジェクトのx座標が一致したら、ペアレントコンストレイントをONにする
        if (m_targetCamera.transform.position.x >= transform.position.x)
        {
            // すでにアクティベート済みなら何もしない
            if (m_constraint.constraintActive)
            {
                return;
            }

            m_constraint.constraintActive = true;

            m_onConstraintActivated.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        var camera = m_targetCamera ? m_targetCamera : Camera.main;

        float aspectRatio = (float)Screen.currentResolution.width / Screen.currentResolution.height;
        var verticalSize = camera.orthographicSize * 2;

        Gizmos.color = new Color(1, 1, 1, 0.2f);
        Gizmos.DrawCube(transform.position, new Vector3(verticalSize / aspectRatio, verticalSize, 1));
    }
}
