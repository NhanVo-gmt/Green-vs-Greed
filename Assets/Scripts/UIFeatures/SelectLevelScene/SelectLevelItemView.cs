using System.Collections.Generic;
using System.Linq;
using Blueprints;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.AssetLibrary;
using GameFoundation.Scripts.UIModule.MVP;
using GameFoundation.Scripts.Utilities.ObjectPool;
using GameFoundationBridge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserData.Controller;
using Zenject;

public class SelectLevelItemModel
{
    public readonly LevelRecord levelRecord;

    public SelectLevelItemModel(LevelRecord levelRecord)
    {
        this.levelRecord = levelRecord;
    }
}

public class SelectLevelItemView : TViewMono
{
    public TextMeshProUGUI title;
    public SelectStageView[] stageViews;
}


public class SelectLevelItemPresenter : BaseUIItemPresenter<SelectLevelItemView, SelectLevelItemModel>
{

    #region Inject

    private readonly ObjectPoolManager objectPoolManager;
    private readonly DiContainer       diContainer;

    #endregion

    private SelectLevelItemModel  model;
    
    public SelectLevelItemPresenter(IGameAssets gameAssets, ObjectPoolManager objectPoolManager, 
                                    DiContainer diContainer) : base(gameAssets)
    {
        this.objectPoolManager = objectPoolManager;
        this.diContainer       = diContainer;
    }
    
    public override void BindData(SelectLevelItemModel model)
    {
        this.model = model;
        
        InitializeStage();
        
        this.View.title.text = $"{model.levelRecord.Id}. {model.levelRecord.Name}";
    }

    void InitializeStage()
    {
        for (int i = 0; i < this.View.stageViews.Length; i++)
        {
            this.View.stageViews[i].BindData(model.levelRecord, i + 1);
        }
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}

