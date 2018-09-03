using UnityEngine;
namespace RPG.Characters
{
    public class FaceCamera : MonoBehaviour
    {
        Camera cameraToLookAt;
        void Start()
        {
            cameraToLookAt = Camera.main;
           
        }

        // Update is called once per frame 
        void LateUpdate()
        {
            transform.LookAt(cameraToLookAt.transform);
        
        }
       
    }
}