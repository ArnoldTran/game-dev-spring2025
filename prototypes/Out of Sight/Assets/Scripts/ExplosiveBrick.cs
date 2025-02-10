using UnityEngine;

public class ExplosiveBrick : MonoBehaviour
{
    private void OnDisable()
    {
        Debug.Log("PING");
        GameObject swarmParent = gameObject.GetComponent<Brick>().getSwarmParent();
        int[] pos = gameObject.GetComponent<Brick>().getPositionInSwarm();
        swarmParent.GetComponent<Cluster>().explodeBrick(pos[0], pos[1]);
    }
}
