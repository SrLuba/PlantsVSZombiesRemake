using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunCounter : MonoBehaviour
{
    public bool auto;
    public int number;
    public List<DigitSlot> numbers;

    public Image bg;

    public Color off, on;

   
    // Update is called once per frame
    void Update()
    {
        if (auto) {  
            number = GameManager.instance.sunCount;
        }

        char[] chars = number.ToString().PadRight(4, 'X').ToCharArray();


        bg.color = Color.Lerp(bg.color, (number.ToString().Length == 1) ? off : on, 5f * Time.deltaTime);

        if (number.ToString().Length == 1)

        {
            numbers[0].show = false;
            numbers[1].show = false;
            numbers[2].show = false;
            numbers[3].show = false;
        }
        else if (number.ToString().Length == 2)
        {
            numbers[0].show = false;
            numbers[1].show = true;
            numbers[2].show = true;
            numbers[3].show = false;


            numbers[1].number = int.Parse(chars[0].ToString());
            numbers[2].number = int.Parse(chars[1].ToString());
        }
        else if (number.ToString().Length == 3)
        {
            numbers[0].show = false;
            numbers[1].show = true;
            numbers[2].show = true;
            numbers[3].show = true;


            numbers[1].number = int.Parse(chars[0].ToString());
            numbers[2].number = int.Parse(chars[1].ToString());
            numbers[3].number = int.Parse(chars[2].ToString());
        }
        else if (number.ToString().Length == 4)
        {
            numbers[0].show = true;
            numbers[1].show = true;
            numbers[2].show = true;
            numbers[3].show = true;


            numbers[0].number = int.Parse(chars[0].ToString());
            numbers[1].number = int.Parse(chars[1].ToString());
            numbers[2].number = int.Parse(chars[2].ToString());
            numbers[3].number = int.Parse(chars[3].ToString());

        }
    }
}
