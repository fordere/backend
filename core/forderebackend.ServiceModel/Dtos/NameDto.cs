namespace forderebackend.ServiceModel.Dtos
{
    public class NameDto : BaseDto
    {
        public virtual string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", this.Name, this.Id);
        }
    }
}