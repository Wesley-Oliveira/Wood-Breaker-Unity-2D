using UnityEngine;
using System.Collections;

public class Plataforma : MonoBehaviour {

	public float velocidadeDeMovimento;
	public float limiteEmX;

	// Use this for initialization
	void Start () 
	{
		limiteEmX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - GetComponent<SpriteRenderer>().bounds.extents.x;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float direçãoHorizontalDoMouse = Input.GetAxis ("Mouse X"); // -1 = esquerdo; 0 = parado; 1 = direita
		GetComponent<Transform>().position += Vector3.right * direçãoHorizontalDoMouse * velocidadeDeMovimento * Time.deltaTime;
		float xAtual = transform.position.x;
		xAtual = Mathf.Clamp (xAtual, -limiteEmX, limiteEmX);
		transform.position = new Vector3 (xAtual, transform.position.y, transform.position.z);
	}
}
