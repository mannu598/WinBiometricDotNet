﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FrameworkTester.ViewModels.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using WinBiometricDotNet;

namespace FrameworkTester.ViewModels
{

    public sealed class WinBioAsyncMonitorFrameworkChangesViewModel : WinBioViewModel, IWinBioAsyncMonitorFrameworkChangesViewModel
    {

        #region Constructors

        public WinBioAsyncMonitorFrameworkChangesViewModel()
        {
            this.ChangeTypes = Enum.GetValues(typeof(ChangeTypes)).Cast<ChangeTypes>().ToArray();
            this.SelectedChangeType = this.ChangeTypes.First();

            WinBiometric.AsyncCompleted -= this.WinBiometricAsyncCompleted;
            WinBiometric.AsyncCompleted += this.WinBiometricAsyncCompleted;

            this.HandleRepository = SimpleIoc.Default.GetInstance<IHandleRepositoryViewModel<IFrameworkHandleViewModel>>();
            this.HandleRepository.PropertyChanged += (sender, args) =>
            {
                this.ExecuteCommand.RaiseCanExecuteChanged();
            };
        }

        #endregion

        #region Properties

        public RelayCommand CancelCommand
        {
            get;
        }

        public bool EnableWait
        {
            get;
            set;
        }

        private RelayCommand _ExecuteCommand;

        public override RelayCommand ExecuteCommand
        {
            get
            {
                return this._ExecuteCommand ?? (this._ExecuteCommand = new RelayCommand(() =>
                {
                    var name = this.Name;

                    try
                    {
                        this.Result = "WAIT";
                        this.UpdateUIImmediately();

                        var window = this.HandleRepository.SelectedHandle;
                        this.BiometricService.AsyncMonitorFrameworkChanges(window.Framework, this.SelectedChangeType);

                        this.Result = "OK";
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, name, MessageBoxButton.OK, MessageBoxImage.Error);
                        this.Result = "FAIL";
                    }
                }, () => this.HandleRepository?.SelectedHandle != null));
            }
        }

        public override string Name => "WinBioAsyncMonitorFrameworkChanges";
        
        public IHandleRepositoryViewModel<IFrameworkHandleViewModel> HandleRepository
        {
            get;
        }

        public IEnumerable<ChangeTypes> ChangeTypes
        {
            get;
        }

        private ChangeTypes _SelectedChangeType;

        public ChangeTypes SelectedChangeType
        {
            get => this._SelectedChangeType;
            set
            {
                this._SelectedChangeType = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        #region Event Handlers

        private void WinBiometricAsyncCompleted(object sender, AsyncCompletedEventArgs e)
        {
        }

        #endregion

        #endregion

    }

}