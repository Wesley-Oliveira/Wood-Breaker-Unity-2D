using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GerenciadorDoGame : MonoBehaviour 
{
	public static int numeroTotalDeBlocos;
	public static int numeroDeBlocosDestruidos;
	public Image estrelas;
	public GameObject canvasGo;
	public static GerenciadorDoGame instancia;
	public Bola bola;
	public Plataforma plataforma;

	void Awake ()
	{
		instancia = this;
	}

	void Start ()
	{
		if (Application.loadedLevel == 1)
		{
			canvasGo.SetActive(false);
			numeroDeBlocosDestruidos = 0;
		}
	}

	public void FinalizarJogo ()
	{
		canvasGo.SetActive(true);
		estrelas.fillAmount = (float)numeroDeBlocosDestruidos / (float)numeroTotalDeBlocos;
		plataforma.enabled = false;
		Destroy(bola.gameObject);
	}

	public void AlterarCena (string cena)
	{
		Application.LoadLevel (cena);
	}

	public void FecharAplicativo ()
	{
		Application.Quit();
	}
}
