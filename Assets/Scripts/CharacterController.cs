using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float movementSpeed; //determines the character speed
    public static bool isFacingRight = true; //checks where character is facing

    public Rigidbody2D myRigidBody;
    public Collider2D myCollider;
    public Animator myAnimator;
    public SpriteRenderer mySpriteRenderer;

    // Animation state names
    public string idleAnimation;
    public string sidewalkAnimation;


    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        // Get the selected character's animation state names
        Character selectedCharacter = FindObjectOfType<CharacterSelection>().selectedCharacter;
        if (selectedCharacter != null)
        {
            idleAnimation = selectedCharacter.idleAnimation;
            sidewalkAnimation = selectedCharacter.sidewalkAnimation;

            // Set the character's AnimatorController
            myAnimator.runtimeAnimatorController = selectedCharacter.animatorController;   
        }
        else
        {
            Debug.LogWarning("No character selected. Default animations will be used.");
        }
    }

    private void Update()
    {
        Vector2 movement = Vector2.zero;

        //Disable upward and downward movement
        /*
        if (Input.GetKey(KeyCode.W)) // Move up
        {
            movement.y = movementSpeed;
        }
        else if (Input.GetKey(KeyCode.S)) // Move down
        {
            movement.y = -movementSpeed;
        }*/

        if (Input.GetKey(KeyCode.D)) // Move right
        {
            movement.x = movementSpeed;
            mySpriteRenderer.flipX = false;
            isFacingRight = false;
            myAnimator.Play(sidewalkAnimation);
        }
        else if (Input.GetKey(KeyCode.A)) // Move left
        {
            movement.x = -movementSpeed;
            mySpriteRenderer.flipX = true;
            isFacingRight = true;
            myAnimator.Play(sidewalkAnimation);
        }

        myRigidBody.velocity = movement != Vector2.zero ? movement : Vector2.zero;

        if (movement == Vector2.zero)
        {
            if (myAnimator.HasState(0, Animator.StringToHash(idleAnimation)))
            {
                myAnimator.Play(idleAnimation);
            }
            else
            {
                Debug.LogWarning($"Animation state '{idleAnimation}' not found in Animator.");
            }
        }
    }

    public void UpdateCharacterAnimations(Character selectedCharacter)
    {
        if (selectedCharacter != null)
        {
            idleAnimation = selectedCharacter.idleAnimation;
            sidewalkAnimation = selectedCharacter.sidewalkAnimation;

            // Update the AnimatorController
            if (myAnimator != null)
            {
                myAnimator.runtimeAnimatorController = selectedCharacter.animatorController;
            }

            Debug.Log($"Updated animations: Idle = {idleAnimation}, Sidewalk = {sidewalkAnimation}");
        }
        else
        {
            Debug.LogWarning("No selected character to update animations.");
        }
    }

}
