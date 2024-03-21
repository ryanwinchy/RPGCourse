using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour              //Added at end, parent script for all tooltips for shared functionality. Makes simpler.
{
    [SerializeField] float xMouseLimit = 960;  //Where tooltip changes pos on x. These values default for 1920 x 1080. But can change if has screen size setting for eg.
    [SerializeField] float yMouseLimit = 540;  //Where tooltip changes pos on y.

    [SerializeField] float xOffset = 150;     //How much the tooltip is moved.
    [SerializeField] float yOffset = 150;


    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newXOffset = 0;
        float newYOffset = 0;

        if (mousePosition.x > xMouseLimit)                //Nicely moves tooltip towards where the skill is.
            newXOffset = -xOffset;
        else
            newXOffset = xOffset;

        if (mousePosition.y > yMouseLimit)                //Nicely moves tooltip towards where the skill is.
            newYOffset = -yOffset;
        else
            newYOffset = yOffset;

        transform.position = new Vector2(mousePosition.x + newXOffset, mousePosition.y + newYOffset);
    }


    public void AdjustFontSize(TextMeshProUGUI _text)    //May not need, unity has auto resize text option now.
    {
        if (_text.text.Length > 12)
            _text.fontSize *= 0.8f;
    }







}
