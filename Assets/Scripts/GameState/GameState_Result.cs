﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState_Result : IGameState
{
    class GameEventListener : IGameEventListener
    {
        public bool Raised { get; private set; } = false;

        public void OnEventRaised()
        {
            Raised = true;
        }
    }

    GameEventListener m_surrenderListener;

    public void OnEnter(GameStateMachineContext ctx)
    {
        m_surrenderListener = new GameEventListener();

        ctx.m_PressSurrenderEvent.RegisterListener(m_surrenderListener);
    }

    public void OnExit(GameStateMachineContext ctx)
    {
        ctx.m_PressSurrenderEvent.UnregisterListener(m_surrenderListener);

        m_surrenderListener = null;
    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        if (m_surrenderListener.Raised)
        {
            //todo: 本来はステートマシンの外部でシーン遷移を呼び出す
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            return new GameState_Title();
        }

        return this;
    }
}
