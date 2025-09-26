/*
    Ethan Gapic-Kott, 000923124
*/

/// Script used to add depth to the background by moving the background with the player's distance

using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    public Transform layer;
    [Range(0f, 2f)]
    public float parallaxFactor = 0.5f;
}

public class Background : MonoBehaviour
{
    public ParallaxLayer[] layers;
    public Transform player;

    private Vector3 lastPlayerPos;

    void Start()
    {
        if (player != null)
            lastPlayerPos = player.position;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Calculates distance of player movement
        Vector3 playerDelta = player.position - lastPlayerPos;

        foreach (var layer in layers)
        {
            // Move layer opposite to player movement
            layer.layer.position += new Vector3(playerDelta.x * layer.parallaxFactor, 0f, 0f);
        }

        lastPlayerPos = player.position;
    }
}
