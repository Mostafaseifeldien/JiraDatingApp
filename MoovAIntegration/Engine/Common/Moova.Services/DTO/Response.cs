namespace Moova.Services.DTO;

public class Response<T>
{
    public Response(T? _data)
    {
        Data = _data;
    }

    public bool IsSuccess => Errors == null;
    public T? Data { get; set; }
    public List<Error>? Errors { get; set; }
}