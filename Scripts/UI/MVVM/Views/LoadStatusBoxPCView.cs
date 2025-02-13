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
using FxManager.Fx;
using UnityEngine.UI;
using Owlcat.Runtime.UI.Controls.Button;
using Owlcat.Runtime.UI.Controls.Other;


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
            base.AddDisposable(FxCache.Instance.Progress.Subscribe(OnProgessChange));
            base.AddDisposable(FxCache.Instance.Status.Subscribe(OnStatusChange));
            base.AddDisposable(_okButton.OnClickAsObservable().Subscribe(OnOkButtonClick));
            _okButton.gameObject.SetActive(false);
        }

        private void OnOkButtonClick(Unit unit)
        {
            gameObject.SetActive(false);
        }

        private void OnStatusChange(FxCollectionStatus status)
        {
            if (status == FxCollectionStatus.Complete) 
                gameObject.SetActive(false);

            if (status == FxCollectionStatus.Error)
                _okButton.gameObject.SetActive(true);

            _loadingStatusText.text = Enum.GetName(typeof(FxCollectionStatus), status);
        }

        private void OnProgessChange(float value)
        {
            _loadingProgressText.text = value.ToString("P1");
        }

        public override void DestroyViewImplementation()
        {
            gameObject.SetActive(false);
        }
    }
}
