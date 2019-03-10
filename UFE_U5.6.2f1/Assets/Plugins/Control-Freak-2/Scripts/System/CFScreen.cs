// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2016 Dan's Game Tools
// http://DansGameTools.com
// -------------------------------------------


using UnityEngine;



namespace ControlFreak2
{

// -----------------
//! Unity's Screen class replacement.
// -----------------

public static class CFScreen
	{
		
	const float DEFAULT_FALLBACK_SCREEN_DIAMETER = 7.0f; // 7 inches	

	// --------------------

	static private bool
		mForceFallbackDpi	= false;

	static private float 
		mFallbackDpi		= 96,
		mFallbackDiameter	= DEFAULT_FALLBACK_SCREEN_DIAMETER,

		mLastScreenWidth = -1,
		mLastScreenHeight = -1,
		mLastScreenDpi	= -1,

		mDpi		= 100,
		mDpcm		= 100,
		mInvDpi		= 1,
		mInvDpcm	= 1;
		
		
		

	// ----------------
	static public float dpi		{ get { UpdateDpiIfNeeded(); return mDpi; } }
	static public float dpcm	{ get { UpdateDpiIfNeeded(); return mDpcm; } }
	static public float invDpi	{ get { UpdateDpiIfNeeded(); return mInvDpi; } }
	static public float invDpcm{ get { UpdateDpiIfNeeded(); return mInvDpcm; } }
	static public float width	{ get { return Screen.width; } }
	static public float height	{ get { return Screen.height; } }
	
	// ---------------------
	//! Lock cursor. Use \ref CFCursor.lockState instead. 
	// ---------------------
	static public bool	lockCursor	
		{
		get { return (CFCursor.lockState == CursorLockMode.Locked); } 
		set { CFCursor.lockState = (value ? CursorLockMode.Locked : CursorLockMode.None); } 
		}

	// ---------------------
	//! Show cursor. Use \ref CFCursor.visible instead. 
	// ---------------------
	static public bool	showCursor	
		{
		get { return CFCursor.visible; } 
		set { CFCursor.visible = value; }
		}
		


	// ------------------------
	static public void ForceFallbackDpi(bool enableFallbackDpi)
		{
		mForceFallbackDpi = enableFallbackDpi;
		UpdateDpi();
		}

	// -----------------------
	static public void SetFallbackScreenDiameter(float diameterInInches)
		{
		mLastScreenWidth	= -1;
		mFallbackDiameter	= Mathf.Max(1.0f, diameterInInches);

		UpdateDpi();
		}


	// ------------------------
	static private void UpdateDpiIfNeeded()
		{
		if ((mLastScreenWidth != Screen.width) || (mLastScreenHeight != Screen.height) || (mLastScreenDpi != Screen.dpi))
			UpdateDpi();
		}


	// ----------------------
	static private void UpdateDpi()
		{
		mLastScreenWidth	= Screen.width;
		mLastScreenHeight	= Screen.height;
		mLastScreenDpi		= Screen.dpi;
	
		mFallbackDpi		= Mathf.Sqrt((mLastScreenWidth * mLastScreenWidth) + (mLastScreenHeight * mLastScreenHeight)) / mFallbackDiameter;
		
#if UNITY_EDITOR || UNITY_WEBPLAYER	 
		if (mForceFallbackDpi)
			mDpi = mFallbackDpi;
		else
			mDpi = mFallbackDpi;
#else
		if ((mLastScreenDpi < 1.0f) || mForceFallbackDpi)
			{
			mDpi = mFallbackDpi;
			}
		else
			{
			mDpi = mLastScreenDpi;
			}
#endif

		mDpcm		= (mDpi / 2.54f);
		mInvDpi		= (1.0f / mDpi);
		mInvDpcm	= (1.0f / mDpcm);
		}



	}
}
