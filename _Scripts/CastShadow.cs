using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastShadow : MonoBehaviour
{
        [Header("Set In Inspector")]
        public Vector2 shadowOffset;
        public Material shadowMaterial;

        [Header("Set Dynamically")]
        SpriteRenderer spriteRenderer;
        GameObject shadowGameobject;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            shadowGameobject = new GameObject("Shadow");
            shadowGameobject.transform.SetParent(transform);
            
            SpriteRenderer shadowSpriteRenderer = shadowGameobject.AddComponent<SpriteRenderer>();

            shadowSpriteRenderer.sprite = spriteRenderer.sprite;
            
            shadowSpriteRenderer.material = shadowMaterial;

            shadowSpriteRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
            shadowSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        }

        void LateUpdate()
        {
            shadowGameobject.transform.position = transform.position + (Vector3)shadowOffset;
            shadowGameobject.transform.rotation = transform.rotation;
        }
    }
