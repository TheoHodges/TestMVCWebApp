using System.Net.Http;
using System.Threading.Tasks;
using TestApp.Models;

namespace TestApp.Services
{
    public interface IPersonService
    {
        Task<HttpResponseMessage> GetPeople();
        Task<HttpResponseMessage> PostPerson(Person person);
    }
}