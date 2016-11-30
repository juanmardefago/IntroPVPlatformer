using UnityEngine;
using System.Collections;

public class WaterTideScript : MonoBehaviour
{

    private Rigidbody2D rBody;
    public Transform[] points;
    private int currentPoint;
    public float speed;
    private bool canMove;
    public float platformStopDelay;

    // Use this for initialization
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        canMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rBody.position != (Vector2)points[currentPoint].position && canMove)
        {
            rBody.MovePosition(Vector2.MoveTowards(rBody.position, points[currentPoint].position, speed * Time.deltaTime));
        }
        else
        {
            canMove = false;
            if (rBody.position == (Vector2)points[currentPoint].position) StartCoroutine(ChangeCurrentPoint());
        }
    }

    private IEnumerator ChangeCurrentPoint()
    {
        currentPoint = (currentPoint + 1) % points.Length;
        yield return new WaitForSeconds(platformStopDelay);
        canMove = true;
    }

}
