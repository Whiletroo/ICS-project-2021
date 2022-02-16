using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Festival.BL.Repositories;
using Festival.BL.Models;
using Festival.App.Services;
using Festival.App.Messages;
using Festival.App.Commands;
using Festival.App.Extensions;

namespace Festival.App.ViewModels
{
    public class StageListViewModel : ViewModelBase
    {
        private readonly StageRepository _stageRepository;
        private readonly IMediator _mediator;

        public ObservableCollection<StageListModel> Stages { get; set; } = new ObservableCollection<StageListModel>();

        public ICommand StageSelectedCommand { get; set; }
        public ICommand StageNewCommand { get; set; }

        public StageListViewModel(StageRepository stageRepository, IMediator mediator)
        {
            _stageRepository = stageRepository;
            _mediator = mediator;

            StageSelectedCommand = new RelayCommand<StageListModel>(StageSelected);
            StageNewCommand = new RelayCommand(StageNew);

            mediator.Register<AddedMessage<StageDetailModel>>(StageAdded);
            mediator.Register<UpdateMessage<StageDetailModel>>(StageUpdated);
            mediator.Register<DeleteMessage<StageDetailModel>>(StageDeleted);
        }
        
        private void StageNew()
            => _mediator.Send(new NewMessage<StageDetailModel>());

        private void StageSelected(StageListModel stageListModel)
            => _mediator.Send(new SelectedMessage<StageDetailModel> { Id = stageListModel.Id });

        private void StageAdded(AddedMessage<StageDetailModel> _) => Load();

        private void StageUpdated(UpdateMessage<StageDetailModel> _) => Load();

        private void StageDeleted(DeleteMessage<StageDetailModel> _) => Load();

        public override void Load()
        {
            Stages.Clear();
            var stages = _stageRepository.GetAll();
            Stages.AddRange(stages);
        }
    }
}
