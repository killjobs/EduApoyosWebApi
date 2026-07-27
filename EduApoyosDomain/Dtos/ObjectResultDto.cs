namespace EduApoyosDomain.Dtos
{
    public class ObjectResultDto<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
    }
}
