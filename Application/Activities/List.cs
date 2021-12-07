using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<List<Activity>> { }

        public class Handler : IRequestHandler<Query, List<Activity>>
        {

            private readonly DataContext _dataContext;
            private readonly ILogger _logger;
            public Handler(DataContext dataContext, ILogger<List> logger)
            {
                
                _dataContext = dataContext;
                _logger = logger;
            }

            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken) // cancellation token is used to handle long running requests
            {
                try
                {
                    for (var i = 0; i < 10; i++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        await Task.Delay(1000, cancellationToken);
                        _logger.LogInformation($"Task {i} has completed");
                    }
                }
                catch (Exception ex) when (ex is TaskCanceledException) 
                {
                    _logger.LogInformation("Task was cancelled");
                }
                return await _dataContext.Activities.ToListAsync();
            }
        }
    }
}