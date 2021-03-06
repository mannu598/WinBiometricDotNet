﻿using FrameworkTester.ViewModels.Interfaces;
using WinBiometricDotNet;

namespace FrameworkTester.ViewModels
{

    public sealed class WinBioGetEnabledSettingViewModel : WinBioPropertyViewModel, IWinBioGetEnabledSettingViewModel
    {

        public override string Name => "WinBioGetEnabledSetting";

        protected override void GetValueAndSource(out bool value, out SettingSourceType source)
        {
            this.BiometricService.GetEnabledSetting(out value, out source);
        }

    }

}