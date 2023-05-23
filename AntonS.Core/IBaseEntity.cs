namespace AntonS.Core;

public interface IBaseEntity
{
    public int Id { get; set; }

}
public interface IBaseEntity<T>
{
    public T Id { get; set; }
}