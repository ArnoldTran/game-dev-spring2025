using UnityEngine;

public class BadBrick : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnDisable()
    {
        gameObject.GetComponent<Brick>().getSwarmParent().GetComponent<Cluster>().badBrickHitAlert();
    }
}
