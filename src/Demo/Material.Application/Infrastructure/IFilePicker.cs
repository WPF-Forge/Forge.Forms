using System.Threading.Tasks;

namespace Material.Application.Infrastructure
{
    public interface IFilePicker
    {
        string GetFile(string fileName, string filter);
    }

    public interface IFileSaver
    {
        string GetFile(string fileName, string filter);
    }
}
