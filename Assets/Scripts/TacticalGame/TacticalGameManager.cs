using SazenGames.Skeleton;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TacticalGameManager : MonoBehaviour
{
    [SerializeField] private TacticalCameraController m_CameraController;
    [SerializeField] private BG3TacticalGroundController m_GroundController; //TODO : pattern Service Locator 

    private InputActionMap m_TacticalGameMovementMap;
    private InputAction m_InputActionCameraUp;
    
    private PlayerController m_PlayerController;
    private ITacticalGroundStrategy m_GroundStrategy; //TODO : pattern Service Locator 

    void OnEnable()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        m_PlayerController = null;
        GC.Collect();
    }

    private void Initialize()
    {
        m_TacticalGameMovementMap = InputSystem.actions.FindActionMap("TacticalGame");
        m_TacticalGameMovementMap.Enable();

        m_PlayerController = new PlayerController();
        m_GroundStrategy = m_GroundController; //TODO : pattern Service Locator 
    }

    private void Update()
    {
        //if We Are Currently Selecting A Character and Moving A Cursor to Move Character there
        m_GroundStrategy.IndicateCharacterGroundLocation(m_CameraController.GetPointerScreenToRay(), ITacticalGroundStrategy.IndicationType.MovementPreview);
    }
}
