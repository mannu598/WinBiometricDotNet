﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FrameworkTester.ViewModels.Interfaces;
using GalaSoft.MvvmLight.Command;
using WinBiometricDotNet;

namespace FrameworkTester.ViewModels
{

    public sealed class WinBioVerifyViewModel : WinBioSessionViewModel, IWinBioVerifyViewModel
    {

        #region Constructors

        public WinBioVerifyViewModel()
        {
            this._FingerPositions = Enum.GetValues(typeof(FingerPosition)).Cast<FingerPosition>().ToArray();
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
                    this.IsMatch = false;
                    this.UnitId = 0;
                    this.RejectDetail = 0;

                    try
                    {
                        this.Result = "WAIT";
                        this.UpdateUIImmediately();

                        var session = this.HandleRepository.SelectedHandle.Session;
                        var result = this.BiometricService.Verify(session, this.SelectedFingerPosition);

                        this.Result = "OK";

                        this.IsMatch = result.IsMatch;
                        this.UnitId = result.UnitId;
                        this.RejectDetail = result.RejectDetail;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, this.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                        this.Result = "FAIL";
                    }
                }, () => this.HandleRepository?.SelectedHandle != null));
            }
        }

        private bool _IsMatch;

        public bool IsMatch
        {
            get
            {
                return this._IsMatch;
            }
            private set
            {
                this._IsMatch = value;
                this.RaisePropertyChanged();
            }
        }

        public override string Name => "WinBioVerify";

        private RejectDetail _RejectDetail;

        public RejectDetail RejectDetail
        {
            get
            {
                return this._RejectDetail;
            }
            private set
            {
                this._RejectDetail = value;
                this.RaisePropertyChanged();
            }
        }

        private uint _UnitId;

        public uint UnitId
        {
            get
            {
                return this._UnitId;
            }
            private set
            {
                this._UnitId = value;
                this.RaisePropertyChanged();
            }
        }

        private FingerPosition _SelectedFingerPosition;

        public FingerPosition SelectedFingerPosition
        {
            get
            {
                return this._SelectedFingerPosition;
            }
            set
            {
                this._SelectedFingerPosition = value;
                this.RaisePropertyChanged();
            }
        }

        private IEnumerable<FingerPosition> _FingerPositions;

        public IEnumerable<FingerPosition> FingerPositions
        {
            get
            {
                return this._FingerPositions;
            }
            private set
            {
                this._FingerPositions = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }

}