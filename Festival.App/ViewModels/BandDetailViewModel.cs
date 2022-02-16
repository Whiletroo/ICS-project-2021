using System;
using System.Windows.Input;
using Festival.BL.Models;
using Festival.BL.Repositories;
using Festival.App.Commands;
using Festival.App.Messages;
using Festival.App.Services;

namespace Festival.App.ViewModels
{
    public class BandDetailViewModel : ViewModelBase
    {
        public BandDetailModel? Model { get; set; }

        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }

        private readonly IMediator _mediator;
        private readonly IMessageDialogService _messageDialogService;
        private readonly BandRepository _bandRepository;

        public BandDetailViewModel(
            BandRepository bandRepository,
            IMessageDialogService messageDialogService,
            IMediator mediator
            )
        {
            _bandRepository = bandRepository;
            _messageDialogService = messageDialogService;
            _mediator = mediator;

            SaveCommand = new RelayCommand(Save, CanSave);
            DeleteCommand = new RelayCommand(Delete, CanDelete);

            _mediator.Register<SelectedMessage<BandDetailModel>>(BandSelected);
            _mediator.Register<NewMessage<BandDetailModel>>(BandNew);
        }

        private void BandSelected(SelectedMessage<BandDetailModel> message)
            => Model = _bandRepository.GetById(message.Id);

        private void BandNew(NewMessage<BandDetailModel> message)
            => Model = new BandDetailModel();

        private bool CanSave() =>
            Model != null
            && !string.IsNullOrWhiteSpace(Model.Name)
            && Model.Genre != default
            && Model.OriginCountry != default;

        private void Save()
        {
            if(Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            if(Model.Id == Guid.Empty)
            {
                _bandRepository.InsertUpdate(Model);
                _mediator.Send(new AddedMessage<BandDetailModel> { Id = Model.Id });
            }
            else
            {
                _bandRepository.InsertUpdate(Model);
                _mediator.Send(new UpdateMessage<BandDetailModel> { Id = Model.Id });
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
                    _bandRepository.Delete(Model.Id);
                }
                catch
                {
                    _ = _messageDialogService.Show(
                        "Deleting failed!",
                        $"Deleting of {Model?.Name} failed!",
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
                }

                _mediator.Send(new DeleteMessage<BandDetailModel> { Id = Model.Id });
            }
            Model = null;
        }
    }
}
