using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class UpdateImage : MonoBehaviour
{
    public Image myImage;
    public Image images;
    private void Start()
    {
        images.ObserveEveryValueChanged(_image => _image.sprite).Subscribe(_sprite =>
        {
            myImage.sprite = _sprite;
        });
    }
}
