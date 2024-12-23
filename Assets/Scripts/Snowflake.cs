using UnityEngine;

public class Snowflake : MonoBehaviour
{
    public float fallSpeed = 2f; // Speed at which the snowflake falls.
    private Vector3 targetPosition; // Target position (Ground level).
    public bool isMelting = false; // To ensure animation only triggers once.

    public Animator animator;

    void Start()
    {
        // Set the initial target position to below the map (Ground position).
        targetPosition = new Vector3(transform.position.x, -6.3f, transform.position.z); // Adjust -5f to your Ground's Y position.
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isMelting)
        {
            // Move the snowflake downward using Lerp.
            transform.position = Vector3.Lerp(transform.position, targetPosition, fallSpeed * Time.deltaTime);

            // Optional: Destroy snowflake if it goes too far past the ground to avoid clutter.
            if (transform.position.y < -10f)
                Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") && gameObject.CompareTag("Snowflake") && !isMelting)
        {
            Debug.Log("Swowflake entered area");
            isMelting = true;

            // Trigger the melting animation.
            if (animator != null)
            {
                animator.SetTrigger("Melt");

                // Use an animation event or coroutine to destroy the snowflake after the animation completes.
                StartCoroutine(DestroyAfterAnimation());
            }
        }
    }

    private System.Collections.IEnumerator DestroyAfterAnimation()
    {
        // Get the length of the current animation.
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Wait for the animation duration.
        yield return new WaitForSeconds(stateInfo.length + 1f);

        // Destroy the snowflake.
        Destroy(gameObject);
    }
}
