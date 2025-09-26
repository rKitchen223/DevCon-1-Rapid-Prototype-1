/*
    Ethan Gapic-Kott, 000923124
*/

/// Script to handle all of the player movement, logic, corutines, and overrall game mechanics

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{

    // Normal and enhanced dash distance / cooldowns
    [Header("Dash Settings")]
    public float dashDistance = 3f;
    public float enhancedDistance = 9f;
    public float dashDuration = 0.25f;
    public float doubleTapTime = 0.3f;
    public float dashCooldown = 0.5f;

    // Leap height and duration
    [Header("Arc / Leap")]
    public float arcHeight = 2f;
    public float leapDurationMultiplier = 2f;

    // Links to player camera and UI the arrows
    [Header("References")]
    public CameraFollow cameraFollow;
    public DashUIManager dashUI;

    // Logic and variables for dashing and dash sequences
    private Rigidbody2D rb;
    private bool canDash = true;
    private float lastTapLeft = -1f, lastTapRight = -1f;
    private List<Vector2> dashSequence = new List<Vector2>();
    private Vector2 lastDashDir = Vector2.zero;
    private Coroutine dashCoroutine;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        spriteRenderer = GetComponent<SpriteRenderer>();
        dashUI?.UpdateDashVisual(Vector2.zero, 0);
    }

    // Checks and updates last player inputs (AA/DD)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && Time.time - lastTapLeft < doubleTapTime)
            QueueDash(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D) && Time.time - lastTapRight < doubleTapTime)
            QueueDash(Vector2.right);

        lastTapLeft = Input.GetKeyDown(KeyCode.A) ? Time.time : lastTapLeft;
        lastTapRight = Input.GetKeyDown(KeyCode.D) ? Time.time : lastTapRight;
    }

    // Initializes dash sequence and direction
    void QueueDash(Vector2 dir)
    {
        if (!canDash) return;

        if (dir != lastDashDir)
        {
            dashSequence.Add(dir);
            if (dashSequence.Count > 3) dashSequence.RemoveAt(0);
        }
        lastDashDir = dir;

        dashUI?.UpdateDashVisual(dir, dashSequence.Count);
        FlipSprite(dir);

        // Check enhanced dash pattern (AA/DD/AA or DD/AA/DD)
        if (dashSequence.Count == 3 &&
            ((dashSequence[0] == Vector2.right && dashSequence[1] == Vector2.left && dashSequence[2] == Vector2.right) ||
             (dashSequence[0] == Vector2.left && dashSequence[1] == Vector2.right && dashSequence[2] == Vector2.left)))
        {
            dashSequence.Clear();
            lastDashDir = Vector2.zero;
            dashUI?.UpdateDashVisual(dir, 3);
            StartDash(dir, enhancedDistance);
            StartCoroutine(ResetArrowAfterDelay(0.1f, dir));
            return;
        }

        StartDash(dir, dashDistance);
    }

    void StartDash(Vector2 dir, float distance)
    {
        if (dashCoroutine != null) StopCoroutine(dashCoroutine);
        dashCoroutine = StartCoroutine(DashCoroutine(dir, distance));
    }

    // Converts the dash into a "leap"
    IEnumerator DashCoroutine(Vector2 dir, float distance)
    {
        canDash = false;
        if (cameraFollow != null) cameraFollow.isPlayerDashing = true;

        float origGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        Vector2 startPos = rb.position;
        Vector2 endPos = startPos + dir.normalized * distance;
        float duration = dashDuration * leapDurationMultiplier;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            Vector2 linear = Vector2.Lerp(startPos, endPos, smoothT);
            float arc = arcHeight * (1f - Mathf.Pow(2f * smoothT - 1f, 2f));
            rb.MovePosition(linear + Vector2.up * arc);
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(endPos);
        rb.gravityScale = origGravity;
        rb.velocity = Vector2.zero;

        if (cameraFollow != null) cameraFollow.isPlayerDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        dashCoroutine = null;
    }

    // Flips the player sprite based on their direction
    void FlipSprite(Vector2 dir)
    {
        if (dir.x != 0) spriteRenderer.flipX = dir.x < 0;
    }

    // Reset the UI arrows
    IEnumerator ResetArrowAfterDelay(float delay, Vector2 dir)
    {
        yield return new WaitForSeconds(delay);
        dashUI?.UpdateDashVisual(dir, 0);
    }
}
