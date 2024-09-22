using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Services
{
    //result pattern implementasyonu
    //static factory method (new ile yaratmayı kontrol altına almak için)
    public class ServiceResult<T>
    {
        public T? Data { get; set; }

        public List<string>? Errors { get; set; }

        //Status kodu zaten gönderiyoruz. Bu değişkenler bizim iç yapımızda kullanıldığı için döndürdüğümüz json da görünmesine gerek yok.
        [JsonIgnore]
        public bool IsSuccess => Errors == null || Errors.Count == 0;

        [JsonIgnore]
        public bool IsFail => !IsSuccess;

        [JsonIgnore]
        public HttpStatusCode Status { get; set; }

        //Kullanıcı post işlemi yaptığında yeni bir veri kaydettiğinde kaydettiği ürünün erişim url ni SuccessAsCreated metodunda set ediyor.
        [JsonIgnore]
        public string? UrlAsCreated { get; set; }

        public static ServiceResult<T> Success(T data, HttpStatusCode status = HttpStatusCode.OK)
        {
            return new ServiceResult<T> { Data = data, Status = status };
        }

        public static ServiceResult<T> SuccessAsCreated(T data, string urlAsCread)
        {
            return new ServiceResult<T> { Data = data, Status = HttpStatusCode.Created, UrlAsCreated = urlAsCread };
        }

        public static ServiceResult<T> Fail(List<string> errors, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ServiceResult<T> { Errors = errors, Status = status };
        }

        public static ServiceResult<T> Fail(string error, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ServiceResult<T> { Errors = new List<string> { error }, Status = status };
        }
    }

    //Bu  class update ve delete için döndürülecek nesnedir. Update ve delete de data yı tekrar geri döndürmüyoruz.
    public class ServiceResult
    {
        public List<string>? Errors { get; set; }

        [JsonIgnore]
        public bool IsSuccess => Errors == null || Errors.Count == 0;

        [JsonIgnore]
        public bool IsFail => !IsSuccess;

        [JsonIgnore]
        public HttpStatusCode Status { get; set; }

        //static factory method (new ile yaratmayı kontrol altına almak için)
        public static ServiceResult Success(HttpStatusCode status = HttpStatusCode.OK)
        {
            return new ServiceResult { Status = status };
        }

        public static ServiceResult Fail(List<string> errors, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ServiceResult { Errors = errors, Status = status };
        }

        public static ServiceResult Fail(string error, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return new ServiceResult { Errors = new List<string> { error }, Status = status };
        }
    }
}