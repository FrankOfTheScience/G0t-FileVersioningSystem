using G0tLib.Models;

namespace G0tLib.Interfaces;
public interface IG0tApi
{
    void Init();
    void Commit(string message);
    List<CommitInfo> Log();
    void Status();
    void Add(string filePath);
}