using FxManager.UI.MVVM.VMs;
using Kingmaker.UI.MVVM._VM.Utility;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace FxManager.UI.MVVM.Views
{
    internal class FxOpenCloseWindowPCView : ViewBase<FxOpenCloseVM>
    {
        [SerializeField]
        Button _button;

        [SerializeField]
        Image _buttonImage;

        [SerializeField]
        Sprite _selectedSprite;

        [SerializeField]
        Sprite _defaultSprite;

        [SerializeField]
        List<GameObject> _controlledObjects = new List<GameObject>();

        private bool _isSelected = false;

        public override void BindViewImplementation()
        {
            base.AddDisposable(_button.OnClickAsObservable().Subscribe(ToggleState));
            gameObject.SetActive(true);
        }

        private void ToggleState(Unit unit)
        {
            _isSelected = !_isSelected;

            if (_isSelected)
            {
                _buttonImage.sprite = _selectedSprite;
                _controlledObjects.ForEach(x => x.SetActive(true));
            }
            else
            {
                _buttonImage.sprite = _defaultSprite;
                _controlledObjects.ForEach(x => x.SetActive(false));
            }
        }

        public override void DestroyViewImplementation()
        {
        }

        
    }
}
