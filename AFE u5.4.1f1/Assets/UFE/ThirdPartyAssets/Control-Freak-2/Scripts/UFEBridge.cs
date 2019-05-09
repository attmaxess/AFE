using System;
using UnityEngine;

namespace ControlFreak2
{
public class UFEBridge : InputTouchControllerBridge 
	{
	private InputRig 
		rig;



	// -----------------
	override public void Init()
		{
		this.rig = this.GetComponentInChildren<InputRig>();


#if UNITY_EDITOR
		if (this.rig == null)
			{
			Debug.LogError("UFE Bridge [" + this.name + "] is missing an Input Rig!!");
			}
#endif		
	
		CF2Input.activeRig = this.rig;
		}

	// --------------
	public override float GetAxis(string axisName)
		{ return CF2Input.GetAxis(axisName);  }

	// --------------
	public override float GetAxisRaw(string axisName)
		{ return CF2Input.GetAxisRaw(axisName);  }

	// --------------
	public override bool GetButton(string axisName)
		{ return CF2Input.GetButton(axisName);  }


	// --------------------
	public override void ShowBattleControls(bool visible, bool animate)
		{
		if (this.rig == null)
			return;

		if (visible && (CF2Input.activeRig != this.rig))
			CF2Input.activeRig = this.rig;

		if (visible == !this.rig.AreTouchControlsHiddenManually())
			return;

		this.rig.ShowOrHideTouchControls(visible, !animate);
		}
	}
}
