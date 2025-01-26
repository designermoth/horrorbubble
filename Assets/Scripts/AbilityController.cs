using System.Collections;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;
using System.Numerics;

public class AbilityController : MonoBehaviour
{
    [SerializeField] Light pointLight;

    [SerializeField] float duration = 5f;
    [SerializeField] float cooldown = 1f;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] normalBreath;
    [SerializeField] AudioClip[] holdBreath;
    [SerializeField] AudioClip[] recoverBreath;

    float maxLightIntensity;

    float currentDuration;

    public bool abilityInUse = false;
    bool onCooldown = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayNormal();
        currentDuration = duration;
        maxLightIntensity = pointLight.intensity;
    }

    void PlayNormal()
    {
        audioSource.clip = normalBreath[Random.Range(0, normalBreath.Length)];
        audioSource.loop = true;
        audioSource.Play();
    }

    void PlayHold()
    {
        audioSource.clip = holdBreath[Random.Range(0, holdBreath.Length)];
        audioSource.loop = false;
        audioSource.Play();
    }

    void PlayRecover()
    {
        audioSource.clip = recoverBreath[Random.Range(0, recoverBreath.Length)];
        audioSource.loop = false;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) || currentDuration <= 0f)
        {
            PlayRecover();
            abilityInUse = false;
            onCooldown = true;
            StartCoroutine(Cooldown());
            StartCoroutine(StartBreath(Mathf.RoundToInt(audioSource.clip.length)));
            pointLight.DOIntensity(maxLightIntensity, 1.5f).SetEase(Ease.InSine);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !onCooldown)
        {
            PlayHold();
            abilityInUse = true;
            pointLight.DOIntensity(0, 0.5f);
        }
        if (abilityInUse) AbilityInUse();
        else
        {
            currentDuration += Time.deltaTime;
            currentDuration = Mathf.Clamp(currentDuration, 0f, duration);
        }
    }

    IEnumerator StartBreath(int length)
    {
        yield return new WaitForSeconds(length);
        PlayNormal();
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    void AbilityInUse()
    {
        currentDuration -= Time.deltaTime;

        //DO SMTH
    }
}
