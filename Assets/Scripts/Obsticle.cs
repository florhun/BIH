using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obsticle : MonoBehaviour
{

    public enum ObsticleType { Coin, Shrink, Spike, Stopper};

    public ObsticleType obs;

    public void Update()
    { 
        if(obs == ObsticleType.Coin)
        {
            transform.DORotate(new Vector3(180, 60, 180), 10f, RotateMode.WorldAxisAdd);
        }
        else if(obs == ObsticleType.Shrink)
        {
            transform.localScale = Vector3.Lerp(Vector3.one / 4, Vector3.one / 2, Mathf.PingPong(Time.time, 1));
        }

    }

}
