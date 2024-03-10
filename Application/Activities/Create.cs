using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest
        {
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;                
            }
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                //At this point we are only adding the data to memory. If this were added
                //directly to the database then we should probably use the async method.
                _context.Activities.Add(request.Activity);

                //Here we are saving the memory to the database. This method should be async.
                await _context.SaveChangesAsync();
            }
        }
    }
}