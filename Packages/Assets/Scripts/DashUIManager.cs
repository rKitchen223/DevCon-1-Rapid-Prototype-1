/*
    Ethan Gapic-Kott, 000923124
*/

/// Script used to handle all of the UI arrow sprites displayed based on player direction

using UnityEngine;
using UnityEngine.UI;

public class DashUIManager : MonoBehaviour
{
    [Header("Arrow Image")]
    public Image arrowImage;    // UI Image to display arrow direction

    [Header("Arrow Sprites")]
    public Sprite leftArrow1;   // First dash
    public Sprite leftArrow2;   // Second dash toward enhanced
    public Sprite leftArrow3;   // Enhanced dash
    public Sprite rightArrow1;
    public Sprite rightArrow2;
    public Sprite rightArrow3;

    [Header("Enhanced Dash Particle")]
    public ParticleSystem arrowParticleEffect;

    /// Updates the arrow sprite based on dash direction and sequence step.
    /// Step o: hide arrow (on game start)
    /// Step 1: first dash
    /// Step 2: second dash
    /// Step 3: enhanced dash

    public void UpdateDashVisual(Vector2 dir, int sequenceStep)
    {
        // Hide arrow if step 0
        if (sequenceStep == 0)
        {
            if (arrowImage != null)
                arrowImage.gameObject.SetActive(false);
            return;
        }

        // Show UI arrow
        if (arrowImage != null)
            arrowImage.gameObject.SetActive(true);

        // Display arrow sprite based on player direction
        if (dir == Vector2.left)
        {
            switch (sequenceStep)
            {
                case 1: arrowImage.sprite = leftArrow1; break;
                case 2: arrowImage.sprite = leftArrow2; break;
                case 3:
                    arrowImage.sprite = leftArrow3;
                    if (arrowParticleEffect != null)
                    {
                        arrowParticleEffect.transform.position = arrowImage.transform.position;
                        arrowParticleEffect.Play();
                    }
                    break;
            }
        }
        else if (dir == Vector2.right)
        {
            switch (sequenceStep)
            {
                case 1: arrowImage.sprite = rightArrow1; break;
                case 2: arrowImage.sprite = rightArrow2; break;
                case 3:
                    arrowImage.sprite = rightArrow3;
                    if (arrowParticleEffect != null)
                    {
                        arrowParticleEffect.transform.position = arrowImage.transform.position;
                        arrowParticleEffect.Play();
                    }
                    break;
            }
        }
    }
}
