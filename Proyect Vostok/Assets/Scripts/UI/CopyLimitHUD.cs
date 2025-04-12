using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CopyLimitHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text copyLimitTMPText;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCopyCountChanged += UpdateHUD;
            UpdateHUD(GameManager.Instance.GetUsedCopies(), GameManager.Instance.getCopyLimit());
        }
    }
    private void UpdateHUD(int usadas, int limite)
    {
        copyLimitTMPText.text = $"Copias usadas: {usadas}/{limite}";
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCopyCountChanged -= UpdateHUD;
        }
    }


}
