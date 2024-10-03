using System.Collections;
using System.Collections.Generic;
using Blueprints;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.Utilities.Extension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserData.Controller;
using Zenject;

public class SelectStageView : MonoBehaviour
{
    public Button          btn;

    private LevelRecord levelRecord;
    private int         index;

    [Inject] private LevelManager levelManager;
    
    public void BindData(LevelRecord levelRecord, int index)
    {
        this.GetCurrentContainer().Inject(this);
        this.levelRecord = levelRecord;
        this.index       = index;
        
        btn.onClick.AddListener(() =>
        {
            btn.onClick.RemoveAllListeners();
            SelectLevel();
        });
    }

    void SelectLevel()
    {
        levelManager.SelectLevel(levelRecord, index).Forget();
    }
}
