﻿using System;
using System.Windows;
using FrameworkTester.ViewModels.Interfaces;
using GalaSoft.MvvmLight.Command;
using WinBiometricDotNet;

namespace FrameworkTester.ViewModels
{

    public sealed class WinBioEnrollCommitViewModel : WinBioSessionViewModel, IWinBioEnrollCommitViewModel
    {

        #region Properties

        private RelayCommand _ExecuteCommand;

        public override RelayCommand ExecuteCommand
        {
            get
            {
                return this._ExecuteCommand ?? (this._ExecuteCommand = new RelayCommand(() =>
                {
                    this.Type = IdentityType.Null;
                    this.TemplateGuid = Guid.Empty;
                    this.Sid = "";

                    try
                    {
                        this.Result = "WAIT";
                        this.UpdateUIImmediately();

                        var session = this.HandleRepository.SelectedHandle.Session;
                        var result = this.BiometricService.CommitEnroll(session);

                        this.Result = "OK";

                        this.Type = result.Type;
                        this.TemplateGuid = result.TemplateGuid;
                        this.Sid = result.Sid?.Value;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, this.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                        this.Result = "FAIL";
                    }
                }, () => this.HandleRepository?.SelectedHandle != null));
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

        private IdentityType _Type;

        public IdentityType Type
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