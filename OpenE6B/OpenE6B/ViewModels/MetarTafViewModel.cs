﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OpenE6B.Annotations;
using OpenE6B.Classes;
using OpenE6B.Pages;

namespace OpenE6B.ViewModels
{
    public class MetarTafViewModel : INotifyPropertyChanged
    {
        private string _stationId;
        private string _rawMetarText;
        private bool _isButtonEnabled;

        public string StationId 
        {
            get { return _stationId; }
            set
            {
                if (value == _stationId) return;
                _stationId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsButtonEnabled));
            }
        }

        public string RawMetarText 
        {
            get { return _rawMetarText; }
            set
            {
                if (value == _rawMetarText) return;
                _rawMetarText = value;
                OnPropertyChanged();
            }
        }
        public Metar Metar { get; set; }
        public IAsyncCommand GetMetarCommand { get; set; }
        public ICommand MainMenuCommand { get; set; }

        public bool IsButtonEnabled
        {
            get
            {   
                return CanRetrieve();
            }
            set
            {
                if (value == _isButtonEnabled) return;
                _isButtonEnabled = value;
                OnPropertyChanged();
            }
        }


        public MetarTafViewModel()
        {
            var retriever = new MetarRetriever();
            GetMetarCommand = new AsyncCommand<Metar>(() => retriever.GetMetar(StationId));
            MainMenuCommand = new RelayCommand(GoToMainMenu);
        }

        public bool CanRetrieve()
        {
            return !string.IsNullOrWhiteSpace(StationId) && StationId.Length == 4;
        }

        public void GoToMainMenu(object param)
        {
            var frame = UIHelper.FindVisualParent<Frame>(param as DependencyObject);
            frame.Content = new MainMenu();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
