using UnityEngine;
using DG.Tweening;

public class EggTapManager : MonoBehaviour
{
    private int tapCount = 1;
    public Transform character, HitFX, RevealFX;
    // Animation variables
    public float characterRevealDuration = 1.0f;
    public float characterBobSpeed = 0.5f;
    public float characterBobHeight = 0.1f;

    public bool isCharacterRevealed = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTap(Input.mousePosition);
        }
    }

    private void HandleTap(Vector3 screenPosition)
    {
        // Don't do anything if character is already revealed
        if (isCharacterRevealed)
            return;

        transform.DOPunchRotation(new Vector3(0, 0, 5), 0.2f);
        transform.GetChild(tapCount).gameObject.SetActive(false);
        Instantiate(HitFX, new Vector3(0, 2, -1), Quaternion.identity);
        if (tapCount == 8)
        {
            isCharacterRevealed = true;
            Instantiate(RevealFX, new Vector3(0, 2, -1), Quaternion.identity);
            character.DOMoveY(2, 2f).SetEase(Ease.OutBack);
            character.DOScale(Vector3.one, 2f).SetEase(Ease.OutBack);
            Camera.main.transform.DOLookAt(Vector3.up * 2.5f, 2f).SetEase(Ease.OutBack);
            Invoke("Restart", 4f);
        }
        tapCount++;
        Debug.Log("Tap count: " + tapCount);
    }
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}