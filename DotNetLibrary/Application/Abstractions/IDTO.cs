namespace Application.Abstractions;

public interface IDTO<out T> where T : class
{
    public T ToEntity();
}