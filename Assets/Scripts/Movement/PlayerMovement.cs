using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using alu0100969535.Utils;

public class PlayerMovement : MonoBehaviour {

    public Action<float> OnPlayerMoveX = delegate {};

    [Header("Controls")]
    [SerializeField] private string axisHorizontal;
    [SerializeField] private string axisVertical;
    [SerializeField] private KeyCode keyUp;
    [SerializeField] private KeyCode keyLeft;
    [SerializeField] private KeyCode keyDown;
    [SerializeField] private KeyCode keyRight;

    [SerializeField] private KeyCode jumpKey;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float jumpForce = 7.5f;

    [Header("Internal")]
    [SerializeField] private float velocityThreshold = 1.25f;
    [SerializeField] private bool shouldFlip = true;

    private Animator animator;
    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private SpriteRenderer spriteRenderer;

    private Dictionary<KeyCode, Action<bool>> bindedKeys = new Dictionary<KeyCode, Action<bool>>();
    private Dictionary<string, Action<float>> bindedAxis = new Dictionary<string, Action<float>>();

    private bool isJumpingButtonPressed;

    private bool isGameRunning = true;
    
    private float speedH = 0.0f;
    private float speedV = 0.0f;
    private bool isJumping = false;
    private bool isCrouching = false;
    private bool isFalling = false;
    private bool isLanded = true;

    private bool isFacingRight = true;
    private bool canJump = true;
    private List<bool> couldJump = new List<bool>{true, true, true, true, true};

    private Vector3 initialPosition;
    private Vector3 initialEulerAngles;
    private Vector3 initialLocalScale;

    void Awake() {
        animator = Utils.Get<Animator>(gameObject, "PlayerMovement needs an Animator");
        rigidbody = Utils.Get<Rigidbody2D>(gameObject, "PlayerMovement needs a Rigidbody2D");
        collider = Utils.Get<Collider2D>(gameObject, "PlayerMovement needs a Collider2D");
        spriteRenderer = Utils.Get<SpriteRenderer>(gameObject, "PlayerMovements needs a SpriteRenderer");

        BuildDictionaryOfBindings();

        initialPosition = transform.position;
        initialEulerAngles = transform.eulerAngles;
        initialLocalScale = transform.localScale;
    }

    public float MovementSpeed {
        get => movementSpeed;
        set => movementSpeed = value;
    }

    public void Alive() {
        isGameRunning = true;
        transform.position = initialPosition;
        transform.eulerAngles = initialEulerAngles;
        transform.localScale = initialLocalScale;
    }

    public void Dead() {
        isGameRunning = false;
        UpdateAnimation();
        StartCoroutine(PerformDeadAnimation(2f));
    }

    private IEnumerator PerformDeadAnimation(float duration) {
        var initialRotation = transform.rotation;

        var initialScale = transform.localScale;
        var finalScale = 0;
        
        float counter = 0;
        while (counter < duration) {
            counter += Time.deltaTime;

            if (isGameRunning) {
                yield break;
            }
            
            var rotation = transform.localEulerAngles;
            rotation.z = Mathf.Lerp(0, 720f, counter);
            transform.localEulerAngles = rotation;
            
            var scale = transform.localScale;
            scale.x = Mathf.Lerp(initialScale.x,finalScale, counter);
            scale.y = Mathf.Lerp(initialScale.y,finalScale, counter);
            transform.localScale = scale;
            yield return null;
        }
    }
    
    

    void FixedUpdate() {
        if (!isGameRunning) {
            return;
        }
        ProcessKeyBindings();
        UpdateFallingState();

        UpdateAnimation();

        MovePlayer();
    }

    void ProcessKeyBindings() {
        const float defaultSpeed = 0.0f;

        speedH = defaultSpeed;
        isJumpingButtonPressed = false;
        isCrouching = false;

        foreach (var entry in bindedAxis) {
            float value = Input.GetAxis(entry.Key);
            entry.Value(value);
        }

        foreach (var entry in bindedKeys) {
            bool isPressed = Input.GetKey(entry.Key);
            entry.Value(isPressed);
        }
    }

    private void UpdateFallingState() {
        isFalling = rigidbody.velocity.y < -velocityThreshold;
        isJumping = rigidbody.velocity.y > velocityThreshold;
        isLanded = rigidbody.velocity.y == 0;

        canJump = !isFalling && !isJumping && couldJump[0] && couldJump[1] && couldJump[2] && couldJump[3] && couldJump[4];
        couldJump[4] = couldJump[3];
        couldJump[3] = couldJump[2];
        couldJump[2] = couldJump[1];
        couldJump[1] = couldJump[0];
        couldJump[0] = !isFalling && !isJumping;
    }

    void UpdateAnimation() {
        FlipIfNeeded(rigidbody.velocity.x);
        animator.SetFloat("SpeedH", Mathf.Abs(speedH));
        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("IsFalling", isFalling);
        animator.SetBool("IsCrouching", isCrouching);
        animator.SetBool("IsLanded", isLanded);
    }

    private void FlipIfNeeded(float velocityH) {
        
        if(!shouldFlip) { 
            return;
        }

        if(Math.Abs(velocityH) - velocityThreshold / 5 < 0.0f && speedH == 0.0f) {
            return;
        }

        if(speedH == 0.0f){
            isFacingRight = velocityH < 0;
        } else {
            isFacingRight = speedH < 0;
        }
        
        UpdateFlip();
    }

    private void UpdateFlip() {
        spriteRenderer.flipX = isFacingRight;
    }

    void MovePlayer() {
        if(ShouldMove()) {
            Move();
            OnPlayerMoveX(speedH);
        } else {
            OnPlayerMoveX(0);
        }

        if(ShouldJump()){
            Jump();
        }
    }

    private bool ShouldMove() {
        return !(speedH == 0.0f || isCrouching && !isJumping);
    }

    private void Move() {
        var movement = new Vector3(speedH * movementSpeed, 0, 0);
        rigidbody.AddForce(movement, ForceMode2D.Impulse);
    }
    
    private bool ShouldJump() {
        return isJumpingButtonPressed && !isJumping && canJump;
    }

    private void Jump() {
        var vector = new Vector2(0, jumpForce);
        rigidbody.AddForce(vector, ForceMode2D.Impulse);
    }

    void BuildDictionaryOfBindings() {

        bindedKeys.Add(keyUp, (isPressed) => {
            isJumpingButtonPressed |= isPressed;
        });

        bindedKeys.Add(keyRight, (isPressed) => {
            if (isPressed) {
                speedH = 1.0f;
            }
        });

        bindedKeys.Add(keyLeft, (isPressed) => {
            if (isPressed) {
                speedH = -1.0f;
            }
        });

        bindedKeys.Add(keyDown, (isPressed) => {
            isCrouching |= isPressed;
        });
        
        bindedKeys.Add(jumpKey, (isPressed) => {
            isJumpingButtonPressed |= isPressed;
        });
    }
}
