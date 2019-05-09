using UnityEngine;
using ControlFreak2;


public class TouchController : MonoBehaviour 
	{
	// ----------------
	public float GetAxis		(string name)	{	return CF2Input.GetAxis(name); }
	public float GetAxisRaw	(string name)	{	return CF2Input.GetAxisRaw(name); }
	public bool GetButton	(string name)	{	return CF2Input.GetButton(name); }
	}
