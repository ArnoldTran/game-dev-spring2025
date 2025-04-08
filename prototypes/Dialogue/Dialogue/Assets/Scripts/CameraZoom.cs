using System.Collections;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera mainCamera;  // Reference to the main camera
    public Transform player;   // Reference to the player or characters
    public float zoomInSize = 5f;  // The zoom level when the camera zooms in
    public float zoomOutSize = 10f; // The zoom level when the camera zooms out
    public float zoomSpeed = 1f; // The speed at which the camera zooms in and out
    public float zoomSmoothTime = 0.3f; // Time for smoothing the transition

    private Vector3 originalPosition;
    private float originalZoom;

    void Start()
    {
        originalPosition = mainCamera.transform.position;  // Store the initial camera position
        originalZoom = mainCamera.orthographicSize;  // Store the initial zoom level (assuming orthographic camera)
    }

    // Call this method when dialogue starts
    public void ZoomInOnCharacter(Transform target)
    {
        StartCoroutine(ZoomInCoroutine(target));
    }

    // Call this method when dialogue ends
    public void ZoomOut()
    {
        StartCoroutine(ZoomOutCoroutine());
    }

    // Coroutine to zoom in on the character
    private IEnumerator ZoomInCoroutine(Transform target)
    {
        Vector3 targetPosition = target.position;  // Position of the character
        targetPosition.z = -1f;  // Set the Z position to -1 while zooming in

        float targetZoom = zoomInSize;  // Desired zoom level

        // Smooth transition to the target position and zoom level
        float elapsedTime = 0f;

        Vector3 initialPosition = mainCamera.transform.position;

        while (elapsedTime < zoomSmoothTime)
        {
            // Interpolate the position only in X and Y (keep Z constant at -1)
            mainCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / zoomSmoothTime);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, elapsedTime / zoomSmoothTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera reaches the target position and zoom level
        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetZoom;
    }

    // Coroutine to zoom out to the original position and zoom level
    private IEnumerator ZoomOutCoroutine()
    {
        Vector3 targetPosition = originalPosition;  // Original camera position
        targetPosition.z = -10f;  // Revert the Z position back to -10 after zooming

        float targetZoom = originalZoom;  // Original zoom level

        // Smooth transition back to the original position and zoom level
        float elapsedTime = 0f;

        while (elapsedTime < zoomSmoothTime)
        {
            // Interpolate the position only in X and Y (keep Z constant at -10)
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, elapsedTime / zoomSmoothTime);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, elapsedTime / zoomSmoothTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera reaches the original position and zoom level
        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetZoom;
    }
}
