using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BirbSpawner : MonoBehaviour
{
    [Tooltip("When a cloud should spawn, a random prefab from this list is chosen.")]
    public GameObject birbPrefab;

    [Tooltip("The distance between clouds at Y = 0. The game should be easiest at this point.")]
    public BirbSpawnParameters spawnParametersAtStartOfGame;
    [Tooltip("The distance between clouds at the endgame height. The game should be hardest at this point.")]
    public BirbSpawnParameters spawnParametersAtEndgame;

    [Tooltip("The Y level that the clouds need to be at in order for clouds to spawn with the easiest settings.")]
    public float startOfGameHeight = 0;
    [Tooltip("The Y level that the clouds need to be at in order for clouds to spawn with the hardest settings.")]
    public float endgameHeight = 50;

    [Tooltip("The width range that clouds will spawn in.")]
    public float spawnZoneWidth = 7;

    [Tooltip("The minimum distance that the next cloud should be at, relative to the camera.")]
    public float distanceToSpawnAhead = 10;

    // This is the y-level of the previously spawned cloud. We use it to calculate when to spawn a cloud, and where.
    private float heightOfLastSpawnedBirb;

    // A reference to the camera. We need this to determine when to spawn a cloud.
    private Camera camera;

    public void Start()
    {
        heightOfLastSpawnedBirb = startOfGameHeight;
        SpawnNextBirb(true);
        // Camera.main is secretly a property that is really slow, so we want to use it as little as possible.
        // Thus, I'm caching it so that I can get away with only calling it once and just using it later.
        camera = Camera.main;
    }
        
    public void Update()
    {
        // The maximum number of clouds we can spawn per frame.
        int sentinel = 20;
        // While we are below the spawning threshold, spawn clouds!
        while (camera.transform.position.y > heightOfLastSpawnedBirb - distanceToSpawnAhead)
        {
            SpawnNextBirb(false);

            // This sentinel value allows us to escape from potential infinite loops.
            // That definitely didn't happen to me while developing this.
            // Nope. Not me.
            sentinel--;
            if (sentinel <= 0)
            {
                break;
            }
        }
    }

    private void SpawnNextBirb(bool isFirstBirb)
    {
        Vector3 randomPosition = GetNextBirbPosition();
            
        if (isFirstBirb)
        {
            randomPosition.y = startOfGameHeight;
        }
            
        heightOfLastSpawnedBirb = randomPosition.y;

        Instantiate(birbPrefab, randomPosition, birbPrefab.transform.rotation);
    }

    private Vector3 GetNextBirbPosition()
    {
        float difficultyPercent = (heightOfLastSpawnedBirb - startOfGameHeight) / endgameHeight;
        // Suddenly I realize that I'm making my variable names REALLY long. But, since this is for a class and I
        // really want you guys to be able to understand the code, I'm keeping them long. Normally you would avoid this.
        float minDistanceToNextBirb = Mathf.Lerp(
            spawnParametersAtStartOfGame.minDistanceToNextBirb,
            spawnParametersAtEndgame.maxDistanceToNextBirb,
            difficultyPercent);
        float maxDistanceToNextBirb = Mathf.Lerp(
            spawnParametersAtStartOfGame.maxDistanceToNextBirb,
            spawnParametersAtEndgame.maxDistanceToNextBirb,
            difficultyPercent);
            
        float distanceToNextBirb = Random.Range(minDistanceToNextBirb, maxDistanceToNextBirb);
        float xPos = Random.Range(0, spawnZoneWidth) - spawnZoneWidth / 2;
        float yPos = heightOfLastSpawnedBirb + distanceToNextBirb;

        return new Vector3(xPos, yPos, 0);
    }

    // This is a special function that lets you draw debug lines on the screen. It's really handy!
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            new Vector3(0, (startOfGameHeight + endgameHeight) / 2), // Center of the drawn cube
            new Vector3(spawnZoneWidth, endgameHeight - startOfGameHeight)); // Size of the drawn cube
    }

    // I wrote this as a separate class because I thought it would make it easy to use the inspector for this component.
    [Serializable]
    public class BirbSpawnParameters
    {
        [Tooltip("The lowest possible randomized distance from one cloud to the next cloud.")]
        public float minDistanceToNextBirb = 1;
        [Tooltip("The highest possible randomized distance from one cloud to the next cloud.")]
        public float maxDistanceToNextBirb = 3;
    }
}