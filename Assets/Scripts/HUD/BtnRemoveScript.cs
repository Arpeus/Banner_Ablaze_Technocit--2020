using UnityEngine;

public class BtnRemoveScript : MonoBehaviour
{
    private HUDMenu m_HUDMenu;
    public int indexBtn;
    // Start is called before the first frame update
    void Awake()
    {
        m_HUDMenu = FindObjectOfType<HUDMenu>();
    }

    public void Remove(int index)
    {
        m_HUDMenu.RemoveCharacter(index);
    }

    public void Destroy()
    {
         Destroy(gameObject);
    }
}
