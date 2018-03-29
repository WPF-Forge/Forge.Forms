
using Forge.Forms.Annotations;

namespace Forge.Forms.Demo.Models
{
    [Text("There are two resources that allow reading text from files.")]
    [Text("{{FileBinding <path>}} allows reading and syncing a file's content, while {{File <path>}} only reads the value once.")]
    [Text("Go to your bin folder and modify example.txt to see it in action.")]
    [Card(2)]
    [Heading("{{FileBinding example.txt}}")]
    [Text("{FileBinding example.txt}")]
    [Break]
    [Card(2)]
    [Heading("{{File example.txt}}")]
    [Text("{File 'example.txt'}")]
    [Break]
    public class FileBindings
    {
    }
}
