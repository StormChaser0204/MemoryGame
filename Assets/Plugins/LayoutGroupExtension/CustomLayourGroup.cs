using UnityEngine;
using UnityEngine.UI;

public class CustomLayourGroup : LayoutGroup
{
    [SerializeField] private VerticalLayoutGroup _group;
    [SerializeField] private GridLayoutGroup _testGrid;
    
    public override void CalculateLayoutInputVertical() => throw new System.NotImplementedException();

    public override void SetLayoutHorizontal() => throw new System.NotImplementedException();

    public override void SetLayoutVertical() => throw new System.NotImplementedException();
}
