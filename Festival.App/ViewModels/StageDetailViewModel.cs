using System;
using System.Windows.Input;
using Festival.BL.Models;
using Festival.BL.Repositories;
using Festival.App.Commands;
using Festival.App.Messages;
using Festival.App.Services;

namespace Festival.App.ViewModels
{
    public class StageDetailViewModel : ViewModelBase
    {
        private readonly StageRepository _stageRepository;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMediator _mediator;

        public StageDetailModel? Model { get; set; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }

        public StageDetailViewModel(
            StageRepository stageRepository,
            IMessageDialogService messageDialogService,
            IMediator mediator
            )
        {
            _stageRepository = stageRepository;
            _messageDialogService = messageDialogService;
            _mediator = mediator;

            SaveCommand = new RelayCommand(Save, CanSave);
            DeleteCommand = new RelayCommand(Delete, CanDelete);

            _mediator.Register<SelectedMessage<StageDetailModel>>(StageSelected);
            _mediator.Register<NewMessage<StageDetailModel>>(StageNew);
        }

        private void StageSelected(SelectedMessage<StageDetailModel> message)
            => Model = _stageRepository.GetById(message.Id);

        private void StageNew(NewMessage<StageDetailModel> message)
            => Model = new StageDetailModel();

        private bool CanSave() =>
            Model != null
            && !string.IsNullOrWhiteSpace(Model.Name);

        private void Save()
        {
            if(Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            if(Model.Id == Guid.Empty)
            {
                _stageRepository.InsertUpdate(Model);
                _mediator.Send(new AddedMessage<StageDetailModel> { Id = Model.Id });
            }
            else
            {
                _stageRepository.InsertUpdate(Model);
                _mediator.Send(new UpdateMessage<StageDetailModel> { Id = Model.Id });
            }

            Model = null;
        }

        private bool CanDelete()
            => Model != null
            && Model.Id != Guid.Empty;

        private void Delete()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be deleted");
            }

            if (Model.Id != Guid.Empty)
            {
                var delete = _messageDialogService.Show(
                    "Delete",
                    $"Do you want to delete {Model?.Name}?",
                    MessageDialogButtonConfiguration.YesNo,
                    MessageDialogResult.No);

                if (delete == MessageDialogResult.No)
                    return;

                try
                {
                    _stageRepository.Delete(Model.Id);
                }
                catch
                {
                    _ = _messageDialogService.Show(
                        "Deleting failed!",
                        $"Deleting of {Model?.Name} failed!",
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
                }

                _mediator.Send(new DeleteMessage<StageDetailModel> { Id = Model.Id });
            }
            Model = null;
        }
    }
}
