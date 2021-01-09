﻿// Copyright (c) Brice Lambson
// The Brice Lambson licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.  Code forked from Brice Lambson's https://github.com/bricelam/ImageResizer/

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ImageResizer.Models;
using ImageResizer.Views;

namespace ImageResizer.ViewModels
{
    public class ProgressViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ResizeBatch _batch;
        private readonly IMainView _mainView;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private double _progress;
        private TimeSpan _timeRemaining;

        public ProgressViewModel(
            ResizeBatch batch,
            MainViewModel mainViewModel,
            IMainView mainView)
        {
            _batch = batch;
            _mainViewModel = mainViewModel;
            _mainView = mainView;

            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
        }

        public double Progress
        {
            get => _progress;
            set => Set(nameof(Progress), ref _progress, value);
        }

        public TimeSpan TimeRemaining
        {
            get => _timeRemaining;
            set => Set(nameof(TimeRemaining), ref _timeRemaining, value);
        }

        public ICommand StartCommand { get; }

        public ICommand StopCommand { get; }

        public void Start()
            => Task.Factory.StartNew(
                () =>
                {
                    _stopwatch.Restart();
                    var errors = _batch.Process(
                        _cancellationTokenSource.Token,
                        (completed, total) =>
                        {
                            var progress = completed / total;
                            Progress = progress;
                            _mainViewModel.Progress = progress;

                            TimeRemaining = _stopwatch.Elapsed.Multiply((total - completed) / completed);
                        });

                    if (errors.Any())
                    {
                        _mainViewModel.Progress = 0;
                        _mainViewModel.CurrentPage = new ResultsViewModel(_mainView, errors);
                    }
                    else
                    {
                        _mainView.Close();
                    }
                },
                _cancellationTokenSource.Token);

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _mainView.Close();
        }
    }
}
