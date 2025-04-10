using UnityEngine;
using System.Collections;

namespace EggTapping
{
    public class EggTapManager : MonoBehaviour
    {
        // Egg stages to visualize cracking
        public GameObject eggStage1;
        public GameObject eggStage2;
        public GameObject eggStage3;
        public GameObject character;

        // Tap counter and required taps
        private int tapCount = 0;
        public int requiredTapsToReveal = 5;

        // Animation variables
        public float characterRevealDuration = 1.0f;
        public float characterBobSpeed = 0.5f;
        public float characterBobHeight = 0.1f;
        
        private bool isCharacterRevealed = false;
        private Vector3 characterStartPosition;

        private void Start()
        {
            // Make sure only the first egg stage is visible at start
            if (eggStage1) eggStage1.SetActive(true);
            if (eggStage2) eggStage2.SetActive(false);
            if (eggStage3) eggStage3.SetActive(false);
            if (character) 
            {
                character.SetActive(false);
                characterStartPosition = character.transform.localPosition;
            }
            
            // Add a mesh collider to this object to detect taps
            MeshCollider meshCollider = GetComponent<MeshCollider>();
            if (meshCollider == null)
            {
                meshCollider = gameObject.AddComponent<MeshCollider>();
            }
        }
        
        private void Update()
        {
            // Handle input for mobile (touch) and desktop (mouse click)
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                HandleTap(Input.GetTouch(0).position);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                HandleTap(Input.mousePosition);
            }
            
            // If character is revealed, make it bob up and down
            if (isCharacterRevealed && character != null)
            {
                float bobOffset = Mathf.Sin(Time.time * characterBobSpeed) * characterBobHeight;
                character.transform.localPosition = characterStartPosition + new Vector3(0, bobOffset, 0);
            }
        }

        private void HandleTap(Vector3 screenPosition)
        {
            // Cast a ray from screen to world
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                // Check if we hit the egg
                Transform hitTransform = hit.transform;
                if (hitTransform == transform || hitTransform.IsChildOf(transform))
                {
                    ProgressEggCracking();
                }
            }
        }

        private void ProgressEggCracking()
        {
            // Don't do anything if character is already revealed
            if (isCharacterRevealed)
                return;
                
            tapCount++;
            Debug.Log("Tap count: " + tapCount);
            
            // Progress through egg cracking stages
            if (tapCount == Mathf.CeilToInt(requiredTapsToReveal * 0.33f))
            {
                ShowEggStage(2);
            }
            else if (tapCount == Mathf.CeilToInt(requiredTapsToReveal * 0.66f))
            {
                ShowEggStage(3);
            }
            else if (tapCount >= requiredTapsToReveal)
            {
                RevealCharacter();
            }
        }
        
        private void ShowEggStage(int stage)
        {
            // Hide all stages first
            if (eggStage1) eggStage1.SetActive(false);
            if (eggStage2) eggStage2.SetActive(false);
            if (eggStage3) eggStage3.SetActive(false);
            
            // Show the requested stage
            switch (stage)
            {
                case 1:
                    if (eggStage1) eggStage1.SetActive(true);
                    break;
                case 2:
                    if (eggStage2) eggStage2.SetActive(true);
                    break;
                case 3:
                    if (eggStage3) eggStage3.SetActive(true);
                    break;
            }
        }
        
        private void RevealCharacter()
        {
            // Hide all egg stages
            if (eggStage1) eggStage1.SetActive(false);
            if (eggStage2) eggStage2.SetActive(false);
            if (eggStage3) eggStage3.SetActive(false);
            
            // Show character with animation
            if (character)
            {
                isCharacterRevealed = true;
                character.SetActive(true);
                StartCoroutine(AnimateCharacterReveal());
            }
        }
        
        private IEnumerator AnimateCharacterReveal()
        {
            float elapsedTime = 0;
            
            // Starting scale (very small)
            character.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            // Target scale
            Vector3 targetScale = new Vector3(0.4f, 0.6f, 0.4f);
            
            // Animate scale over time
            while (elapsedTime < characterRevealDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / characterRevealDuration;
                
                // Smooth step for nicer animation
                t = t * t * (3f - 2f * t); 
                
                // Apply scale
                character.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), targetScale, t);
                
                yield return null;
            }
            
            // Ensure final scale is exact
            character.transform.localScale = targetScale;
        }
    }
}
