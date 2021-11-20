using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MIkeController : MonoBehaviour
{
    public Transform Camera;
    public Transform t1;
    public Transform t2;

    public Transform l1;
    public Transform l2;

    public Animator anim; 



    private void Start()
    {

        Sequence sequence = DOTween.Sequence();
        sequence.Append(Camera.DOLocalMove(t1.position, 3));
        sequence.AppendCallback(()=> {
            l1.gameObject.SetActive(true);
            l2.gameObject.SetActive(true);
        });
        sequence.InsertCallback(3.2f,() => {
            anim.SetInteger("d", 1);
        });
        sequence.InsertCallback(5,()=> {

            //Camera.parent.DORotate(new Vector3(0, 360, 0), 5, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
            Camera.DOShakePosition(1, 0.1f).SetLoops(-1) ;
        });




    }
}
