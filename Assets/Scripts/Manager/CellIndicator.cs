using UnityEngine;

namespace Manager
{
    public class CellIndicator : MonoBehaviour
    {
        [SerializeField] SpriteRenderer SpriteRenderer;
        [SerializeField] Color DefaultColor;

        public bool IsIndicatorActive()
        {
            return gameObject.activeSelf;
        }
        
        public void SetCellIndicatorActive(bool Active)
        {
            gameObject.SetActive(Active);    
        }
        
        public void SetCellIndicatorPosition(Vector3 NewPos)
        {
            transform.position = NewPos;
        }

        public void SetCellIndicatorColor(bool SetDefaultColor = false, Color NewColor = new Color())
        {
            if (SetDefaultColor)
            {
                SpriteRenderer.color = DefaultColor;
                return;
            }

            SpriteRenderer.color = NewColor;

        }
    }
}