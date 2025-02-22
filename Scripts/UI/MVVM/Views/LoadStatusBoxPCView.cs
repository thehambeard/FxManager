using FxManager.UI.MVVM.VMs;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UniRx;
using FxManager.Cache;
using UnityEngine.UI;
using Owlcat.Runtime.UI.Controls.Button;
using Owlcat.Runtime.UI.Controls.Other;
using FxManager.Utility.Extensions.StringExtentsions;


namespace FxManager.UI.MVVM.Views
{
    internal class LoadStatusBoxPCView : ViewBase<LoadStatusBoxVM>
    {
        [SerializeField]
        TextMeshProUGUI _loadingStatusText;

        [SerializeField]
        TextMeshProUGUI _loadingProgressText;

        [SerializeField]
        Button _okButton;

        public override void BindViewImplementation()
        {
            gameObject.SetActive(true);
            base.AddDisposable(BlueprintCache.Instance.Progress.Subscribe(OnProgessChange));
            base.AddDisposable(BlueprintCache.Instance.Status.Subscribe(OnStatusChange));
            base.AddDisposable(_okButton.OnClickAsObservable().Subscribe(OnOkButtonClick));
            _okButton.gameObject.SetActive(false);
        }

        private void OnOkButtonClick(Unit unit)
        {
            gameObject.SetActive(false);
        }

        public void OnStatusChange(BlueprintLoadingStatus status)
        {
            if (status == BlueprintLoadingStatus.Complete) 
                gameObject.SetActive(false);

            if (status == BlueprintLoadingStatus.Error)
                _okButton.gameObject.SetActive(true);

            _loadingStatusText.text = Enum.GetName(typeof(BlueprintLoadingStatus), status).ConvertCamelCaseToWords();
        }

        private void OnProgessChange(float value)
        {
            if (value < 0f)
                _loadingProgressText.gameObject.SetActive(false);
            else if (!_loadingProgressText.gameObject.activeSelf)
                _loadingProgressText.gameObject.SetActive(true);

            _loadingProgressText.text = value.ToString("P1");
        }

        public override void DestroyViewImplementation()
        {
            gameObject.SetActive(false);
        }
    }
}
