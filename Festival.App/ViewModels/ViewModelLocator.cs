using System;
using Microsoft.EntityFrameworkCore;
using Festival.App.Services;
using Festival.BL.Factories;
using Festival.BL.Repositories;


namespace Festival.App.ViewModels
{
    public class ViewModelLocator
    {
        private readonly IMediator _mediator;
        private readonly IMessageDialogService _messageDialogService;
        private readonly BandRepository _bandRepository;
        private readonly StageRepository _stageRepository;
        private readonly PerformanceRepository _performanceRepository;

        public BandListViewModel BandListViewModel => new(_bandRepository, _mediator);
        public BandDetailViewModel BandDetailViewModel => new(_bandRepository, _messageDialogService, _mediator);
        public StageListViewModel StageListViewModel => new(_stageRepository, _mediator);
        public StageDetailViewModel StageDetailViewModel => new(_stageRepository, _messageDialogService, _mediator);
        public PerformanceListViewModel PerformanceListViewModel => new(_performanceRepository, _mediator);
        public PerformanceDetailViewModel PerformanceDetailViewModel
            => new(_performanceRepository, _bandRepository, _stageRepository, _messageDialogService, _mediator);

        public ViewModelLocator()
        {
            _mediator = new Mediator();
            _messageDialogService = new MessageDialogService();
            var dbContextFactory = new DbContextFactory();

            _bandRepository = new BandRepository(dbContextFactory);
            _stageRepository = new StageRepository(dbContextFactory);
            _performanceRepository = new PerformanceRepository(dbContextFactory);

            using var dbx = dbContextFactory.Create();
            dbx.Database.Migrate();
        }
    }
}
