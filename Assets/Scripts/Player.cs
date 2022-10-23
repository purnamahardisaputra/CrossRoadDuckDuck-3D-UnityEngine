using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text stepText;
    [SerializeField] ParticleSystem dieParticles;
    [SerializeField, Range(0.01f, 1f)] float moveDuration = 0.2f;
    [SerializeField, Range(0.01f, 1f)] float jumpHigh = 0.5f;
    private float rightBoundary;
    private float leftBoundary;
    private float backBoundary;
    [SerializeField] AudioSource jumpSound;
    [SerializeField] AudioSource dieSound;

    [SerializeField] private int maxTravel;
    public int MaxTravel { get => maxTravel; }
    
    [SerializeField] private int currentTravel;
    public int CurrentTravel { get => currentTravel; }

    public bool IsDie { get => this.enabled == false; }


    public void SetUp(int minZPos, int extent)
    {
        backBoundary = minZPos - 1;
        leftBoundary = -(extent + 1);
        rightBoundary = extent + 1;
    }



    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.UpArrow))
        //     Debug.Log("Forward");
        // if (Input.GetKeyDown(KeyCode.DownArrow))
        //     Debug.Log("Backward");

        var MoveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
            MoveDir += new Vector3(0, 0, 1);

        if (Input.GetKey(KeyCode.DownArrow))
            MoveDir += new Vector3(0, 0, -1);

        if (Input.GetKey(KeyCode.RightArrow))
            MoveDir += new Vector3(1, 0, 0);

        if (Input.GetKey(KeyCode.LeftArrow))
            MoveDir += new Vector3(-1, 0, 0);


        // if (MoveDir == Vector3.zero)
        //     return;
        // if (IsJumping() == false)
        //     Jump(MoveDir);

        if (MoveDir != Vector3.zero && IsJumping() == false)
            Jump(MoveDir);
    }
    private void Jump(Vector3 TargetDirection)
    {
        // var TargetPosition = transform.position + new Vector3(TargetDirection.x, 0, TargetDirection.y);
        //atur rotasi
        var targetPosition = transform.position + TargetDirection;
        transform.LookAt(targetPosition);

        // loncat ke atas
        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHigh, moveDuration / 2));
        moveSeq.Append(transform.DOMoveY(0f, moveDuration / 2));

        if (targetPosition.z <= backBoundary || targetPosition.x <= leftBoundary || targetPosition.x >= rightBoundary)
            return;

        if (Tree.AllPositions.Contains(targetPosition))
            return;

        // transform.DOMoveY(0.5f, 0.1f).onComplete(() => transform.DOMoveY(0, 0.1f));
        // gerak maju / mundur / samping
        transform.DOMoveX(targetPosition.x, moveDuration);
        transform.DOMoveZ(targetPosition.z, moveDuration).OnComplete(UpdateTravel);
        jumpSound.Play();
    }

    private void UpdateTravel()
    {
        currentTravel = (int)this.transform.position.z;
        if (currentTravel > maxTravel)
            maxTravel = currentTravel;

        stepText.text = "STEP: " + maxTravel.ToString();
    }

    

    public bool IsJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled == false)
            return;
        // di execute sekali pada frame ketika nempel pertama kali
        var car = other.GetComponent<Car>();
        if (car != null)
        {
            AnimateCrash(car);
        }

        if (other.tag == "Car")
        {
            // Debug.Log("Hit " + other.name);
            // AnimateDie();
        }
    }

    private void AnimateCrash(Car car)
    {
        // var isRight = car.transform.rotation.y == 90;
        // transform.DOMoveX(isRight ? 8 : -8, 0.2f);
        // transform.DORotate(Vector3.forward * 360, 0.2f).SetLoops(100, LoopType.Restart);

        // Gepeng
        transform.DOScaleY(0.1f, 0.2f);
        transform.DOScaleX(3, 0.2f);
        transform.DOScaleZ(2, 0.2f);
        this.enabled = false;
        dieParticles.Play();
        dieSound.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        // di execute setiap frame selama masih nempel
    }

    private void OnTriggerExit(Collider other)
    {
        // diexecute sekali [ada frame ketika tidak nempel
    }

    

}
