namespace forderebackend.ServiceModel.Dtos
{
    public class PaymentDto : BaseDto
    {
        public int UserId { get; set; }

        public string Comment { get; set; }

        public bool HasPaid { get; set; }

        public UserDto User { get; set; }

        public string UserTeams { get; set; }
    }
}