using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;
    public float health = 100;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    [SerializeField] private LayerMask m_CameraBoundaryLayer;

    // Reference to the Lose UI prefab
    public GameObject loseUIPrefab;
    private GameObject loseUIObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check for collisions with the CameraBoundary layer
        if (Physics2D.OverlapCircle(transform.position, 0.1f, m_CameraBoundaryLayer))
        {
            // Player is colliding with the camera boundary; prevent movement
            horizontalMove = 0f;
        }
        else
        {
            // Player is not colliding with the camera boundary
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool IsCrouching)
    {
        animator.SetBool("IsCrouching", IsCrouching);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);

        // Show the Lose UI when the player dies
        if (loseUIPrefab != null)
         {
         loseUIObject = Instantiate(loseUIPrefab);
         }
    }

    public bool IsAlive()
    {
        // Check if the player's health is greater than zero
        return health > 0;
    }
}
