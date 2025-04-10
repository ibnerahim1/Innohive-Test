using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace EggTapping
{
    public class InstructionTextUpdater : MonoBehaviour
    {
        public Text instructionText;
        public EggTapManager eggTapManager;
        
        private void Start()
        {
            if (instructionText == null)
            {
                Debug.LogError("Instruction Text reference is missing!");
                return;
            }
            
            if (eggTapManager == null)
            {
                Debug.LogError("EggTapManager reference is missing!");
                return;
            }
            
            // Set initial text
            instructionText.text = "Tap the egg to make it hatch!";
            
            // Start checking for state changes
            StartCoroutine(CheckEggState());
        }
        
        private IEnumerator CheckEggState()
        {
            bool isEggCracked = false;
            bool isCharacterRevealed = false;
            
            while (true)
            {
                // Check if egg is cracked by checking if stage2 or stage3 is active
                bool currentEggCracked = (eggTapManager.eggStage2 && eggTapManager.eggStage2.activeSelf) || 
                                        (eggTapManager.eggStage3 && eggTapManager.eggStage3.activeSelf);
                
                // Check if character is revealed
                bool currentCharacterRevealed = eggTapManager.character && eggTapManager.character.activeSelf;
                
                // Update instruction text based on state changes
                if (currentCharacterRevealed && !isCharacterRevealed)
                {
                    instructionText.text = "Your character has hatched!";
                    isCharacterRevealed = true;
                }
                else if (currentEggCracked && !isEggCracked)
                {
                    instructionText.text = "Keep tapping to break the egg!";
                    isEggCracked = true;
                }
                
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
