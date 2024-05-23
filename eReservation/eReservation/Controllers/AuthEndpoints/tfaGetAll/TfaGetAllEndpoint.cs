using eReservation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eReservation.Controllers.AuthEndpoints.tfaGetAll
{
    [Route("Tfa-GetAll")]

    public class TfaGetAllEndpoint : BaseEndpoint<TfasGetAllRequest, TfaGetAllResponse>
    {
        private readonly DataContext db;

        public TfaGetAllEndpoint(DataContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public override async Task<TfaGetAllResponse> Obradi([FromQuery] TfasGetAllRequest request, CancellationToken cancellationToken)
        {
            var lastTwoFKeys = await db.AutentifikacijaToken
                .GroupBy(x => x.KorisnickiNalogId)
                .Select(group => new
                {
                    KorisnickiNalogId = group.Key,
                    LastTwoFKey = group.OrderByDescending(x => x.id).Select(x => x.TwoFKey).FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            var tfas = lastTwoFKeys.Select(x => new TfasGetAllResponseRow
            {
                ID = x.KorisnickiNalogId,
                TwoFKey = x.LastTwoFKey
            }).ToList();

            return new TfaGetAllResponse
            {
                Tfas = tfas
            };
        }
    }
}
