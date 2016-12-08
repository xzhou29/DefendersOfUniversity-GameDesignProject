using System.Collections.Generic;
using UnityEngine;

abstract public class Unit : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public Point GridPosition { get; set; }

    protected Animator myAnimator;

    private SpriteRenderer spriteRenderer;

    private Stack<Node> path;

    private Vector3 destination;

    public bool IsActive { get; set; }

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            this.speed = value;
        }
    }

    protected virtual void Awake()
    {

        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        Move();
    
    }
    

    public void Animate(Point currentPos, Point newPos)
    {

        //If we are moving down
        if (currentPos.Y > newPos.Y)
        {
            myAnimator.SetFloat("Horizontal", 0);

            myAnimator.SetFloat("Vertical", 1);
        }
        //IF we are moving up
        else if (currentPos.Y < newPos.Y)
        {
            myAnimator.SetFloat("Horizontal", 0);
            myAnimator.SetFloat("Vertical", -1);
        }
        //If we aren't moving up or down
        if (currentPos.Y == newPos.Y)
        {
            //If we are moving left
            if (currentPos.X > newPos.X)
            {
                myAnimator.SetFloat("Vertical", 0);
                myAnimator.SetFloat("Horizontal", -1);
            }
            //If we are moving right
            else if (currentPos.X < newPos.X)
            {
                myAnimator.SetFloat("Vertical", 0);
                myAnimator.SetFloat("Horizontal", 1);
            }
        }
        //// for DEBUG ONLY
        //if (myAnimator.layerCount > 1)
        //{
        //    myAnimator.SetLayerWeight(1, 1);
        //}

    }
    public void Move()
    {
        if (IsActive) 
        {

            transform.position = Vector2.MoveTowards(transform.position, destination, Speed * Time.deltaTime);

            //Checks if we arrived at the destination
            if (transform.position == destination)
            {
           
                if (path != null && path.Count > 0)
                {             
                    Animate(GridPosition, path.Peek().GridPosition);

                    GridPosition = path.Peek().GridPosition;

                    //Sets a new destination
                    destination = path.Pop().WorldPosition;

                }
                else //If we don't have a path then we are done moving
                {
                    // FOR THE DEBUG ONLY
                    //if (myAnimator.layerCount > 1)
                    //{
                    //    myAnimator.SetLayerWeight(1, 0);
                    //}

                    IsActive = false;
                }
            }
        }
    }

    public void SetPath(Stack<Node> newPath, bool active)
    {
        if (newPath != null) //If we have a path
        {    
            this.path = newPath;
            Animate(GridPosition, path.Peek().GridPosition);

            GridPosition = path.Peek().GridPosition;

            destination = path.Pop().WorldPosition;

            this.IsActive = active;
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Tile") 
        {
            spriteRenderer.sortingOrder = (int)other.GetComponent<TileScript>().GridPosition.Y;
        }
    }

}