using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Animations test
public class Cubes : MonoBehaviour
{
    [SerializeField] private Vector3 maxRange;
    [SerializeField] private float step;
    private Transform[] cubes;
    // private int current = 0;

    public int recursionDepth = 3;  // Depth of fractal recursion
    public float childScale = 0.5f; // Scale factor for child objects
    public float childDistance = 2f; // Distance of child objects from parent
    public float rotationSpeed = 20f; // Speed of rotation for child objects

    void Start()
    {
        // Debug.Log(cubes.Length);
        // GenerateFractal(transform, recursionDepth);
        // cubes = transform.Cast<Transform>().ToArray();
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // cube.sc
        ReplicateObjectOnCubeFaces(cube, 1.2f);
    }

    void Update()
    {
        // MotionEquation(cubes, transform.position, step);
        // AnimateFractal(transform);
    }

    private void MotionEquation(Transform[] bodies, Vector3 pivot, float angleDegrees)
    {
        // Convert angle from degrees to radians for trigonometric calculations
        float angleRadians = MathF.PI / 180 * angleDegrees;

        // Iterate over each object and calculate its new position
        for (int i = 0; i < bodies.Length; i++)
        {
            // Translate the object to the pivot's local space
            Vector3 translatedObject = bodies[i].position - pivot;

            // Apply rotation using a rotation matrix
            float cosTheta = MathF.Cos(angleRadians);
            float sinTheta = MathF.Sin(angleRadians);

            float xNew = translatedObject.x * cosTheta - translatedObject.y * sinTheta;
            float yNew = translatedObject.x * sinTheta + translatedObject.y * cosTheta;

            // Translate the object back to world space
            bodies[i].position = new Vector3(xNew, yNew, 0);
        }
    }

    private void GenerateFractal(Transform parent, int depth)
    {
        if (depth == 0)
            return;

        for (int i = 0; i < 4; i++)
        {
            // Create a new child GameObject
            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Cube);
            child.transform.parent = parent;

            // Set the child's scale and position relative to the parent
            child.transform.localScale = parent.localScale * childScale;
            child.transform.localPosition = new Vector3(
                Mathf.Cos(i * Mathf.PI / 2) * childDistance,
                0,
                Mathf.Sin(i * Mathf.PI / 2) * childDistance
            );

            // Recursively generate children of the child object
            GenerateFractal(child.transform, depth - 1);
        }
    }

    // Animate fractal pattern by rotating the child objects
    private void AnimateFractal(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Rotate the child objects
            child.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            // Recursively animate child objects
            AnimateFractal(child);
        }
    }

    void ReplicateObjectOnCubeFaces(GameObject objectToReplicate, float cubeSize)
    {
        // Positions for the 6 faces of a cube (up, down, left, right, forward, backward)
        Vector3[] facePositions = new Vector3[]
        {
            Vector3.up,    // Top face
            Vector3.down,  // Bottom face
            Vector3.left,  // Left face
            Vector3.right, // Right face
            Vector3.forward, // Front face
            Vector3.back   // Back face
        };

        // Loop through each face position and instantiate a copy of the object
        foreach (Vector3 position in facePositions)
        {
            // Instantiate a new copy of the object
            GameObject replicatedObject = Instantiate(objectToReplicate, transform);

            // Set the position of the new object relative to the parent object
            replicatedObject.transform.localPosition = position * cubeSize;

            // Optionally, adjust rotation so the object faces outward
            replicatedObject.transform.localRotation = Quaternion.LookRotation(position);
        }
    }
}
