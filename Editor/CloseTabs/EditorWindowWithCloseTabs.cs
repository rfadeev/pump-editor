using UnityEditor;

namespace PumpEditor
{
    public abstract class EditorWindowWithCloseTabs : EditorWindow, IHasCustomMenu
    {
        public virtual void AddItemsToMenu(GenericMenu menu)
        {
            CloseTabsHelper.AddCloseOtherTabsItem(this, menu);
        }
    }
}
