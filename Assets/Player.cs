using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    Vector2 firstPressPosition;         //1.初期ポジションの記録
    Vector2 secondPressPositoin;        //2.スライドして離したポジション記録
    Vector2 currentSwipePosition;       //1から2のスライド距離の記録
    public float detectionSensitivBottom = -0.8f;

    public float Step = 0.1f;
    public float jump = 5.0f;
    public int MaxjumpCount = 2;
    public int jumpCount = 0;
    public float slidingProgress;
    public const float slidingTime = 0.1f;

    Rigidbody2D rigit2D;
    Animator animator;
    bool isSwipe = false;
    bool sliding;

    // Start is called before the first frame update
    void Start()
    {
        rigit2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPressPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            currentSwipePosition  = Input.mousePosition;
            slidingAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            secondPressPositoin = Input.mousePosition;
            DetectSwipe();
        }
        this.transform.position += new Vector3(Step * Time.deltaTime, 0, 0);
        
    }

    void DetectSwipe()
    {
        Vector2 currentSwipePosition = secondPressPositoin - firstPressPosition;
        float swipeMagnitude = currentSwipePosition.magnitude;

        if (swipeMagnitude > 0.0f)
        {
            currentSwipePosition.Normalize();
            if (currentSwipePosition.y < detectionSensitivBottom)
            {
                Debug.Log("離した");
                isSwipe = true;
                slidingProgress += Time.deltaTime;
                if (slidingProgress < slidingTime)
                {

                }
                else
                {
                    currentSwipePosition = new Vector2(0, 0);
                    slidingProgress = 0f;
                }
            }

        }

        if (!isSwipe)
        {
            rigit2D.AddForce(transform.up * jump, ForceMode2D.Impulse);
            Debug.Log("click");
        }
        // クリック後に isSwipe をリセットする
        isSwipe = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ruby")
        {
            if(Input.GetMouseButtonDown(0))
            {
                rigit2D.AddForce(transform.up * jump, ForceMode2D.Impulse);
                jumpCount++;
             }
            if (collision.gameObject.CompareTag("Floor"))
            {
                jumpCount = 0;
            }
        }
        if(collision.gameObject.tag== ("Emerald"))
        {

        }
    }

    private void slidingAction()
    {
        Vector2 currentSwipePosition = secondPressPositoin - firstPressPosition;
        float swipeMagnitude = currentSwipePosition.magnitude;

        if (swipeMagnitude > 0.0f)
        {
            currentSwipePosition.Normalize();
            if (currentSwipePosition.y < detectionSensitivBottom)
            {
                Debug.Log("下");
                isSwipe = true;
                slidingProgress += Time.deltaTime;
                if (slidingProgress < slidingTime)
                {
                    animator.SetBool("sliding", true);
                }
                else
                {
                    currentSwipePosition = new Vector2(0, 0);
                    slidingProgress = 0f;
                    animator.SetBool("sliding", false);
                }
            }
        }
        isSwipe = false;
    }
}
