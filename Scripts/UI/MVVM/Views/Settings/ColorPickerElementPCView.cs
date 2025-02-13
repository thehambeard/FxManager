using FxManager.UI.MVVM.VMs.Settings;
using Owlcat.Runtime.UI.MVVM;
using Owlcat.Runtime.UI.VirtualListSystem.ElementSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

namespace FxManager.UI.MVVM.Views.Settings
{
    internal class ColorPickerElementPCView : VirtualListElementViewBase<ColorPickerElementVM>
    {
        public override VirtualListLayoutElementSettings LayoutSettings => _virtualListSettings;

        [SerializeField]
        private VirtualListLayoutElementSettings _virtualListSettings;

        [SerializeField]
        private Slider _redSlider;

        [SerializeField]
        private Slider _greenSlider;

        [SerializeField]
        private Slider _blueSlider;

        [SerializeField]
        private Slider _alphaSlider;

        [SerializeField]
        private Image _colorPreview;

        [SerializeField]
        private TextMeshProUGUI _settingName;

        public override void BindViewImplementation()
        {
            SetupSlider(_redSlider);
            SetupSlider(_greenSlider);
            SetupSlider(_blueSlider);
            SetupSlider(_alphaSlider);

            base.AddDisposable(base.ViewModel.Color.Subscribe(new Action<Color>(SetSlidersFromColor)));
            base.AddDisposable(_redSlider.onValueChanged.AsObservable().Subscribe(new Action<float>(SetPreviewColor)));
            base.AddDisposable(_greenSlider.onValueChanged.AsObservable().Subscribe(new Action<float>(SetPreviewColor)));
            base.AddDisposable(_blueSlider.onValueChanged.AsObservable().Subscribe(new Action<float>(SetPreviewColor)));
            base.AddDisposable(_alphaSlider.onValueChanged.AsObservable().Subscribe(new Action<float>(SetPreviewColor)));
            base.AddDisposable(base.ViewModel.SettingName.Subscribe(text => _settingName.text = text));
            SetPreviewColor();
        }

        private void SetupSlider(Slider slider)
        {
            slider.wholeNumbers = false;
            slider.minValue = 0f;
            slider.maxValue = 1f;
        }

        private void SetPreviewColor(float dummy) => SetPreviewColor();

        private void SetPreviewColor()
        {
            var color = new Color(_redSlider.value, _greenSlider.value, _blueSlider.value, _alphaSlider.value);
            _colorPreview.color = color;
            base.ViewModel.Color.Value = color;
        }

        private void SetSlidersFromColor(Color color)
        {
            _redSlider.value = color.r;
            _greenSlider.value = color.g;
            _blueSlider.value = color.b;
            _alphaSlider.value = color.a;
        }

        public override void DestroyViewImplementation()
        {
        }
    }
}
