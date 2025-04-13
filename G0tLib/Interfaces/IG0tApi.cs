using G0tLib.Commands;

namespace G0tLib.Interfaces;
public interface IG0tApi
{
    void Init();
    void Commit(string message);
    List<CommitInfo> Log();
    void Status();
    void Add(string filePath);
    void Checkout(string commitHash);
    void CreateBranch(string branchName);
    void SwitchBranch(string branchName);
}