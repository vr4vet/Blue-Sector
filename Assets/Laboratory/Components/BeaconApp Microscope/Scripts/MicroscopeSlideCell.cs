using UnityEngine;
using UnityEngine.UI;

public class MicroscopeSlideCell : MonoBehaviour
{
    [SerializeField] private Image Image;
    [SerializeField] private int AmountOfPlankton;

    public int GetAmountOfPlankton()
    {
        return AmountOfPlankton;
    }

    public Image GetImage()
    {
        return Image; 
    }
}
