using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public event Action MoveCharacterToSelectedPositionEvent;

    private InputActionMap m_PlayerControllerActionMap;
    private InputAction m_MoveCharacterToSelectedPosition;

    [SerializeField] private TacticalCharacterController m_SelectedCharacter;

    private void OnEnable()
    {
        m_PlayerControllerActionMap = InputSystem.actions.FindActionMap("PlayerController");
        m_PlayerControllerActionMap.Enable();

        m_MoveCharacterToSelectedPosition = m_PlayerControllerActionMap.FindAction("MoveCharacterToSelectedPosition");
    }

    //public void Init(ITacticalGroundStrategy groundStrategy)
    //{
    //    m_GroundStrategy = groundStrategy;
    //}

    private void Update()
    {
        if (m_MoveCharacterToSelectedPosition.IsPressed())
        {
            MoveCharacterToSelectedPositionEvent?.Invoke();
        }
    }

    public void MoveCharacterToSelectedPosition(Vector3 moveToPosition)
    {
        if (m_SelectedCharacter)
        {
            //Debug.Log("MoveCharacterToSelectedPosition(" + position + ")");
            m_SelectedCharacter.MoveCharacterToSelectedPosition(moveToPosition);
        }
    }
}
