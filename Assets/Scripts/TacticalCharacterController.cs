using UnityEngine;

public class TacticalCharacterController : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float m_WalkSpeed = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Animator.speed = m_WalkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //void OnAnimatorMove()
    //{

    //}
}
