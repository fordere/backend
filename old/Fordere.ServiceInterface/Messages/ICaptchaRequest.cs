using Fordere.ServiceInterface.Dtos;

namespace Fordere.ServiceInterface.Messages
{
    public interface ICaptchaRequest
    {
        string Captcha { get; set; }
    }
}