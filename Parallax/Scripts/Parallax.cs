using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Parallax : MonoBehaviour
{
    
    [System.Serializable]
    public class SpriteParallax
    {
        
       [HideInInspector] public Vector3 startPos;
       [HideInInspector] public Vector3 itemSize;
       [HideInInspector] public float speed;        
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


    
    [System.Serializable]
    public class ParallaxCam
    {
        public Texture2D texture;
        public Sprite sprite;
        public Transform camera;
        public Vector3 lastCamPos;
        public Vector2 parallaxEffectMultiplier;
        public bool horizontal, vertical;
        public float itemSizeX;
        public float itemSizeY;
        public float posX;
        public float posY;
    }


    public enum ParallaxType
    {
        Camera, NoCamera
    }
    
    
    public enum Direction
    {
        Left, Down, Up, Right
    }

    public enum CamWay
    {
        Horizontal, Vertical
    }


   public ParallaxType parallaxDropDown;

    [HideInInspector] public CamWay camWay;
    [HideInInspector] public Direction type;
    [HideInInspector] public SpriteParallax spriteParallax;
    [HideInInspector] public ParallaxCam parallaxCam; 
    




    private void Start()
    {


        CheckChildren();
        
        InitNoCamVars();

        InitCamVars();
        



    }

    #region ErrorsChecking
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


    #region Methods



    void InitCamVars()
    {
        parallaxCam.camera = Camera.main.transform;
        parallaxCam.lastCamPos = parallaxCam.camera.position;
        parallaxCam.sprite = GetComponent<SpriteRenderer>().sprite;
        parallaxCam.texture = parallaxCam.sprite.texture;
        parallaxCam.itemSizeX = parallaxCam.texture.width / parallaxCam.sprite.pixelsPerUnit;
        parallaxCam.itemSizeY = parallaxCam.texture.height / parallaxCam.sprite.pixelsPerUnit;
        parallaxCam.posX = parallaxCam.camera.position.x;
        parallaxCam.posY = parallaxCam.camera.position.y;
    }
    
    void InitNoCamVars()
    {
        
        spriteParallax.spriteRenderer = GetComponent<SpriteRenderer>();
        spriteParallax.startPos = spriteParallax.spriteRenderer.transform.position;
        spriteParallax.itemSize = spriteParallax.spriteRenderer.bounds.size;
    }


    private void ParallaxMoveX() //Move the parallax to the left
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
    private void XParallaxMove() //Move the parallax to the right
    {
        if (spriteParallax.alternate == true)
        {
            float newPos = Mathf.Repeat(Time.time * spriteParallax.speed, spriteParallax.itemSize.x * spriteParallax.alternation);
            transform.position = spriteParallax.startPos + Vector3.right * newPos;

        }
        else
        {
            float newPos = Mathf.Repeat(Time.time * spriteParallax.speed, spriteParallax.itemSize.x);
            transform.position = spriteParallax.startPos + Vector3.right * newPos;
        }
    }
    private void ParallaxMoveY()//Move the parallax Down
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
    private void YParallaxMove() //Move the Parallax Up
    {
        if (spriteParallax.alternate == true)
        {
            float newPos = Mathf.Repeat(Time.time * spriteParallax.speed, spriteParallax.itemSize.y * spriteParallax.alternation);
            transform.position = spriteParallax.startPos + Vector3.up * newPos;
        }
        else
        {
            float newPos = Mathf.Repeat(Time.time * spriteParallax.speed, spriteParallax.itemSize.y);
            transform.position = spriteParallax.startPos + Vector3.up * newPos;
        }
    }

    void MoveParallaxHorizontal()
    {
        Vector3 deltaMovement = parallaxCam.camera.position - parallaxCam.lastCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxCam.parallaxEffectMultiplier.x, deltaMovement.y * parallaxCam.parallaxEffectMultiplier.y);
        parallaxCam.lastCamPos = parallaxCam.camera.position;
        if (parallaxCam.horizontal)
        {
            if (Mathf.Abs(parallaxCam.camera.position.x - transform.position.x) >= parallaxCam.itemSizeX)
            {
                float offsetPositionX = (parallaxCam.camera.position.x - transform.position.x) % parallaxCam.itemSizeX;
                transform.position = new Vector3(parallaxCam.camera.position.x + offsetPositionX, transform.position.y, 0);
            }
        }

    }
    void MoveParallaxVertical()
    {
        Vector3 deltaMovement = parallaxCam.camera.position - parallaxCam.lastCamPos;
        transform.position -= new Vector3(deltaMovement.x * parallaxCam.parallaxEffectMultiplier.x, deltaMovement.y * parallaxCam.parallaxEffectMultiplier.y);
        parallaxCam.lastCamPos = parallaxCam.camera.position;
        if (parallaxCam.vertical)
        {
            if (Mathf.Abs(parallaxCam.camera.position.y - transform.position.y) >= parallaxCam.itemSizeY)
            {
                float offsetPositionY = (parallaxCam.camera.position.y - transform.position.y) % parallaxCam.itemSizeY;
                transform.position = new Vector3(transform.position.x, parallaxCam.camera.position.y + offsetPositionY, 0);
            }
        }
    }


    private void ExecuteNoCam() //The Parallax Moves on its own depanding on the speed the user chose !Not Related to the Camera
    {      
        
        switch (type)
        {
            case Direction.Up:
                {
                    YParallaxMove();
                    break;
                }
            case Direction.Down:
                {
                    ParallaxMoveY();
                    break;
                }
            case Direction.Left:
                {
                    ParallaxMoveX();
                    break;
                }
            case Direction.Right:
                {
                    XParallaxMove();
                    break;
                }
        }
    }

    void ExecuteCam()
    {
        switch (camWay)
        {
            case CamWay.Horizontal:
                {
                    MoveParallaxHorizontal();
                    break;
                }
            case CamWay.Vertical:
                {
                    MoveParallaxVertical();
                    break;
                }

        }
    }

    #endregion


    private void FixedUpdate()
    {
        switch(parallaxDropDown)
        {
            case ParallaxType.NoCamera:
                {
                    ExecuteNoCam();
                    break;
                }
                
            case ParallaxType.Camera:
                {
                    ExecuteCam();
                    
                    break;
                }               
        }
        
    }


    #region ParallaxEditor
#if UNITY_EDITOR

    [CustomEditor(typeof(Parallax))]
    public class ParallaxEditor : Editor
    {
        public SerializedProperty
            parallaxDropDown, parallaxCam1, direction;

        private void OnEnable()
        {
            parallaxDropDown = serializedObject.FindProperty("parallaxDropDown");
            parallaxCam1 = serializedObject.FindProperty("parallaxCam");
            direction = serializedObject.FindProperty("type");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
           base.OnInspectorGUI();

            Parallax parallax = (Parallax)target;
            ParallaxType parallaxType = (ParallaxType)parallaxDropDown.enumValueIndex;
            
            switch(parallaxType)
            {
                case ParallaxType.Camera:
                    Camera();
                    break;
                case ParallaxType.NoCamera:                    
                    NoCamera();
                    break;
            }

            
            void Camera()
            {
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();
                parallax.parallaxCam.parallaxEffectMultiplier = EditorGUILayout.Vector2Field("Effect:", parallax.parallaxCam.parallaxEffectMultiplier);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical();
                parallax.parallaxCam.horizontal = EditorGUILayout.Toggle("Horizontal:", parallax.parallaxCam.horizontal);
                parallax.parallaxCam.vertical = EditorGUILayout.Toggle("vertical:", parallax.parallaxCam.vertical);
                EditorGUILayout.EndVertical();

                


            }
            
            void NoCamera()
            {                
                EditorGUILayout.BeginHorizontal();
                EditorGUI.indentLevel++;

                parallax.type = (Direction)EditorGUILayout.EnumPopup("Direction", parallax.type); //direction of the parallax dropdown

                parallax.spriteParallax.alternate = EditorGUILayout.Toggle("Alternate: ", parallax.spriteParallax.alternate);//alternation toggle
                EditorGUILayout.EndHorizontal();
                if(parallax.spriteParallax.alternate == true)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();
                    parallax.spriteParallax.alternation = EditorGUILayout.IntField("Alternations: ", parallax.spriteParallax.alternation);//Int alternations to choose how many alternations there will be
                    parallax.spriteParallax.alternation = EditorGUILayout.IntSlider(parallax.spriteParallax.alternation, 2, 10); //handy IntSlider for alternations
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }              

                EditorGUILayout.BeginVertical();
                parallax.spriteParallax.speed = EditorGUILayout.FloatField("Speed: ", parallax.spriteParallax.speed);//Speed of the Parallax Effect
                EditorGUILayout.EndVertical();
 
            }

           serializedObject.ApplyModifiedProperties();

        }
    }
#endif
    #endregion
}
