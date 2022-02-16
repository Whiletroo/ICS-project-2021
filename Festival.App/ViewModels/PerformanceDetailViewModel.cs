using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Festival.BL.Models;
using Festival.BL.Repositories;
using Festival.App.Commands;
using Festival.App.Messages;
using Festival.App.Services;
using Festival.App.Extensions;
using Festival.BL.Exceptions;
using System.Linq;

namespace Festival.App.ViewModels
{
    public class PerformanceDetailViewModel : ViewModelBase
    {
        public PerformanceDetailModel? Model { get; set; }
        public ObservableCollection<BandListModel> Bands { get; set; } = new ObservableCollection<BandListModel>();
        public ObservableCollection<StageListModel> Stages { get; set; } = new ObservableCollection<StageListModel>();

        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }

        private readonly IMediator _mediator;
        private readonly IMessageDialogService _messageDialogService;
        private readonly PerformanceRepository _performanceRepository;
        private readonly BandRepository _bandRepository;
        private readonly StageRepository _stageRepository;

        public PerformanceDetailViewModel(
            PerformanceRepository performanceRepository,
            BandRepository bandRepository,
            StageRepository stageRepository,
            IMessageDialogService messageDialogService,
            IMediator mediator
            )
        {
            _performanceRepository = performanceRepository;
            _bandRepository = bandRepository;
            _stageRepository = stageRepository;
            _messageDialogService = messageDialogService;
            _mediator = mediator;

            SaveCommand = new RelayCommand(Save, CanSave);
            DeleteCommand = new RelayCommand(Delete, CanDelete);

            _mediator.Register<SelectedMessage<PerformanceDetailModel>>(PerformanceSelected);
            _mediator.Register<NewMessage<PerformanceDetailModel>>(PerformanceNew);
        }

        private void PerformanceSelected(SelectedMessage<PerformanceDetailModel> message)
            => Model = _performanceRepository.GetById(message.Id);

        private void PerformanceNew(NewMessage<PerformanceDetailModel> message)
            => Model = new PerformanceDetailModel();

        private bool CanSave() =>
            Model != null
            && Model.StartPerformanceTime.HasValue
            && Model.EndPerformanceTime.HasValue
            && Model.BandId != Guid.Empty
            && Model.StageId != Guid.Empty;

        private void Save()
        {
            if (Model == null)
            {
                throw new InvalidOperationException("Null model cannot be saved");
            }

            MapProperties();

            try
            {
                if (Model.Id == Guid.Empty)
                {
                    _performanceRepository.InsertUpdate(Model);
                    _mediator.Send(new AddedMessage<PerformanceDetailModel> { Id = Model.Id });
                }
                else
                {
                    _performanceRepository.InsertUpdate(Model);
                    _mediator.Send(new UpdateMessage<PerformanceDetailModel> { Id = Model.Id });
                }
            }
            catch (DateTimeCollisionException)
            {
                _messageDialogService.Show("Error",
                    $"There is a performance time collision!",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
                return;
            }
            catch (DateTimeStartEndException)
            {
                _messageDialogService.Show("Error",
                    $"Start time must be before end time!",
                    MessageDialogButtonConfiguration.OK,
                    MessageDialogResult.OK);
                return;
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
                    $"Do you want to delete performance of {Model?.BandName} on {Model?.StageName} at {Model?.StartPerformanceTime}?",
                    MessageDialogButtonConfiguration.YesNo,
                    MessageDialogResult.No);

                if (delete == MessageDialogResult.No)
                    return;

                try
                {
                    _performanceRepository.Delete(Model.Id);
                }
                catch
                {
                    _ = _messageDialogService.Show(
                        "Deleting failed!",
                        $"Deleting of performance of {Model?.BandName} on {Model?.StageName} at {Model?.StartPerformanceTime} failed!",
                        MessageDialogButtonConfiguration.OK,
                        MessageDialogResult.OK);
                }

                _mediator.Send(new DeleteMessage<PerformanceDetailModel> { Id = Model.Id });
            }
            Model = null;
        }

        public override void Load()
        {
            Bands.Clear();
            var bands = _bandRepository.GetAll();
            Bands.AddRange(bands);

            Stages.Clear();
            var stages = _stageRepository.GetAll();
            Stages.AddRange(stages);
        }

        private void MapProperties()
        {
            var bandItem = Bands.Where(item => item.Id == Model.BandId).FirstOrDefault();
            Model.BandName = bandItem.Name;
            var stageItem = Stages.Where(item => item.Id == Model.StageId).FirstOrDefault();
            Model.StageName = stageItem.Name;
        }
    }
}

