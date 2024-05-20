//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class TileAnimationController : MonoBehaviour
//{
//    public Tilemap tilemap;
//    public Vector3Int tileLocation;
//    public AnimatedTile flowAnimation;
//    public AnimatedTile idleAnimation;

//    private float activationDuration;

//    void Start()
//    {
//        // Calculate the total duration of the activation animation
//        activationDuration = flowAnimation.animationSpeed * flowAnimation.m_AnimatedSprites.Length;
//        // Set the activation tile initially
//        tilemap.SetTile(tileLocation, flowAnimation);

//        // Start the coroutine to switch to the idle tile after the activation animation
//        StartCoroutine(SwitchToIdleAnimation(activationDuration));
//    }

//    public IEnumerator SwitchToIdleAnimation(float delay)
//    {
//        // Wait for the duration of the activation animation
//        yield return new WaitForSeconds(delay);
//        // Switch to the idle tile that loops indefinitely
//        tilemap.SetTile(tileLocation, idleAnimation);
//    }
//}