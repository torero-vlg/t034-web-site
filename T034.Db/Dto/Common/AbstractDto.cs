namespace T034.Core.Dto.Common
{
    public abstract class AbstractDto<T> : IDto<T>
    {
        public T Id { get; set; }
    }
}
