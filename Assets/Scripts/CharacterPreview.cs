using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterPreview : MonoBehaviour,ISelectHandler
{
    [SerializeField] private Image _preview;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Text _chTxt;
    [SerializeField] private string _name;
    public void OnSelect(BaseEventData eventData)
    {
        _preview.sprite = sprite;
        _chTxt.text = _name;
    }
}
