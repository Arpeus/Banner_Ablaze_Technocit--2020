using UnityEngine.EventSystems;

public class EventHoverButton : EventTrigger
{
    public int index;
    private HUDMenu m_hudMenu;

    public void Start()
    {
        m_hudMenu = FindObjectOfType<HUDMenu>();
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        m_hudMenu.DisplayInfoTroop(index);
    }
}
