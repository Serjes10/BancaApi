namespace BancaApi.Domain.Entities;

public class Response<T>
{
    public T Data { get; set; }
    public bool HasError { get; set; }

    public List<string> Errors {get;set;}

    public Response(){
        HasError = false;
        Errors = new List<string>();
    }

    public Response(T data){
        Data = data;
        HasError = false;
        Errors = new List<string>();
    }

    public Response(List<string> errors){
        HasError = true;
        Errors = errors ?? new List<string>();
    }



}


