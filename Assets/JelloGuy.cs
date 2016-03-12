using UnityEngine;
using System.Collections;
using DG.Tweening;


public class JelloGuy : MonoBehaviour {

    Rigidbody rigid;
    float scaleY;
    Vector3 localScale;
    public Transform target;
    public float jumpDuration;
    public float jumpSpeed;

	// Use this for initialization
	void Start () {
        rigid = this.gameObject.GetComponent<Rigidbody>();
        scaleY = transform.localScale.y;
        localScale = transform.localScale;
        StartCoroutine(jump());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}

    

    IEnumerator jump()
    {
        rigid.transform.DOJump(rigid.transform.position + transform.forward*2f, 2, 1, 1f, false);
        yield return new WaitForSeconds(.1f);
        rigid.transform.DOScale(new Vector3(1f, 1.4f, 1f), .2f);
        yield return new WaitForSeconds(.9f);
        StartCoroutine(jump());
    }

    void OnCollisionEnter(Collision coll) {
        //rigid.transform.DOPunchScale(localScale * 1.5f, 1f, 3, 1);
        //rigid.transform.DOShakeScale(.5f, new Vector3(0, 2f, 0), 5, 0);
        rigid.transform.DOScale(new Vector3(1.4f,1f,1.4f), .2f);
        Vector3 targetFlat = target.position;
        targetFlat.y = rigid.transform.position.y;
        rigid.transform.DOLookAt(targetFlat, .5f, AxisConstraint.None, Vector3.up);


        //rigid.transform.DOScaleY(scaleY / 1.25f, .2f);


    }
    void OnCollisionExit(Collision coll)
    {


    }

    
}
