namespace App.Domain.Exceptions;

//CriticalException ın ctor da tanımladığımız mesajı miras aldığımız Exception' a gönder
internal class CriticalException(string message) : Exception(message);