using UnityEngine;
using UnityEngine.UIElements;

public class BG3TacticalGroundController : MonoBehaviour, ITacticalGroundStrategy 
{
    [SerializeField] private GroundIndicator m_charMovementPreviewIndicator;
    [SerializeField] private GroundIndicator m_charMovementConfirmationIndicator;
    [SerializeField] private LayerMask m_TacticalGridLayer;

    public void IndicateCharacterGroundLocation(Ray screenPointToRay, ITacticalGroundStrategy.IndicationType indicationType)
    {
        //TODO : iterate on a list of all the indicators to hide them
        m_charMovementPreviewIndicator.Show(false);
        m_charMovementConfirmationIndicator.Show(false);

        GroundIndicator showedIndicator;
        switch (indicationType)
        {
            case ITacticalGroundStrategy.IndicationType.SimpleSelection:
            case ITacticalGroundStrategy.IndicationType.MovementPreview:
                showedIndicator = m_charMovementPreviewIndicator;
                break;
            case ITacticalGroundStrategy.IndicationType.MovementConfirmation:
                showedIndicator = m_charMovementConfirmationIndicator;
                break;
            default: 
                showedIndicator = null;
                break;
        }

        if (Physics.Raycast(screenPointToRay, out RaycastHit hitInfo, 1000f, m_TacticalGridLayer))
        {
            showedIndicator?.Show(true);
            showedIndicator?.SetIndicatorPosition(hitInfo.point);
        }
    }
}
