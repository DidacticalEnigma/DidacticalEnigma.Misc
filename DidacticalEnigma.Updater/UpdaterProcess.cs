using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using Utility.Utils;

namespace DidacticalEnigma.Updater
{
    public abstract class UpdaterProcess : INotifyPropertyChanged
    {
        private UpdateStatus currentStatus = new UpdateStatus.ReadyToStartStatus();

        public UpdateStatus CurrentStatus
        {
            get => currentStatus;
            set
            {
                currentStatus = value;
                OnPropertyChanged();
                OnUpdateStatusChange?.Invoke(currentStatus);
                updateCommand.OnExecuteChanged();
            }
        }
        
        public string Name { get; }

        private readonly RelayCommand updateCommand;
        public ICommand UpdateCommand => updateCommand;

        public event Action<UpdateStatus> OnUpdateStatusChange;

        protected abstract Task Start();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Execute()
        {
            if (updateCommand.CanExecute(null))
            {
                await Start();
            }
        }

        protected UpdaterProcess(string name)
        {
            Name = name;
            updateCommand = new RelayCommand(async () =>
            {
                await Execute();
            },
            () =>
            {
                switch (currentStatus)
                {
                    case UpdateStatus.DownloadingStatus downloadingStatus:
                    case UpdateStatus.ProcessingStatus processingStatus:
                        return false;
                    case UpdateStatus.FailureStatus failureStatus:
                    case UpdateStatus.ReadyToStartStatus readyToStartStatus:
                    case UpdateStatus.SuccessStatus successStatus:
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(currentStatus));
                }
            });
        }
    }
}