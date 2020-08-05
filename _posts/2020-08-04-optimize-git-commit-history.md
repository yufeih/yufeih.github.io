---
layout: post
title: Optimizing Git Contributor List
---

# Optimizing Git Contributor List

There is a nice feature on [docs] that shows the portraits of contributors to a docs page. Despite the tiny screen space they took, the time to calculate this list can be suprisingly high.

![docs contributor list](/assets/docs-contributor-list.png)

The way how [docs] calculates git contributor list for an artical is simple: It first executes `git log` on the source file to get the commit history, then it calls some GitHub API to associate a GitHub account to the email address of each commit. If an article has the `author` metadata, the author appears first in the contributor list.

For repositories like [azure-docs], the commit history can be huge. Today it contains `613,513` commits and is still growing rapidly each day. Unlike code repositories, documentation repositories tend to see large amounts of small commits, `>50%` of which are single file edits.

![](/assets/azure-docs-commits.png)

Executing `git log` alone on [azure-docs] took more than 20 seconds on my desktop:

![](/assets/azure-docs-git-log.png)

Executing `git log {file}` for some random files took somewhere from 2 seconds to 6 seconds to complete. There are about `15,000` articles in [azure-docs], even if `git log` are fully parallelized on my 8 core desktop, it still took at least `2 hours` just to execute `git log` for every file.

This is one of the biggest performance bottleneck of the docs content pipeline. With the optimization described below, we are able to drive down this number to about 4 minutes for a fresh build, and a couple of seconds for subsequent builds.

## Reading Git Data in Process Using `libgit2`

The first step is to replace `git` the exe with an in process library [libgit2]. [libgit2] comes with a [NuGet package](https://www.nuget.org/packages/LibGit2Sharp) so its extreamly easy to embed to a .NET Core application. Moving to [libgit2] removes the extra overhead of spawning extra processes, and more importantly opens up the door of further optimizations that require access to git internals.

If you are not familiar with git internals, [Derrick Stolee] has a series of blog posts on optimizing git for Azure DevOps. It is a bit techy but you only need to know these two sections: [what makes a commit](https://devblogs.microsoft.com/devops/supercharging-the-git-commit-graph-ii-file-format/#what-makes-a-commit) and [how git stores files as a merkle tree](https://devblogs.microsoft.com/devops/super-charging-the-git-commit-graph-iv-bloom-filters/#file-history-in-git).


## How Git Calcualtes Commit History

Git calculates the commit history for a file by walking the commit graph and picks the commits that have changed the file. In the simpliest linear history case below, the hash for file _a.md_ has changed from `d00c` in commit 1 (`C1`) to `373d` in `C2`, so `git log` on _a.md_ produces `[C2, C1]`.

![](/assets/git-commit-linear.png)

A commit can have multiple parent commits

![](/assets/git-commit-all-parents.png)

![](/assets/git-commit-single-parent.png)

## Sharing Git Object Data Between Files




[docs]: https://docs.microsoft.com
[azure-docs]: https://github.com/MicrosoftDocs/azure-docs
[libgit2]: https://libgit2.org
[Derrick Stolee]: https://devblogs.microsoft.com/devops/author/stolee/