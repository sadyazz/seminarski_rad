namespace eReservation.Controllers.AuthEndpoints.tfaGetAll
{
    public class TfaGetAllResponse
    {
        public List<TfasGetAllResponseRow> Tfas { get; set; }
    }

    public class TfasGetAllResponseRow
    {
        public int ID { get; set; }
        public string TwoFKey { get; set; }
    }
}
