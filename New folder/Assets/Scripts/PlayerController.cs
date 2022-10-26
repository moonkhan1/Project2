using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Dreamteck.Splines;

public class PlayerController : MonoBehaviour
{
    float Horizontal;
    SplineFollower splineFollower;
    [SerializeField] float _speed = 5f;
     Collider[] colliders;
    [SerializeField] Transform detectTransform;
    [SerializeField] float DetectionRange = 1;
    [SerializeField] LayerMask layer;
    [SerializeField] Transform holdTransform;
    [SerializeField] int aliveGoblinCount = 0;
     [SerializeField] int deadGoblinCount = 0;
     [SerializeField] Transform Drop;
    [SerializeField] float ItemDistanceBetween = 0.5f;
    [SerializeField] float DropDistanceBetween = 1f;

    float DropRate = 1f;
    float DropSecond = 1f;
    float NextDropTime;

    public Stack<Collider> Boxes;
    Rigidbody rb;
    Animator _anim;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        splineFollower = this.transform.parent.GetComponent<SplineFollower>();
        
        Boxes = new Stack<Collider>();
        _anim = GetComponent<Animator>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(detectTransform.position, DetectionRange);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        colliders = Physics.OverlapSphere(detectTransform.position, DetectionRange, layer);
        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Debug.Log(hit.name);
                hit.tag = "Collected";
                hit.transform.parent = holdTransform;

                 Boxes.Push(hit);
                var seq = DOTween.Sequence();
                seq.Append(hit.transform.DOLocalJump(new Vector3(0, aliveGoblinCount * ItemDistanceBetween), 2, 1, 0.3f))
                   .Join(hit.transform.DOScale(1.25f, 0.1f))
                   .Insert(0.1f, hit.transform.DOScale(1f, 0.2f));
                seq.AppendCallback(() =>
                {
                    hit.transform.localRotation = Quaternion.Euler(-90, -95, 0);
                });
                aliveGoblinCount++;
            }
        }
    }

    public void MovePlayer()
    {
        Horizontal = Input.GetAxis("Horizontal");
        // if(Horizontal == 0f) return;
        // transform.Translate(Vector3.right * Horizontal * Time.deltaTime * _speed);
        // float xBoundry = Mathf.Clamp(transform.position.x, -3f,3f);
        transform.localPosition += new Vector3(Horizontal , 0, 0) *Time.deltaTime*_speed;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Drop"))
        {
            
        
        Debug.Log("sadasd");
        splineFollower.follow = false;
        _anim.SetBool("isFinished", true);

        if (Time.time >= NextDropTime)
        {
            if (Boxes.Count <= 0) return;
            GameObject go = Boxes.Pop().gameObject;
            go.transform.parent = null;
            var Seq = DOTween.Sequence();
            Seq.Append(go.transform.DOJump(Drop.position + new Vector3(0, (deadGoblinCount * DropDistanceBetween), 0), 2, 1, 0.5f))
                    .Join(go.transform.DOScale(1.5f, 0.1f))
                    .Insert(0.1f, go.transform.DOScale(1, 0.2f))
                    .AppendCallback(() => { go.transform.rotation = Quaternion.Euler(-90, -95, 0); });
            other.GetComponent<Drop>().StackedDropItems.Push(go);
            deadGoblinCount++;
            aliveGoblinCount--;
            NextDropTime = Time.time + DropSecond / DropRate;
            
            
        }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(other.GetComponent<Drop>().SellDropedItems());
        deadGoblinCount = 0;
    }

    private void OnCollisionEnter(Collision other) {
        
        if (other.transform.CompareTag("Obstacles"))
        {
            _anim.SetBool("isDead", true);
            splineFollower.follow = false;
            holdTransform.gameObject.SetActive(false);
        }
    }




}
