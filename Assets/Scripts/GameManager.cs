using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

    public enum Lvl { level1, level2}

    public static Lvl Level;
    public TouchInput input;
    public GameObject hole;

    [SerializeField] GameObject gridPref;
    [SerializeField] GameObject ground;
    [SerializeField] GameObject cam;

    private Rigidbody rigidBody;

    private bool inPlay;
    private float newY;
    public int ballcount;


    private void Start()
    {
        BorderBlock.gridPref = gridPref;
        BorderBlock.ground = ground;
        BorderBlock.GetSingleton().GridStart(0);

        DOTween.SetTweensCapacity(1250, 50);
    }

    private void Update()
    {
        switch (Level)
        {
            case Lvl.level1:
                
                break;

            case Lvl.level2:
                if (!inPlay)
                {
                    inPlay = true;
                    newY = -30f;
                    StartCoroutine(NextLevel());
                }

                if(ballcount == 2)
                {
                    
                }

                break;
        }
    }


    IEnumerator NextLevel()
    {
        BorderBlock.GetSingleton().GridReset();
        yield return new WaitForSeconds(3f);
        BorderBlock.GetSingleton().GridStart(newY);
        cam.transform.DOMoveY(6.33f + newY, 10, false);
        
    }
}
