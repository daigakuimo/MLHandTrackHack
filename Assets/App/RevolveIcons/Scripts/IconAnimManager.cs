using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IconAnimManager : MonoBehaviour
{
    [SerializeField]
    private float defaultIconScale = 0.6f;
    
    [SerializeField]
    private float selectedIconScale = 1.5f;
    
    
    private List<IconBehaviour> _icons = new List<IconBehaviour>();

    private int iconNum;
    private int selectedIconIndex;

    private void OnEnable()
    {
        _icons = GetComponentsInChildren<IconBehaviour>().ToList();

        iconNum = _icons.Count;
        selectedIconIndex = 0;
        
            
        foreach (var (icon, i) in _icons.Select((x, i) => (x, i)))
        {
            ChangeSize(icon, i == selectedIconIndex);
        }
    }
    
    private void ChangeSize(IconBehaviour target, bool isBig)
    {
        if (isBig)
        {
            target.transform.localScale = Vector3.one * selectedIconScale;
        }
        else
        {
            target.transform.localScale = Vector3.one * defaultIconScale;
        }
    }

    public void OnFist()
    {
        _icons[selectedIconIndex].OnFist();
    }

    public void OnRoleIcons()
    {
        ChangeSize(_icons[selectedIconIndex], false);
        selectedIconIndex++;
        if (selectedIconIndex >= iconNum)
        {
            selectedIconIndex = 0;
        }
        
        ChangeSize(_icons[selectedIconIndex], true);
    }
}
