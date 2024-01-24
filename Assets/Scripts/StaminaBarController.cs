using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Aplicar em um GameObject do tipo Slider
//Remova o componente Handle Slide Area (remove também o Handle, já que é filho)

public class StaminaBarController : MonoBehaviour
{
    public Slider slider;
    [SerializeField] private float reducaoPorSegundo = 1f;
    [SerializeField] private float reducaoPorColisao = 10f;
    [SerializeField] private float aumentoPorOsso = 1f;
    [SerializeField] private float updateRate = 0.2f;
    [SerializeField] private Color fullStamina;
    [SerializeField] private Color halfStamina;
    [SerializeField] private Color noStamina;
    private Color cor;
    [SerializeField] private BoxCollider playerCollider; //Isto não se pode tirar?? -D.R.


    void Start()
    {
        //Identifica automaticamente o componente Slider conseguir utilizá-lo
        slider = GetComponent<Slider>();

        //Repete a cada intervalo definido a redução de energia
        InvokeRepeating("ReduzirEnergiaSequencial", 1f, updateRate);
    }



    void ReduzirEnergiaSequencial()
    {
        if (PlayerManager.isGameStarted)
        {
            slider.value -= reducaoPorSegundo;
            AtualizarCor();

        }
    }

    public void ReduzirEnergiaColisao()
    {
        if (PlayerManager.isGameStarted)
        {
            slider.value -= reducaoPorColisao;
            AtualizarCor();
        }
    }

    public void AumentarEnergiaOsso()
    {
        if (PlayerManager.isGameStarted)
        {
            slider.value += aumentoPorOsso;
            AtualizarCor();
        }
    }

    void AtualizarCor()
    {
        //Percentual de energia restante
        float percentualEnergia = slider.value / slider.maxValue;

        //Calcula a cor atual baseada o percentual de energia
        //Faz uma interpolação entre as cores e retorna a cor correspondente ao percentual de energia
        if (percentualEnergia >= 0.5f)
        {
            float a = (1.0f - ((percentualEnergia - 0.5f) / 0.5f));
            cor = Color.Lerp(fullStamina, halfStamina, a);
        }
        else
        {
            float b = (1.0f - (percentualEnergia / 0.5f));
            cor = Color.Lerp(halfStamina, noStamina, b);
        }

        //Aplica a cor ao slider
        slider.fillRect.GetComponent<Image>().color = cor;
    }
}