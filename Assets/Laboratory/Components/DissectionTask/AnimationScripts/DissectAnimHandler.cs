using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DissectAnimHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SkinnedMeshRenderer skinFlap, gillFlap, swimBladder;
    private bool _animatingSkinFlap = false, _animatingGillFlap = false, _animatingSwimBladder = false;

    private float _skinFlapBlend0 = 0f, _skinFlapBlend1 = 0f;
    private float _gillFlapBlend = 0f;
    private float _swimBladderBlend = 0f;

    private float _skinFlapSpeed, _gillFlapSpeed, _swimBladderSpeed;

    public UnityEvent m_SkinFlapAnimationCompleted;
    public UnityEvent m_GillFlapAnimationCompleted;
    public UnityEvent m_SwimBladderAnimationCompleted;

    // Start is called before the first frame update
    void Start()
    {
        if (!animator)
            Debug.LogError("Animator component not set in inspector");

        if (!skinFlap || !gillFlap || !swimBladder)
            Debug.LogError("SkinnedMeshRenderer components not set in inspector");

        m_SkinFlapAnimationCompleted ??= new();
        m_GillFlapAnimationCompleted ??= new();
        m_SwimBladderAnimationCompleted ??= new();
    }

    private void Update()
    {
        // The skin flap has two blend shapes. The animation goes as follows:
        // - Move blend shape 0 from 0 to 100.
        // - Move blend shape 0 from 100 to 0, while moving blend shape 1 from 0 to 100.
        if (_animatingSkinFlap)
        {
            if (_skinFlapBlend0 < 100 && _skinFlapBlend1 <= 0)
            {
                _skinFlapBlend0 += Mathf.Min(_skinFlapSpeed * Time.deltaTime, 100 - _skinFlapBlend0);
                skinFlap.SetBlendShapeWeight(0, _skinFlapBlend0);
            }
            else
            {
                if (_skinFlapBlend0 > 0 &&  _skinFlapBlend1 < 100)
                {
                    _skinFlapBlend0 -= Mathf.Min(_skinFlapSpeed * Time.deltaTime, _skinFlapBlend0);
                    skinFlap.SetBlendShapeWeight(0, _skinFlapBlend0);

                    _skinFlapBlend1 += Mathf.Min(_skinFlapSpeed * Time.deltaTime, 100 - _skinFlapBlend1);
                    skinFlap.SetBlendShapeWeight(1, _skinFlapBlend1);
                }
                else
                {
                    _animatingSkinFlap = false;
                    m_SkinFlapAnimationCompleted.Invoke();
                }
            }
        }

        // The gill flap has one blend shape. The animation goes as follows:
        // - Move blend shape all the way from 0 to 100
        if (_animatingGillFlap)
        {
            if (_gillFlapBlend < 100)
            {
                _gillFlapBlend += Mathf.Min(_gillFlapSpeed * Time.deltaTime, 100 - _gillFlapBlend);
                gillFlap.SetBlendShapeWeight(0, _gillFlapBlend);
            }
            else
            {
                _animatingGillFlap = false;
                m_GillFlapAnimationCompleted.Invoke();
            }
        }

        // The swim bladder has one blend shape. The animation goes as follows:
        // - Move blend shape all the way from 0 to 100
        if (_animatingSwimBladder)
        {
            if (_swimBladderBlend < 100)
            {
                _swimBladderBlend += Mathf.Min(_swimBladderSpeed * Time.deltaTime, 100 - _swimBladderBlend);
                swimBladder.SetBlendShapeWeight(0, _swimBladderBlend);
            }
            else
            {
                _animatingSwimBladder = false;
                m_SwimBladderAnimationCompleted.Invoke();
            }
        }
    }

    public void PlayStomachAnimation(float speed)
    {
        _skinFlapSpeed = speed;
        _animatingSkinFlap = true;
    }

    public void PlayGillAnimation(float speed)
    {
        _gillFlapSpeed = speed;
        _animatingGillFlap = true;
    }

    public void PlaySwimBladderAnimation(float speed)
    {
        _swimBladderSpeed = speed;
        _animatingSwimBladder = true;
    }

    public void ResetAnimations()
    {
        skinFlap.SetBlendShapeWeight(0, 0);
        skinFlap.SetBlendShapeWeight(1, 0);
        gillFlap.SetBlendShapeWeight(0, 0);
        swimBladder.SetBlendShapeWeight(0, 0);
    }
}
