using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Festival.BL.Repositories;
using Festival.BL.Models;
using Festival.App.Commands;
using Festival.App.Messages;
using Festival.App.Services;
using Festival.App.Extensions;

namespace Festival.App.ViewModels
{
    public class PerformanceListViewModel : ViewModelBase
    {
        private readonly PerformanceRepository _performanceRepository;
        private readonly IMediator _mediator;

        public PerformanceListViewModel(PerformanceRepository performanceRepository, IMediator mediator)
        {
            _performanceRepository = performanceRepository;
            _mediator = mediator;

            PerformanceSelectedCommand = new RelayCommand<PerformanceListModel>(PerformanceSelected);
            PerformanceNewCommand = new RelayCommand(PerformanceNew);

            mediator.Register<UpdateMessage<PerformanceDetailModel>>(PerformanceUpdated);
            mediator.Register<DeleteMessage<PerformanceDetailModel>>(PerformanceDeleted);
            mediator.Register<AddedMessage<PerformanceDetailModel>>(PerformanceAdded);
        }

        public ObservableCollection<PerformanceListModel> Performances { get; set; } = new ObservableCollection<PerformanceListModel>();

        public ICommand PerformanceSelectedCommand { get; }
        public ICommand PerformanceNewCommand { get; }

        private void PerformanceNew() => _mediator.Send(new NewMessage<PerformanceDetailModel>());

        private void PerformanceUpdated(UpdateMessage<PerformanceDetailModel> _) => Load();

        private void PerformanceAdded(AddedMessage<PerformanceDetailModel> _) => Load();

        private void PerformanceDeleted(DeleteMessage<PerformanceDetailModel> _) => Load();

        private void PerformanceSelected(PerformanceListModel performance) => _mediator.Send(new SelectedMessage<PerformanceDetailModel> { Id = performance.Id });

        public override void Load()
        {
            Performances.Clear();
            var performances = _performanceRepository.GetAll();
            Performances.AddRange(performances);
        }
    }
}
