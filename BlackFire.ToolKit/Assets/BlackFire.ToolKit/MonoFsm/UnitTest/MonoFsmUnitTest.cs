using System.Collections;
using System.Collections.Generic;
using BlackFire.ToolKit;
using Unity.Collections;
using UnityEngine;

public class MonoFsmUnitTest : MonoBehaviour {

	public enum TestState //1.定义状态枚举。
	{
		Init,
		Move,
		Stop
	}

	private IMonoFsm<TestState> m_Fsm = null;
	
	private void Start()
	{
		m_Fsm = MonoFsm<TestState>.Init(this); //2.初始化状态机。
		m_Fsm.ChangeState(TestState.Init); //3.切换状态。
	}

	
	private IEnumerator Init_Enter()
	{
		Debug.Log("Init_Enter");
		yield return new WaitForSeconds(3f);
		Debug.Log("Init_Enter 3s later.");
	}

	private int m_Frame = 0;
	private void Init_Update()
	{
		Debug.Log("Init_Update");
		if (m_Frame++==600)
		{
			m_Fsm.ChangeState(TestState.Move);
		}
	}
	
	private void Init_FixedUpdate()
	{
		Debug.Log("Init_FixedUpdate");
	}
	
	private void Init_Exit()
	{
		Debug.Log("Init_Exit");
	}
	
	
	
	
	
	private void Move_Enter()
	{
		Debug.Log("Move_Enter");
	}

	private void Move_Update()
	{
		Debug.Log("Move_Update");
	}
	
	private void Move_FixedUpdate()
	{
		Debug.Log("Move_FixedUpdate");
	}
	
	private void Move_Exit()
	{
		Debug.Log("Move_Exit");
	}
	
	

}
