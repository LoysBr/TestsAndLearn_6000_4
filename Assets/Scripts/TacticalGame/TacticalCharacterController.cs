using UnityEngine;

public class TacticalCharacterController : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float m_WalkSpeed = 1f;

    private Vector3 m_MoveToPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StopMoving();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.SqrMagnitude(m_MoveToPosition - transform.position) < 0.01f)
        {
            StopMoving();
        }
    }

    public void MoveCharacterToSelectedPosition(Vector3 moveToPosition)
    {
        m_MoveToPosition = moveToPosition;

        Vector3 lookAtPos = new Vector3(moveToPosition.x, transform.position.y, moveToPosition.z);
        transform.LookAt(lookAtPos);

        
        m_Animator.SetBool("TriggerIdle", false);
        m_Animator.SetBool("TriggerWalk", true);
        //m_Animator.speed = m_WalkSpeed;
    }

    public void StopMoving()
    {
        Debug.Log("stop moving");
        //m_Animator.speed = 0f;

        m_Animator.SetBool("TriggerWalk", false);
        m_Animator.SetBool("TriggerIdle", true);
    }
}
