using System.Collections;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource bubbleAudioSource;
    [SerializeField] private AudioClip[] bubbleAudioClips;
    [SerializeField] private AudioSource waterAudioSource;
    [SerializeField] private AudioClip[] waterAudioClips;
    [SerializeField] private int cooldown = 5;

    private int bubbleIndex = -1;
    private int waterIndex = -1;
    private void Start()
    {
        StartCoroutine(CooldownBubble(Mathf.RoundToInt(Random.Range(0, 5))));
        StartCoroutine(CooldownWater(Mathf.RoundToInt(Random.Range(0, 5))));
    }

    private void PlayBubble()
    {
        bubbleIndex = GetRandom(bubbleIndex, bubbleAudioClips.Length);
        AudioClip audioClip = bubbleAudioClips[bubbleIndex];
        bubbleAudioSource.clip = audioClip;
        bubbleAudioSource.Play();
        StartCoroutine(CooldownBubble(Mathf.RoundToInt(audioClip.length)));
    }

    private void PlayWater()
    {
        waterIndex = GetRandom(waterIndex, waterAudioClips.Length);
        AudioClip audioClip = waterAudioClips[waterIndex];
        waterAudioSource.clip = audioClip;
        waterAudioSource.Play();
        StartCoroutine(CooldownWater(Mathf.RoundToInt(audioClip.length)));
    }

    private IEnumerator CooldownBubble(int length)
    {
        yield return new WaitForSeconds(length + cooldown);
        PlayBubble();
    }

    private IEnumerator CooldownWater(int length)
    {
        yield return new WaitForSeconds(length + cooldown);
        PlayWater();
    }


    private int GetRandom(int index, int length)
    {
        int i = Random.Range(0, length);
        while ( i == index)
        {
            i = Random.Range(0, length);
        }
        return i;
    }
}
