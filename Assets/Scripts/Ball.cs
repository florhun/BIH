using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    private TouchInput input;
    private GameObject hole;
    public GameManager GM;
    public Rigidbody rigidBody;
    public float speed;

    //Assigns needed components at creation.
    private void Start()
    {
        if(input == null)
        {
            input = GM.input;
            hole = GM.hole;
        }
    }

    //Gets input
    private void FixedUpdate()
    {
        scale = transform.localScale.x;
        if (input.SwipeUp && !isMoving)
        {
            GetDirectionBall(Vector3.forward);
            GetDirectionHole(Vector3.forward);
        }
        else if (input.SwipeDown && !isMoving)
        {
            GetDirectionBall(-Vector3.forward);
            GetDirectionHole(-Vector3.forward);
        }
        else if (input.SwipeRight && !isMoving)
        {
            GetDirectionBall(Vector3.right);
            GetDirectionHole(Vector3.right);
        }
        else if (input.SwipeLeft && !isMoving)
        {
            GetDirectionBall(-Vector3.right);
            GetDirectionHole(-Vector3.right);
        }
    }

    public float moveTime;
    private bool isMoving;
    private float scale;
    public AnimationCurve curve;
    [SerializeField] LayerMask ballMask;
    [SerializeField] LayerMask holeMask;


    //Raycasts to the given direction and moves the object.
    public void GetDirectionBall(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, ballMask))  
        {
           // if (hit.transform.CompareTag("Grid"))
            //{
                StartCoroutine(MoveBallNew(Vector3.Distance(hit.transform.position, transform.position) - (scale + .1f) , direction));
            //}
        }

    }
    public void GetDirectionHole(Vector3 direction)
    {
        RaycastHit hitHole;

        if (Physics.Raycast(hole.transform.position, direction, out hitHole, Mathf.Infinity, holeMask))
        {
            if (hitHole.transform.CompareTag("Grid"))
            {
                StartCoroutine(MoveHoleNew(Vector3.Distance(hitHole.transform.position, hole.transform.position) - .6f, direction));
            }
        }
    }

    private int level;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            other.gameObject.transform.DOKill();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Grid"))
        {
            //rigidBody.isKinematic = true;
        }

        if (other.CompareTag("Hole"))
        {
            rigidBody.isKinematic = false;
            print(level);
            if(level == 0)
            {
                level = 1;
                GameManager.Level = GameManager.Lvl.level2;
            }
            if(level == 1)
            {
                GM.ballcount += 1;
            }
            
        }

        if (other.CompareTag("Obsticle"))
        {
            if(other.GetComponent<Obsticle>().obs == Obsticle.ObsticleType.Spike)
            {
                transform.DOShakePosition(0.2f,.5f,2,10f).OnComplete(() =>
                {
                    transform.DOMove(new Vector3(3.5f, -30 + .25f, 7), 2f, false);
                });

            }
            else if(other.GetComponent<Obsticle>().obs == Obsticle.ObsticleType.Stopper)
            {
                Destroy(other.gameObject);
            }
        }

        if (other.CompareTag("Shrink"))
        {
            StartCoroutine(Shrink());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hole"))
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator MoveBallNew(float dist, Vector3 dir)
    {
        isMoving = true;
        float elapsedTime = 0;

        Vector3 origPos = transform.position;
        Vector3 targetPos = origPos + (dir * dist);

        transform.DORotate(dir * dist, 1f, RotateMode.FastBeyond360);
        while(elapsedTime <= 1)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, curve.Evaluate(elapsedTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isMoving = false;

    }
    IEnumerator MoveHoleNew(float dist, Vector3 dir)
    {
        isMoving = true;
        float elapsedTime = 0;

        Vector3 origPosHole = hole.transform.position;
        Vector3 targetPosHole = origPosHole + (dir * dist);
        while(elapsedTime <= 1)
        {
            hole.transform.position = Vector3.Lerp(origPosHole, targetPosHole, curve.Evaluate(elapsedTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isMoving = false;

    }

    IEnumerator Shrink()
    {
        transform.DOScale(Vector3.one * scale /2, 1f);
        yield return new WaitForSeconds(7f);
        transform.DOScale(Vector3.one * scale * 2, 1f);

    }
    IEnumerator Fall()
    {
        yield return new WaitForSeconds(3f);
        transform.DOMove(new Vector3(3.5f, -30 + .25f, 7), 2f, false);
        hole.transform.DOMove(new Vector3(.6f, -30 + .01f, .6f), 5f, false);
        yield return new WaitForSeconds(2f);
        rigidBody.isKinematic = true;
    }
}
