using UnityEngine;

public class Parrallax : MonoBehaviour
{
   
        [SerializeField] private float parallaxMultiplier = 0.1f;
        private Material parallaxMaterial;
        private float lastPlayerX;
        [SerializeField] private Transform player;

        void Start()
        {
            parallaxMaterial = GetComponent<Renderer>().material;
            lastPlayerX = player.position.x;
        }

        void Update()
        {
            float deltax = player.position.x - lastPlayerX;
            parallaxMaterial.mainTextureOffset += new Vector2(deltax * parallaxMultiplier, 0);
            lastPlayerX = player.position.x;
        }
    

}
