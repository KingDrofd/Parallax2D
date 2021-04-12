using UnityEngine;


public class Parallax : MonoBehaviour
{
    [System.Serializable]
    public class SpriteParallax
    {
        
       [HideInInspector] public Vector3 startPos;
       [HideInInspector]public Vector3 itemSize;
        public float speed;

        
       [HideInInspector] public SpriteRenderer spriteRenderer;
        #region       alternations

        
        public bool alternate = false;
        [@Range(2, 10, 2/*This can be any number depending how you will alternate and how many*/)]
        public int alternation; //Minimum is 2,
                                //sprite must end with what it started,
                                //example "1 0 1" going from 1 to 1 is better visually than going from 0 to 1
                                //in case of 4 alternation it would be, "1 0 1 0 1" and "1 1 0 1" works too, it's all 
        #endregion 
    }

    public enum Type
    {
        Left, Down
    }


    public Type type;
    public SpriteParallax spriteParallax;

    private void Start()
    {
            CheckChildren(); 
        
            spriteParallax.spriteRenderer = GetComponent<SpriteRenderer>();
            spriteParallax.startPos = spriteParallax.spriteRenderer.transform.position;
            spriteParallax.itemSize = spriteParallax.spriteRenderer.bounds.size;
    }

    #region errors
    void CheckChildren()
    {
       if(transform.childCount > spriteParallax.alternation && spriteParallax.alternate == true)
       {
                Debug.LogError("Incorrect Number Of Children Or Alternation On " + gameObject + ". Need:" + (transform.childCount - spriteParallax.alternation) + " Less " +
               " Children," + " Or " + (transform.childCount - spriteParallax.alternation) + " More " +
               " Alternation");
       }
       else if (transform.childCount < spriteParallax.alternation && spriteParallax.alternate == true)
       {
            Debug.LogError("Incorrect Number Of Children Or Alternation On " + gameObject + ". Need:" + (transform.childCount - spriteParallax.alternation) * (-1) + " More " +
           " Children," + " Or " + (transform.childCount - spriteParallax.alternation) * (-1) + " Less " +
           " Alternation");
       }
    }
    
    #endregion
    private void ParallaxMoveX()
    {   
        if (spriteParallax.alternate == true)
        {
            float newPos = Mathf.Repeat(Time.time * spriteParallax.speed, spriteParallax.itemSize.x * spriteParallax.alternation);
            transform.position = spriteParallax.startPos + Vector3.left * newPos;
            
        }else 
        {
            float newPos = Mathf.Repeat(Time.time * spriteParallax.speed, spriteParallax.itemSize.x);
            transform.position = spriteParallax.startPos + Vector3.left * newPos;
        }
    }
    private void ParallaxMoveY()
    {       
        if (spriteParallax.alternate == true)
        {
            float newPos = Mathf.Repeat(Time.time * spriteParallax.speed, spriteParallax.itemSize.y * spriteParallax.alternation);
            transform.position = spriteParallax.startPos + Vector3.down * newPos;
        }else 
        {
            float newPos = Mathf.Repeat(Time.time * spriteParallax.speed, spriteParallax.itemSize.y);
            transform.position = spriteParallax.startPos + Vector3.down * newPos;
        }
    }
    private void Initialize()
    {      
        switch (type)
        {
            case Type.Down:
                {
                    ParallaxMoveY();
                    break;
                }
            case Type.Left:
                {
                    ParallaxMoveX();
                    break;
                }
        }
    }
    private void FixedUpdate()
    {
        Initialize();
    }
}
