using Microsoft.AspNetCore.Mvc;

namespace eReservation.Controllers
{
    [ApiController]
    public abstract class BaseEndpoint<TRequest, TResponse>:ControllerBase
    {
        public abstract Task<TResponse>Obradi(TRequest request, CancellationToken cancellationToken);
    }
}
