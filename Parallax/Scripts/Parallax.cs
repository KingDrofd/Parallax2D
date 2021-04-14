
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


    
    [System.Serializable]
    public class ParallaxCam
    {
        public Transform camera;
        public Vector3 lastCamPos;
        public float parallaxEffectMultiplier = 0f;

    }


    void InitNoCamVars()
    {
        Debug.LogWarning("init");
        spriteParallax.spriteRenderer = GetComponent<SpriteRenderer>();
        spriteParallax.startPos = spriteParallax.spriteRenderer.transform.position;
        spriteParallax.itemSize = spriteParallax.spriteRenderer.bounds.size;
    }

     
    
    
    

    public enum ParallaxType
    {
        Camera, NoCamera
    }
    
    
    public enum Type
    {
        Left, Down
    }

   public ParallaxType parallaxDropDown;
   [HideInInspector] public Type type;
   [HideInInspector] public SpriteParallax spriteParallax;
   [HideInInspector] public ParallaxCam parallaxCam; 



    private void Start()
    {


        CheckChildren();
        
        InitNoCamVars();
        //parallaxCam.camera = Camera.main.transform;
        //parallaxCam.lastCamPos = parallaxCam.camera.position;



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

    void MoveParallax()
    {
        Vector3 deltaMovement = parallaxCam.camera.transform.position;

        transform.position += deltaMovement * parallaxCam.parallaxEffectMultiplier;
        parallaxCam.lastCamPos = parallaxCam.camera.position;
    }
    private void ExecuteNoCam()
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

    void ExecuteCam()
    {

    }

    private void LateUpdate()
    {
        //Vector3 deltaMovement = parallaxCam.camera.position - parallaxCam.lastCamPos;
        //transform.position += deltaMovement * parallaxCam.parallaxEffectMultiplier;
        //transform.position = parallaxCam.camera.position;
    }

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
                    MoveParallax();
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
            parallaxDropDown, parallaxCam1;

        private void OnEnable()
        {
            parallaxDropDown = serializedObject.FindProperty("parallaxDropDown");
            parallaxCam1 = serializedObject.FindProperty("parallaxCam");
        }
        public override void OnInspectorGUI()
        {
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
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(parallaxCam1);
                EditorGUILayout.EndVertical();

            }
            void NoCamera()
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.indentLevel++;

                parallax.spriteParallax.alternate = EditorGUILayout.Toggle("Alternate: ", parallax.spriteParallax.alternate);
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                parallax.spriteParallax.alternation = EditorGUILayout.IntField("Alternations: ", parallax.spriteParallax.alternation);
                parallax.spriteParallax.alternation = EditorGUILayout.IntSlider(parallax.spriteParallax.alternation, 2, 10);
                EditorGUILayout.EndHorizontal();
  

                parallax.spriteParallax.speed = EditorGUILayout.FloatField("Speed: ", parallax.spriteParallax.speed);

                EditorGUILayout.EndVertical();

            }

        }
    }
#endif
    #endregion
}
