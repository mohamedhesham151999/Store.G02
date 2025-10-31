namespace Store.G02.Shared.ErrorsModels
{
    public class ValidationError
    {
        public string Field { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}