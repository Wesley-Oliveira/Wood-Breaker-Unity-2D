using UnityEngine;
using System.Collections;

public class Bola : MonoBehaviour 
{
	public Vector3 Direção;
	public float Velocidade;
	public GameObject particulaBlocos;
	public ParticleSystem particulaFolhas;
    public LineRenderer guia;
    public int pontosDaGuia = 3;

	// Use this for initialization
	void Start () 
	{
		Direção.Normalize(); // equivalente a fazer Direção = Direção.normalized;
        guia.SetVertexCount(pontosDaGuia);
    }
	
	// Update is called once per frame
	void Update () 
	{
		transform.position += Direção * Velocidade * Time.deltaTime;
        AtualizarLineRenderer();
	}

    void AtualizarLineRenderer()
    {
        int pontoAtual = 1;
        Vector3 direçãoAtual = Direção;
        Vector3 ultimaPosição = transform.position;
        guia.SetPosition(0, ultimaPosição);
        while(pontoAtual < pontosDaGuia)
        {
            RaycastHit2D hit = Physics2D.Raycast(ultimaPosição, direçãoAtual);
            ultimaPosição = hit.point;
            guia.SetPosition(pontoAtual, ultimaPosição);
            direçãoAtual = Vector3.Reflect(direçãoAtual, hit.normal);
            ultimaPosição += direçãoAtual * 0.05f;
            pontoAtual++;
        }
    }

	// Função chamada quando esse elemento colidir com outro. O outro elemento, junto com informações da colisão, são encontrados no parâmetro 'colisor'
	void OnCollisionEnter2D (Collision2D colisor)
	{
		bool colisãoInvalida = false;
		Vector2 normal = Vector2.zero;
		foreach (ContactPoint2D c in colisor.contacts)
		{
			normal += c.normal;
		}
		normal /= colisor.contacts.Length;
		normal.Normalize ();
		Plataforma plataforma = colisor.transform.GetComponent<Plataforma>();
		GeradorDeArestas geradorDeArestas = colisor.transform.GetComponent<GeradorDeArestas>();
		if (plataforma != null) // Se existir o componente Plataforma no game object com o qual a bolinha colidiu...
		{
			if (normal != Vector2.up)
			{
				colisãoInvalida = true;		
			}
			else
			{
				particulaFolhas.transform.position = plataforma.transform.position;
				particulaFolhas.Play();
			}
		}
		else if (geradorDeArestas != null)
		{
			if (normal == Vector2.up)
			{
				colisãoInvalida = true;
			}
		}
		else // Caso cairmos no else, estamos colidindo com um bloco, pois é o que sobra
		{
			colisãoInvalida = false;
			Bounds bordasColisor = colisor.transform.GetComponent<SpriteRenderer>().bounds;
			Vector3 posiçãoDeCriação = new Vector3 (colisor.transform.position.x + bordasColisor.extents.x, colisor.transform.position.y - bordasColisor.extents.y, colisor.transform.position.z);
			GameObject particulas = (GameObject)Instantiate(particulaBlocos, posiçãoDeCriação, Quaternion.identity);
			ParticleSystem componenteParticulas = particulas.GetComponent<ParticleSystem>();
			Destroy (particulas, componenteParticulas.duration + componenteParticulas.startLifetime);
			Destroy (colisor.gameObject);
			GerenciadorDoGame.numeroDeBlocosDestruidos++;
		}
		if (!colisãoInvalida)
		{
			Direção = Vector2.Reflect (Direção, normal);
			Direção.Normalize();
		}
		else
		{
			GerenciadorDoGame.instancia.FinalizarJogo();
		}
	}
}
