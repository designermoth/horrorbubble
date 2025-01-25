using System.Collections;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;
using System.Numerics;

public class AbilityController : MonoBehaviour
{

    [SerializeField] Slider slider;
    [SerializeField] Light pointLight;

    [SerializeField] float duration = 5f;
    [SerializeField] float cooldown = 1f;

    float maxLightIntensity;

    float currentDuration;
    bool abilityInUse = false;
    bool onCooldown = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentDuration = duration;
        maxLightIntensity = pointLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) || currentDuration <= 0f)
        {
            abilityInUse = false;
            onCooldown = true;
            StartCoroutine(Cooldown());
            pointLight.DOIntensity(maxLightIntensity, 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !onCooldown)
        {
            abilityInUse = true;
            pointLight.DOIntensity(0, 0.5f);
        }
        if (abilityInUse) AbilityInUse();
        else
        {
            currentDuration += Time.deltaTime;
            currentDuration = Mathf.Clamp(currentDuration, 0f, duration);
        }
        slider.value = currentDuration / 5f;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    } 

    void AbilityInUse()
    {
        currentDuration -= Time.deltaTime;
        // Tween a Vector3 called myVector to 3,4,8 in 1 second
        //DOTween.To(() => myVector, x => myVector = x, new Vector3(3, 4, 8), 1);
        
        //DO SMTH
    }
}
