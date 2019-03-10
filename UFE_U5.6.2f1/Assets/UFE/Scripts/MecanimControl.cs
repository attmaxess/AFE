using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MecanimAnimationData {
	public AnimationClip clip;
	public string clipName;
	public float speed = 1;
	public float transitionDuration = -1;
	public WrapMode wrapMode;
	public bool applyRootMotion;
	[HideInInspector] public int timesPlayed = 0;
	[HideInInspector] public float secondsPlayed = 0;
	[HideInInspector] public float length = 1;
    [HideInInspector] public float originalSpeed = 1;
    [HideInInspector] public float normalizedSpeed = 1;
    [HideInInspector] public float normalizedTime = 1;
	[HideInInspector] public string stateName;
}

[RequireComponent (typeof (Animator))]
public class MecanimControl : MonoBehaviour {

	public MecanimAnimationData defaultAnimation = new MecanimAnimationData();
	public MecanimAnimationData[] animations = new MecanimAnimationData[0];
	public bool debugMode = false;
	public bool alwaysPlay = false;
	public bool overrideRootMotion = false;
    public bool overrideAnimatorUpdate = false;
	public float defaultTransitionDuration = 0.15f;
	public WrapMode defaultWrapMode = WrapMode.Loop;

	public Animator animator;
	
	private RuntimeAnimatorController controller;
    public AnimatorOverrideController overrideController;
    public float currentNormalizedTime;
    public string currentState;
    public float currentSpeed;

	public MecanimAnimationData currentAnimationData;
	public bool currentMirror;

	public delegate void AnimEvent(MecanimAnimationData animationData);
	public static event AnimEvent OnAnimationBegin;
	public static event AnimEvent OnAnimationEnd;
	public static event AnimEvent OnAnimationLoop;
	
	// UNITY METHODS
	void Awake () {
		animator = gameObject.GetComponent<Animator>();
        animator.logWarnings = false;
        controller = (RuntimeAnimatorController)Instantiate(Resources.Load("MC_Controller"));

        foreach (MecanimAnimationData animData in animations) {
			if (animData.wrapMode == WrapMode.Default) animData.wrapMode = defaultWrapMode;
			animData.clip.wrapMode = animData.wrapMode;
		}

	}
	
	void Start(){
		if (defaultAnimation.clip == null && animations.Length > 0){
			SetDefaultClip(animations[0].clip, "State1", animations[0].speed, animations[0].wrapMode, false);
		}

        if (defaultAnimation.clip != null && currentAnimationData == null) {
			foreach(MecanimAnimationData animData in animations) {
				if (animData.clip == defaultAnimation.clip)
					defaultAnimation.clip = (AnimationClip) Instantiate(defaultAnimation.clip);
			}
			
			currentAnimationData = defaultAnimation;
			currentAnimationData.stateName = "State1";
			currentAnimationData.length = currentAnimationData.clip.length;

            AnimatorOverrideController overrideController = new AnimatorOverrideController();
            overrideController.runtimeAnimatorController = controller;
            overrideController["Default"] = currentAnimationData.clip;
            overrideController["State1"] = currentAnimationData.clip;

			animator.runtimeAnimatorController = overrideController;
			animator.Play("State1", 0, 0);

			if (overrideRootMotion) animator.applyRootMotion = currentAnimationData.applyRootMotion;
			SetSpeed(currentAnimationData.speed);
		}
	}
	
	public void DoFixedUpdate(){
        //WrapMode emulator
        if (overrideAnimatorUpdate) {
            animator.enabled = false;
            animator.Update(Time.fixedDeltaTime);
        }

        if (currentAnimationData == null || currentAnimationData.clip == null) return;


        currentAnimationData.secondsPlayed += (Time.fixedDeltaTime * GetSpeed());
        if (currentAnimationData.secondsPlayed > currentAnimationData.length) {
            currentAnimationData.secondsPlayed = currentAnimationData.length;
        }
        currentAnimationData.normalizedTime = currentAnimationData.secondsPlayed / currentAnimationData.length;

		if (currentAnimationData.secondsPlayed == currentAnimationData.length){
			if (currentAnimationData.clip.wrapMode == WrapMode.Loop || currentAnimationData.clip.wrapMode == WrapMode.PingPong) {
				if (MecanimControl.OnAnimationLoop != null) MecanimControl.OnAnimationLoop(currentAnimationData);
				currentAnimationData.timesPlayed ++;
				
				if (currentAnimationData.clip.wrapMode == WrapMode.Loop) {
					SetCurrentClipPosition(0);
				}
				
				if (currentAnimationData.clip.wrapMode == WrapMode.PingPong) {
					SetSpeed(currentAnimationData.clipName, -currentAnimationData.speed);
					SetCurrentClipPosition(0);
				}
				
			}else if (currentAnimationData.timesPlayed == 0) {
				if (MecanimControl.OnAnimationEnd != null) MecanimControl.OnAnimationEnd(currentAnimationData);
				currentAnimationData.timesPlayed = 1;

                if ((currentAnimationData.clip.wrapMode == WrapMode.Once ||
                    currentAnimationData.clip.wrapMode == WrapMode.Clamp)
                    && alwaysPlay) {
                    Play(defaultAnimation, currentMirror);
                } else if (!alwaysPlay) {
                    SetSpeed(0);
                }
			}
		}
	}
	
	void OnGUI(){
		//Toggle debug mode to see the live data in action
		if (debugMode) {
			GUI.Box (new Rect (Screen.width - 340,40,340,420), "Animation Data");
			GUI.BeginGroup(new Rect (Screen.width - 330,60,400,420));{
				
				AnimatorClipInfo[] animationInfoArray = animator.GetCurrentAnimatorClipInfo(0);
				foreach (AnimatorClipInfo animationInfo in animationInfoArray){
					AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
					GUILayout.Label(animationInfo.clip.name);
					GUILayout.Label("-Wrap Mode: "+ animationInfo.clip.wrapMode);
					GUILayout.Label("-Is Playing: "+ IsPlaying(animationInfo.clip));
					GUILayout.Label("-Blend Weight: "+ animationInfo.weight);
					GUILayout.Label("-Normalized Time: "+ animatorStateInfo.normalizedTime);
					GUILayout.Label("-True Length: "+ animationInfo.clip.length);
					GUILayout.Label("----");
				}

                GUILayout.Label("Global Speed: " + GetSpeed().ToString());

				GUILayout.Label("--Current Animation Data--");
                GUILayout.Label("-Clip Name: " + currentAnimationData.clipName);
                GUILayout.Label("-Animation Speed: " + GetSpeed(currentAnimationData).ToString());
				GUILayout.Label("-Normalized Speed: "+ GetNormalizedSpeed(currentAnimationData));
				GUILayout.Label("-Times Played: "+ currentAnimationData.timesPlayed);
				GUILayout.Label("-Seconds Played: "+ currentAnimationData.secondsPlayed);
                GUILayout.Label("-Emulated Length: " + currentAnimationData.length);
                GUILayout.Label("-Normalized Time: " + currentAnimationData.normalizedTime);
			}GUI.EndGroup();
		}
	}
	


	// MECANIM CONTROL METHODS
	public void RemoveClip(string name) {
		List<MecanimAnimationData> animationDataList = new List<MecanimAnimationData>(animations);
		animationDataList.Remove(GetAnimationData(name));
		animations = animationDataList.ToArray();
	}

	public void RemoveClip(AnimationClip clip) {
		List<MecanimAnimationData> animationDataList = new List<MecanimAnimationData>(animations);
		animationDataList.Remove(GetAnimationData(clip));
		animations = animationDataList.ToArray();
	}
	
	public void SetDefaultClip(AnimationClip clip, string name, float speed, WrapMode wrapMode, bool mirror) {
		defaultAnimation.clip = (AnimationClip) Instantiate(clip);
		defaultAnimation.clip.wrapMode = wrapMode;
		defaultAnimation.clipName = name;
		defaultAnimation.speed = speed;
		defaultAnimation.originalSpeed = speed;
		defaultAnimation.transitionDuration = -1;
		defaultAnimation.wrapMode = wrapMode;
	}
	
	public void AddClip(AnimationClip clip, string newName) {
		AddClip(clip, newName, 1, defaultWrapMode);
	}

    public void AddClip(AnimationClip clip, string newName, float speed, WrapMode wrapMode) {
        AddClip(clip, newName, speed, wrapMode, clip.length);
    }

    public void AddClip(AnimationClip clip, string newName, float speed, WrapMode wrapMode, float length) {
		if (GetAnimationData(newName) != null) Debug.LogWarning("An animation with the name '"+ newName +"' already exists.");
		MecanimAnimationData animData = new MecanimAnimationData();
		animData.clip = (AnimationClip) Instantiate(clip);
		if (wrapMode == WrapMode.Default) wrapMode = defaultWrapMode;
		animData.clip.wrapMode = wrapMode;
		animData.clip.name = newName;
		animData.clipName = newName;
        animData.speed = speed;
        animData.originalSpeed = speed;
        animData.length = length;
		animData.wrapMode = wrapMode;

		List<MecanimAnimationData> animationDataList = new List<MecanimAnimationData>(animations);
		animationDataList.Add(animData);
		animations = animationDataList.ToArray();
	}

	public MecanimAnimationData GetAnimationData(string clipName){
		foreach(MecanimAnimationData animData in animations){
			if (animData.clipName == clipName){
				return animData;
			}
		}
		if (clipName == defaultAnimation.clipName) return defaultAnimation;
		return null;
	}

	public MecanimAnimationData GetAnimationData(AnimationClip clip){
		foreach(MecanimAnimationData animData in animations){
			if (animData.clip == clip){
				return animData;
			}
		}
		if (clip == defaultAnimation.clip) return defaultAnimation;
		return null;
	}

    public void CopyAnimationData(MecanimAnimationData from, ref MecanimAnimationData to) {
        if (from == null || from.clip == null) return;
	    to.clip = (AnimationClip) Instantiate(from.clip);
		to.clip.wrapMode = from.clip.wrapMode;
		to.clip.name = from.clip.name;
		to.clipName = from.clipName;
        to.speed = from.speed;
        to.transitionDuration = from.transitionDuration;
        to.wrapMode = from.wrapMode;
        to.applyRootMotion = from.applyRootMotion;
        to.timesPlayed = from.timesPlayed;
        to.secondsPlayed = from.secondsPlayed;
        to.length = from.length;
        to.originalSpeed = from.originalSpeed;
        to.normalizedSpeed = from.normalizedSpeed;
        to.normalizedTime = from.normalizedTime;
        to.stateName = from.stateName;
    }
	
	public void CrossFade(string clipName, float blendingTime){
		CrossFade(clipName, blendingTime, 0, currentMirror);
	}

	public void CrossFade(string clipName, float blendingTime, float normalizedTime, bool mirror){
		_playAnimation(GetAnimationData(clipName), blendingTime, normalizedTime, mirror);
	}
	
	public void CrossFade(MecanimAnimationData animationData, float blendingTime, float normalizedTime, bool mirror){
		_playAnimation(animationData, blendingTime, normalizedTime, mirror);
	}

	public void Play(string clipName, float blendingTime, float normalizedTime, bool mirror){
		_playAnimation(GetAnimationData(clipName), blendingTime, normalizedTime, mirror);
	}
	
	public void Play(AnimationClip clip, float blendingTime, float normalizedTime, bool mirror){
		_playAnimation(GetAnimationData(clip), blendingTime, normalizedTime, mirror);
	}

	public void Play(string clipName, bool mirror){
		_playAnimation(GetAnimationData(clipName), 0, 0, mirror);
	}

	public void Play(string clipName){
		_playAnimation(GetAnimationData(clipName), 0, 0, currentMirror);
	}
	
	public void Play(AnimationClip clip, bool mirror){
		_playAnimation(GetAnimationData(clip), 0, 0, mirror);
	}

	public void Play(AnimationClip clip){
		_playAnimation(GetAnimationData(clip), 0, 0, currentMirror);
	}

	public void Play(MecanimAnimationData animationData, bool mirror){
		_playAnimation(animationData, animationData.transitionDuration, 0, mirror);
	}

	public void Play(MecanimAnimationData animationData){
		_playAnimation(animationData, animationData.transitionDuration, 0, currentMirror);
	}
	
	public void Play(MecanimAnimationData animationData, float blendingTime, float normalizedTime, bool mirror){
		_playAnimation(animationData, blendingTime, normalizedTime, mirror);
	}

	public void Play(){
        SetSpeed(currentAnimationData.speed);
	}


	private void _playAnimation(MecanimAnimationData targetAnimationData, float blendingTime, float normalizedTime, bool mirror){
		//The overrite machine. Creates an overrideController, replace its core animations and restate it back in
		if (targetAnimationData == null || targetAnimationData.clip == null) return;

        bool prevMirror = currentMirror;
        currentMirror = mirror;

        float animSpeed = targetAnimationData.originalSpeed * (targetAnimationData.originalSpeed < 0 ? -1 : 1);

        currentNormalizedTime = GetCurrentClipPosition();
        currentState = "State1";

        if (!mirror){
            if (targetAnimationData.originalSpeed >= 0){
                currentState = "State1";
			}else{
                currentState = "State2";
			}
		}else{
            if (targetAnimationData.originalSpeed >= 0){
                currentState = "State3";
			}else{
                currentState = "State4";
			}
		}


        overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = controller;

        if (currentAnimationData != null && currentAnimationData.clip != null) 
            overrideController["Default"] = currentAnimationData.clip;

        overrideController[currentState] = targetAnimationData.clip;

        if (blendingTime == -1) blendingTime = currentAnimationData.transitionDuration;
		if (blendingTime == -1) blendingTime = defaultTransitionDuration;

		if (blendingTime <= 0 || currentAnimationData == null){
			animator.runtimeAnimatorController = overrideController;
            animator.Play(currentState, 0, normalizedTime);

		}else{
			animator.runtimeAnimatorController = overrideController;
            
			currentAnimationData.stateName = "Default";
			SetCurrentClipPosition(currentNormalizedTime);

            animator.Play("Default", 0, normalizedTime);
            animator.CrossFade(currentState, (blendingTime / animSpeed), 0, normalizedTime);
        }

        // Update Previous Mirror
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Default")) {
            if (animator.GetBool("Mirror") != prevMirror) {
                animator.SetBool("Mirror", prevMirror);
            }
        }
        animator.Update(0);

        targetAnimationData.timesPlayed = 0;
        targetAnimationData.secondsPlayed = (normalizedTime * targetAnimationData.length) / animSpeed;
        targetAnimationData.normalizedTime = normalizedTime;
        targetAnimationData.speed = targetAnimationData.originalSpeed;

        if (overrideRootMotion) animator.applyRootMotion = targetAnimationData.applyRootMotion;
        SetSpeed(targetAnimationData.originalSpeed);
		
        if (currentAnimationData != null) {
            currentAnimationData.speed = currentAnimationData.originalSpeed;
            currentAnimationData.normalizedSpeed = 1;
            currentAnimationData.timesPlayed = 0;
        }

		currentAnimationData = targetAnimationData;
        currentAnimationData.stateName = currentState;

		if (MecanimControl.OnAnimationBegin != null) MecanimControl.OnAnimationBegin(currentAnimationData);
	}

    public void RestoreAnimatorData(MecanimAnimationData animData, float normalizedTime) {
        currentAnimationData = animData;
        currentAnimationData.normalizedTime = normalizedTime;
        currentAnimationData.secondsPlayed = (normalizedTime * currentAnimationData.length) / currentAnimationData.speed;
    }
	
	public bool IsPlaying(string clipName){
		return IsPlaying(GetAnimationData(clipName));
	}
	
	public bool IsPlaying(string clipName, float weight){
		return IsPlaying(GetAnimationData(clipName), weight);
	}
	
	public bool IsPlaying(AnimationClip clip){
		return IsPlaying(GetAnimationData(clip));
	}
	
	public bool IsPlaying(AnimationClip clip, float weight){
		return IsPlaying(GetAnimationData(clip), weight);
	}
	
	public bool IsPlaying(MecanimAnimationData animData){
        return (currentAnimationData == animData);
	}
	
	public bool IsPlaying(MecanimAnimationData animData, float weight){
		if (animData == null) return false;
		if (currentAnimationData == null) return false;
		if (currentAnimationData == animData && animData.wrapMode == WrapMode.Once && animData.timesPlayed > 0) return false;
        if (currentAnimationData == animData && animData.wrapMode == WrapMode.Clamp && animData.timesPlayed > 0) return false;
		if (currentAnimationData == animData && animData.wrapMode == WrapMode.ClampForever) return true;
		if (currentAnimationData == animData) return true;

        AnimatorClipInfo[] animationInfoArray = animator.GetCurrentAnimatorClipInfo(0);
		foreach (AnimatorClipInfo animationInfo in animationInfoArray){
			if (animData.clip == animationInfo.clip && animationInfo.weight >= weight) return true;
		}
		return false;
	}
	
	public string GetCurrentClipName(){
		return currentAnimationData.clipName;
	}
	
	public int GetCurrentClipPlayCount(){
		return currentAnimationData.timesPlayed;
	}
	
	public float GetCurrentClipTime(){
		return currentAnimationData.secondsPlayed;
	}

	public float GetCurrentClipLength(){
		return currentAnimationData.length;
	}

    public Vector2 GetDeltaPosition() {
        return animator.deltaPosition;
    }

	public void SetCurrentClipPosition(float normalizedTime){
		SetCurrentClipPosition(normalizedTime, false);
	}

    public void SetCurrentClipPosition(float normalizedTime, bool pause) {
        normalizedTime = Mathf.Clamp01(normalizedTime);
        currentAnimationData.secondsPlayed = normalizedTime * currentAnimationData.length;
        currentAnimationData.normalizedTime = normalizedTime;

        animator.Play(currentAnimationData.stateName, 0, normalizedTime);
        animator.Update(0);

        if (pause) Pause();
    }

    public float GetCurrentClipPosition() {
        if (currentAnimationData == null) return 0;
		return currentAnimationData.secondsPlayed/currentAnimationData.length;
	}
	
	public void Stop(){
		Play(defaultAnimation.clip, defaultTransitionDuration, 0, currentMirror);
	}

    public void Pause() {
        SetSpeed(0);
	}

    public void SetSpeed(AnimationClip clip, float speed) {
        SetSpeed(GetAnimationData(clip), speed);
    }

    public void SetSpeed(string clipName, float speed) {
        SetSpeed(GetAnimationData(clipName), speed);
    }

    public void SetSpeed(MecanimAnimationData animData, float speed) {
        if (animData != null) {
            animData.normalizedSpeed = speed / animData.originalSpeed;

            animData.speed = speed;
            if (IsPlaying(animData)) SetSpeed(speed);
        }
    }

    public void SetSpeed(float speed) {
        //animator.SetFloat("Speed", Mathf.Abs(speed));
        animator.speed = Mathf.Abs(speed);
        currentSpeed = speed;
    }

    public void SetNormalizedSpeed(AnimationClip clip, float normalizedSpeed) {
        SetNormalizedSpeed(GetAnimationData(clip), normalizedSpeed);
    }

    public void SetNormalizedSpeed(string clipName, float normalizedSpeed) {
        SetNormalizedSpeed(GetAnimationData(clipName), normalizedSpeed);
    }

    public void SetNormalizedSpeed(MecanimAnimationData animData, float normalizedSpeed) {
        if (animData == null) return;
        animData.normalizedSpeed = normalizedSpeed;
        animData.speed = animData.originalSpeed * animData.normalizedSpeed;
        if (IsPlaying(animData)) SetSpeed(animData.speed);
    }
	
	public void RestoreSpeed(){
		SetSpeed(currentAnimationData.speed);
	}
	
	public void Rewind(){
		SetSpeed(-currentAnimationData.speed);
	}

	public void SetWrapMode(WrapMode wrapMode){
		defaultWrapMode = wrapMode;
	}
	
	public void SetWrapMode(MecanimAnimationData animationData, WrapMode wrapMode){
		animationData.wrapMode = wrapMode;
		animationData.clip.wrapMode = wrapMode;
	}

	public void SetWrapMode(AnimationClip clip, WrapMode wrapMode){
		MecanimAnimationData animData = GetAnimationData(clip);
		animData.wrapMode = wrapMode;
		animData.clip.wrapMode = wrapMode;
	}

	public void SetWrapMode(string clipName, WrapMode wrapMode){
		MecanimAnimationData animData = GetAnimationData(clipName);
		animData.wrapMode = wrapMode;
		animData.clip.wrapMode = wrapMode;
	}

    public float GetSpeed(AnimationClip clip) {
        return GetSpeed(GetAnimationData(clip));
	}

    public float GetSpeed(string clipName) {
        return GetSpeed(GetAnimationData(clipName));
	}

    public float GetSpeed(MecanimAnimationData animData) {
        return animData.speed;
    }

    public float GetSpeed() {
        //return animator.GetFloat("Speed");
		return animator.speed;
	}

    public float GetNormalizedSpeed(AnimationClip clip) {
        return GetNormalizedSpeed(GetAnimationData(clip));
    }

    public float GetNormalizedSpeed(string clipName) {
        return GetNormalizedSpeed(GetAnimationData(clipName));
    }

    public float GetNormalizedSpeed(MecanimAnimationData animData) {
        return animData.normalizedSpeed;
    }
	
	public bool GetMirror(){
		return currentMirror;
	}

	public void SetMirror(bool toggle){
		SetMirror(toggle, 0, false);
	}
	
	public void SetMirror(bool toggle, float blendingTime){
		SetMirror(toggle, blendingTime, false);
	}

	public void SetMirror(bool toggle, float blendingTime, bool forceMirror){
		if (currentMirror == toggle && !forceMirror) return;
		
		if (blendingTime == 0) blendingTime = defaultTransitionDuration;
		_playAnimation(currentAnimationData, blendingTime, GetCurrentClipPosition(), toggle);
	}
}
