﻿using System;
using System.Windows;
using FrameworkTester.Services.Interfaces;
using FrameworkTester.ViewModels.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using WinBiometricDotNet;

namespace FrameworkTester.ViewModels
{

    public sealed class WinBioEnrollCommitViewModel : WinBioViewModel, IWinBioEnrollCommitViewModel
    {

        #region Fields

        private readonly IWinBiometricService _Service;

        #endregion

        #region Constructors

        public WinBioEnrollCommitViewModel()
        {
            this._Service = SimpleIoc.Default.GetInstance<IWinBiometricService>();
        }

        #endregion

        #region Properties

        private RelayCommand _ExecuteCommand;

        public override RelayCommand ExecuteCommand
        {
            get
            {
                return this._ExecuteCommand ?? (this._ExecuteCommand = new RelayCommand(() =>
                {
                    this.Type = IdentityTypes.Null;
                    this.TemplateGuid = Guid.Empty;
                    this.Sid = "";

                    try
                    {
                        this.Result = "WAIT";
                        var result = this._Service.CommitEnroll();
                        this.Result = "OK";

                        this.Type = result.Type;
                        this.TemplateGuid = result.TemplateGuid;
                        this.Sid = result.Sid;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        this.Result = "FAIL";
                    }
                }));
            }
        }

        public override string Name => "WinBioEnrollCommit";

        private string _Sid;

        public string Sid
        {
            get
            {
                return this._Sid;
            }
            private set
            {
                this._Sid = value;
                this.RaisePropertyChanged();
            }
        }

        private Guid _TemplateGuid;

        public Guid TemplateGuid
        {
            get
            {
                return this._TemplateGuid;
            }
            private set
            {
                this._TemplateGuid = value;
                this.RaisePropertyChanged();
            }
        }

        private IdentityTypes _Type;

        public IdentityTypes Type
        {
            get
            {
                return this._Type;
            }
            private set
            {
                this._Type = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }

}