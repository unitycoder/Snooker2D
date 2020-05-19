using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Snooker2D : MonoBehaviour
{
    public Transform stick;
    public Transform ball;
    public Transform selectionFx;
    public Slider slider;


    bool follow = true;
    float ballRadius = 0;
    LayerMask layerMaskBalls = 1 << 9;
    LayerMask layerMaskBallsAndWalls = 1 << 9 | 1 << 10;
    LayerMask layerMaskWalls = 1 << 10;

    RaycastHit2D hit;
    float dist = 0;
    float minDist = 0;
    float maxDist = -3;
    float forceMultiplier = 2.5f;

    void Start()
    {
        ballRadius = ball.GetComponent<CircleCollider2D>().radius;
        minDist = -(ballRadius + ballRadius / 2);
        dist = Mathf.Clamp(maxDist / 2, maxDist, minDist);

        slider.maxValue = -maxDist;
        slider.minValue = -minDist;
        slider.value = dist + -maxDist - minDist;

        selectionFx.GetComponent<Fader>().StartFade();
    }

    void Update()
    {
        if (Input.GetKeyDown("r")) Application.LoadLevel(Application.loadedLevel);

        // MOUSE : Power Adjust
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            dist += Input.GetAxis("Mouse ScrollWheel");
            //if (dist>minDist) dist=minDist;
            dist = Mathf.Clamp(dist, maxDist, minDist);
            //Debug.Log("dist:"+dist);
            slider.value = -maxDist - minDist - (dist + -maxDist - minDist);
            //slider.image.color = Color.Lerp(Color.green,Color.red,slider.normalizedValue);
            //Debug.Log(slider.normalizedValue);
        }

        // MOUSE: Get mouse position
        Vector3 mPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

        // rotate stick around ball
        if (follow)
        {
            // TODO: add force distance here?
            Vector3 temp = (stick.position - ball.position).normalized;
            stick.rotation = Quaternion.LookRotation(Vector3.forward, mPos - ball.position);
            stick.position = ball.position;
            stick.localPosition += stick.up * dist;

            // raycast from ball to forward
            hit = Physics2D.CircleCast(ball.position, ballRadius, stick.up, 99, layerMaskBallsAndWalls);

            // we hit other balls
            if (hit.collider != null)
            {
                // from stick/ball to target
                GLDebug.DrawLine(stick.position, hit.centroid, Color.white, 0, false);

                // circle preview gizmo
                GLDebug.DrawCircle(hit.centroid, ballRadius, Color.yellow, 0, false);

                // get reflection direction for whiteball
                Vector3 reflectDir = Vector3.Reflect((new Vector3(hit.centroid.x, hit.centroid.y, 0) - stick.position).normalized, hit.normal);

                GLDebug.DrawRay(hit.centroid, reflectDir, Color.yellow, 0, false);

                // target ball estimated direction
                if (hit.collider.CompareTag("Ball"))
                {
                    Vector3 targetDir = (hit.transform.position - new Vector3(hit.centroid.x, hit.centroid.y, 0)).normalized * 10;
                    GLDebug.DrawRay(hit.centroid, targetDir, Color.red, 0, false);
                }

            }
            else
            { // no hit on balls
              // then raycast to walls instead
                hit = Physics2D.CircleCast(stick.position, ballRadius, stick.up, 99, layerMaskWalls);
                if (hit.collider != null)
                {
                    GLDebug.DrawLine(stick.position, hit.centroid, Color.red, 0, false);
                    Vector3 reflectDir2 = Vector3.Reflect((new Vector3(hit.centroid.x, hit.centroid.y, 0) - stick.position).normalized, hit.normal);
                    GLDebug.DrawRay(hit.centroid, reflectDir2, Color.red, 0, false);
                }
                else
                {

                }
            }
        }

        // MOUSE: Shoot
        if (follow && Input.GetMouseButtonUp(0)) // && hit.collider != null)
        {
            stick.GetComponent<AudioSource>().volume = Mathf.Lerp(0, 1, ReMap(dist, maxDist, minDist, 1, 0));
            stick.GetComponent<AudioSource>().Play();
            Vector3 forceDir = (ball.position - stick.position).normalized * -(dist - minDist - 0.02f) * forceMultiplier;
            ball.GetComponent<Rigidbody2D>().AddForce(forceDir, ForceMode2D.Impulse);
            follow = false;
            Invoke("HideShowStick", 0.2f);
        }

        // start following
        if (!follow)
        {
            if (ball.GetComponent<Rigidbody2D>().velocity.sqrMagnitude < 0.015f)
            {
                follow = true;
                selectionFx.GetComponent<Fader>().StartFade();
                Invoke("HideShowStick", 0.2f);
            }
        }

    } // update()

    void HideShowStick()
    {
        stick.GetComponent<Renderer>().enabled = !stick.GetComponent<Renderer>().enabled;
    }

    /// Remaps value from one range to another range
    float ReMap(float val, float from1, float to1, float from2, float to2)
    {
        return from2 + (val - from1) * (to2 - from2) / (to1 - from1);
    }

}
