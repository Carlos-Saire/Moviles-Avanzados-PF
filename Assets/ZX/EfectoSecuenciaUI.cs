using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EfectSecuenciaUI : MonoBehaviour
{
    [Header("Imágenes")]
    [SerializeField] private List<Image> imagenes;

    [Header("Material")]
    [SerializeField] private Material materialBase;

    [Header("Duración")]
    [SerializeField] private float delayEntreImagenes;
    [SerializeField] private float velocidadDissolve;

    private List<Material> materialesClonados = new();

    private void Start()
    {
        // Crear copias del material
        foreach (var img in imagenes)
        {
            Material clone = new Material(materialBase);
            materialesClonados.Add(clone);
            img.material = clone;
        }
        ResetProgress();
        StartCoroutine(SecuenciaDissolve());
    }
    private void ResetProgress()
    {
        foreach (Material m in materialesClonados)
        {
            m.SetFloat("_Progress", 1f);  
        }
    }

    private IEnumerator SecuenciaDissolve()
    {
        for (int i = 0; i < imagenes.Count; i++)
        {
            yield return new WaitForSeconds(delayEntreImagenes);

            yield return StartCoroutine(DissolveImagen(i));

            imagenes[i].gameObject.SetActive(false);
        }
    }
    private IEnumerator DissolveImagen(int index)
    {
        Material mat = materialesClonados[index];
        float progress = 1f;

        while (progress > 0)
        {
            progress -= Time.deltaTime * velocidadDissolve;
            mat.SetFloat("_Progress", progress);
            yield return null;
        }
        mat.SetFloat("_Progress", 0f);
    }
}
