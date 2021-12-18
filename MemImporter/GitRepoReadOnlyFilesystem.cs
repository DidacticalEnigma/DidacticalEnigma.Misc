using System;
using System.Collections.Generic;
using System.IO;
using DidacticalEnigma.Project;
using LibGit2Sharp;

namespace MemImporter;

public class GitRepoReadOnlyFilesystem : IReadOnlyFileSystem
{
    private readonly Commit commit;

    public GitRepoReadOnlyFilesystem(Commit commit)
    {
        this.commit = commit;
    }

    public Stream FileOpen(string path)
    {
        var blob = (Blob)this.commit[path].Target;
        return blob.GetContentStream();
    }

    public IEnumerable<string> List(string path)
    {
        path = path.EndsWith("/", StringComparison.InvariantCulture) ? path.Remove(path.Length - 1) : path;
        var target = path == ""
            ? this.commit.Tree
            : (Tree)this.commit.Tree[path].Target;
        foreach (var entry in target)
        {
            yield return $"{entry.Path + (entry.Mode == Mode.Directory ? "/" : "")}";
        }
    }
}