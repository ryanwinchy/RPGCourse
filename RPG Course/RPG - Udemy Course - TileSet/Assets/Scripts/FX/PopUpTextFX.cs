using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpTextFX : MonoBehaviour
{
    TextMeshPro myText;

    [SerializeField] float speed;
    [SerializeField] float disappearingSpeed;
    [SerializeField] float colourDisappearingSpeed;

    [SerializeField] float lifetime;

    float textTimer;


    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<TextMeshPro>();
        textTimer = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);
        textTimer -= Time.deltaTime;

        if (textTimer < 0)
        {
            float alpha = myText.color.a - colourDisappearingSpeed * Time.deltaTime;
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

            if (myText.color.a < 50)
                speed = disappearingSpeed;    //Update runs every frame, so this will change var value then first line of this function will change value of speed.

            if (myText.color.a <= 0)
                Destroy(gameObject);

        }
    }
}
