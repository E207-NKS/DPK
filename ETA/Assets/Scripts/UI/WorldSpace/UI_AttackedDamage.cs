using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AttackedDamage : UI_Base
{

    public int AttackedDamage { get; set; }
    public bool IsGurared;
    public bool IsEvasion;
    public float _startTime;
    public float _duration = 0.75f;
    enum Texts
    {
        AttackedDamageText
    }

    Stat _stat;

    public override void Init()
    {
        //Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        string showedText = $"{AttackedDamage}";
        if(IsGurared)
            showedText = "SHIELD";
        else if(IsEvasion)
            showedText = "Miss";
        Get<TextMeshProUGUI>((int)Texts.AttackedDamageText).text = showedText;
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * ((parent.GetComponent<Collider>().bounds.size.y) + 0.3f);
        _startTime = Time.time;
        Destroy(gameObject, _duration);

    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        if (Time.time - _startTime < _duration * 0.3 / 3)
        {
            transform.position += Vector3.up * Time.deltaTime * 9.0f;
        }
        else if(Time.time - _startTime < _duration * 0.6 / 3)
        {
            transform.position -= Vector3.up * Time.deltaTime * 3.0f;
        }
        else
        {
            transform.localScale *= 1.001f;
            TextMeshProUGUI textMesh = Get<TextMeshProUGUI>((int)Texts.AttackedDamageText);
            SetTransparency(textMesh, 1 - (Time.time - _startTime));
        }
        

        



    }

    public void SetTransparency(TextMeshProUGUI textMesh, float alpha)
    {
        if (textMesh != null)
        {
            Color color = textMesh.color;
            color.a = alpha; // 알파 값을 조정
            textMesh.color = color; // 변경된 색상을 다시 할당
        }
    }



}