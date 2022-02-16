using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Festival.BL.Models;
using Festival.BL.Repositories;
using Festival.App.Commands;
using Festival.App.Messages;
using Festival.App.Services;
using Festival.App.Extensions;

namespace Festival.App.ViewModels
{
    public class BandListViewModel : ViewModelBase
    {
        private readonly BandRepository _bandRepository;
        private readonly IMediator _mediator;

        public ObservableCollection<BandListModel> Bands { get; set; } = new ObservableCollection<BandListModel>();

        public ICommand BandSelectedCommand { get; set; }
        public ICommand BandNewCommand { get; set; }

        public BandListViewModel(BandRepository bandRepository, IMediator mediator)
        {
            _bandRepository = bandRepository;
            _mediator = mediator;

            BandSelectedCommand = new RelayCommand<BandListModel>(BandSelected);
            BandNewCommand = new RelayCommand(BandNew);

            mediator.Register<AddedMessage<BandDetailModel>>(BandAdded);
            mediator.Register<UpdateMessage<BandDetailModel>>(BandUpdated);
            mediator.Register<DeleteMessage<BandDetailModel>>(BandDeleted);
        }

        private void BandNew()
            => _mediator.Send(new NewMessage<BandDetailModel>());

        private void BandSelected(BandListModel bandListModel)
            => _mediator.Send(new SelectedMessage<BandDetailModel> {Id = bandListModel.Id});

        private void BandAdded(AddedMessage<BandDetailModel> band) => Load();

        private void BandUpdated(UpdateMessage<BandDetailModel> band) => Load();

        private void BandDeleted(DeleteMessage<BandDetailModel> band) => Load();

        public override void Load()
        {
            Bands.Clear();
            var bands = _bandRepository.GetAll();
            Bands.AddRange(bands);
        }

    }
}
