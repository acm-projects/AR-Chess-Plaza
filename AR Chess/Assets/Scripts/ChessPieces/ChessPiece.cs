using System.Threading.Tasks;
using UnityEngine;

public enum ChessPieceType {
    Pawn = 0,
    Rook = 1,
    Knight = 2, 
    Bishop = 3,
    Queen = 4,
    King = 5
    // in the tutorial, he starts with none = 0, pawn = 1. 
    // so be aware of the other parts where he minus 1
}
public class ChessPiece : MonoBehaviour
{

    Animator animator;
    // int attackHash = Animator.StringToHash("Attack");
    // int dieHash = Animator.StringToHash("Die");


    public int team; // 0 for white,   1 for black
    public int currentX;
    public int currentY;
    public ChessPieceType type;
    public bool hasFinishedAnimation = true;

    private Vector3 desiredPostion; // will be used for setting the target position for smooth movement
    private Vector3 desiredScale = Vector3.one; // used to scale down and move aside once it is removed.

    private Vector3[] wayPoints;
    private Vector3 startMarker, endMarker;
    private int currentStartPointIndex;
    private float startTime;
    private float journeyLenth; 
    private bool needMovement = false;

    // CONSTANTS
    private float HEIGHT_MULTIPLE = 3.0f;
    private float MOVING_SPEED = 10f;


    private void SetPoints() {
        startMarker = wayPoints[currentStartPointIndex];
        endMarker = wayPoints[currentStartPointIndex + 1];
        // Debug.Log("Index: " + currentStartPointIndex.ToString() + "\n");
        // Debug.Log(wayPoints[2]);
        // Debug.Log(needMovement);
        // startTime = Time.time;
        journeyLenth = Vector3.Distance(startMarker, endMarker);
    }

    private void Update() {
        // transform.position = Vector3.Lerp(transform.position, desiredPostion, Time.deltaTime * 10);
        if (needMovement) {
            float distCovered = Vector3.Distance(startMarker, transform.position);
            float completionRate = distCovered / journeyLenth;
            float step =  1.0f * Time.deltaTime;
            // Debug.Log("Completion rate:" + completionRate.ToString() + "\n");

            // transform.position = Vector3.Lerp(startMarker, endMarker, completionRate);
            transform.position = Vector3.MoveTowards(transform.position, endMarker, MOVING_SPEED * Time.deltaTime);
            // Debug.Log("Current position: " + transform.position.ToString());
            
            float distRemain = Vector3.Distance(transform.position, endMarker);
            // Debug.Log("Distance remaining: " + distRemain.ToString());
            if (distRemain < 0.001 && currentStartPointIndex + 2 < wayPoints.Length) {
                currentStartPointIndex++;
                SetPoints();
            } else if (distRemain < 0.001 && currentStartPointIndex + 2 == wayPoints.Length) {

                needMovement = false;
                hasFinishedAnimation = true;
            }
        }

        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * 10);
    }

    // private void MovePieceAnimation() {
    //     // Jump to the sky at the start position
    //     Vector3 startPositionInTheSky = new Vector3(transform.position.x, transform.position.y * HEIGHT_MULTIPLE, transform.position.z);
    //     transform.position = Vector3.Lerp(transform.position, startPositionInTheSky,Time.deltaTime * MOVING_TIME);

    //     // Move to the sky at the desired position
    //     Vector3 desiredPositionInTheSky = new Vector3(desiredPostion.x, desiredPostion.y * HEIGHT_MULTIPLE, desiredPostion.z);
    //     transform.position = Vector3.Lerp(transform.position, desiredPositionInTheSky, Time.deltaTime * MOVING_TIME);

    //     // Land down at the desired position
    //     // Vector3 desiredPositionOnTheGround = new Vector3(desiredPostion.x, desiredPostion.y, desiredPostion.z);
    //     // transform.position = Vector3.Lerp(transform.position, desiredPositionOnTheGround, Time.deltaTime * MOVING_TIME);
    // }

    private void createWaypoints(Vector3 currentPosition, Vector3 finalPosition) {
        Vector3 startPositionInTheSky = new Vector3(currentPosition.x, currentPosition.y * HEIGHT_MULTIPLE, currentPosition.z);
        Vector3 desiredPositionInTheSky = new Vector3(finalPosition.x, finalPosition.y * HEIGHT_MULTIPLE, finalPosition.z);
        // Vector3 desiredPositionOnTheGround = new Vector3(finalPosition.x, finalPosition.y, finalPosition.z);

        wayPoints = new Vector3[] {currentPosition, startPositionInTheSky, desiredPositionInTheSky, finalPosition};
    }

    public virtual void SetPosition(Vector3 position, bool force = false) {
        if (force) {
            desiredPostion = position;
            // transform the piece immediately
            transform.position = desiredPostion; 
        } else {
            hasFinishedAnimation = false;
            needMovement = true;
            currentStartPointIndex = 0;
            createWaypoints(transform.position, position);
            SetPoints();
            desiredPostion = position;
            // MovePieceAnimation();
        }
    }

    public virtual void SetScale(Vector3 position, bool force = false) {
        desiredScale = position;
        if (force) {
            // transform the piece immediately
            transform.localScale = desiredScale; 
        }
    }


    public void triggerAttack() {
        animator.SetTrigger("Attack");
        
    }

    public void triggerDie() {
        animator.SetTrigger("Die");
    }

    public void triggerFroze() {
        animator.SetTrigger("Froze");
    }

    public bool DieAnimationIsPlaying() {
        return isAnimationPlaying("2");
    }

    public bool AttackAnimationIsPlaying() {
        return isAnimationPlaying("1");
    }

    bool isAnimationPlaying(string animationTag) {
        // return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);        
        

        // this is for the case after the die animation has finished.
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) { return false;}

        // Tag "1" = attack, tag "2" = die
        return animator.GetCurrentAnimatorStateInfo(0).IsTag(animationTag);        
    }

    // public bool AnimatorIsPlaying(string stateName = "Attack 1"){
    //     return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    // }

    // public bool isAnimatorPlaying() {
    //     return animator.GetCurrentAnimatorStateInfo(0).length >
    //         animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    // }

    void Start() {
        animator = GetComponent<Animator>();
    }

}
