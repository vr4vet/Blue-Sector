using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Button1))]
public class RestartButton : MonoBehaviour
{
	public Spawner spawner;

	private Button1 button;

	void Start() {
		button = gameObject.GetComponent<Button1>();
	}

	void Update () {
		button.visible = spawner.CanInstantiate;
	}

	void OnClick() {
		spawner.Instantiate();
	}
}
