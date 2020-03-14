namespace Fordere.RestService.Entities
{
    public interface IFordereObjectWithName : IFordereObject
    {
        string Name { get; set; }

        //public override string ToString()
        //{
        //    return string.Format("{0} [{1}]", this.Name, this.Id);
        //}
    }
}