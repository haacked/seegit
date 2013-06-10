# SeeGit - The Git Repository Visualizer

This is a little experiment in creating a realtime git repository visualizer.

![Screenshot](https://f.cloud.github.com/assets/19977/552514/42561530-c357-11e2-9f3d-6514f7f3f47b.png)

__WARNING: This is some haacky code. I plan to rewrite it later. It's a proof of
 concept__

# Goal
I just want something that I can use during presentations on Git. So as I run
commands, it'll show the git graph in a beautiful manner.

# Next steps
* I need to add local and remote branch annotations.

# Development
To build the project you will need [Expression Blend SDK](http://www.microsoft.com/en-gb/download/details.aspx?id=10801).
Additionally, to run the build script you will need Windows PowerShell (Win 7 [here](http://www.microsoft.com/en-us/download/details.aspx?id=34595) and WinXP [here](http://www.microsoft.com/en-us/download/details.aspx?id=16818)).

If you would like to contribute, check out the [CONTRIBUTING guidelines](https://github.com/Haacked/SeeGit/blob/master/CONTRIBUTING.md).

If you're wondering why the unit tests are structured as they are, read my blog
post about [structuring unit tests](http://haacked.com/archive/2012/01/01/structuring-unit-tests.aspx).

# License
[MIT License](https://github.com/Haacked/SeeGit/blob/master/LICENSE.txt)

# Credits

* [LibGit2Sharp](https://github.com/libgit2/libgit2sharp) - [License: MIT](https://github.com/libgit2/libgit2sharp/blob/master/LICENSE.md) LibGit2Sharp is a thin .Net layer (well... we try to keep it as thin as possible :-) ) wrapping the libgit2 linkable C Git library.

* [QuickGraph](http://quickgraph.codeplex.com/) - [License: Ms-PL](http://quickgraph.codeplex.com/license) QuickGraph provides generic directed/undirected graph datastructures and algorithms for .NET. 
* [GraphSharp](http://graphsharp.codeplex.com/) - [License: Ms-PL](http://graphsharp.codeplex.com/) Graph# is a graph layout framework. It contains some layout algorithms and a GraphLayout control for WPF applications
* [Reactive Extensions](http://msdn.microsoft.com/en-us/data/gg577609) - [License: EULA](http://msdn.microsoft.com/en-us/devlabs/ff394099.aspx) The Reactive Extensions (Rx) is a library for composing asynchronous and event-based programs using observable sequences and LINQ-style query operators. 
* [WPFExtensions](http://wpfextensions.codeplex.com/) - [License: Ms-PL](http://wpfextensions.codeplex.com/license) Some extensions for the WPF framework. Controls, attached behaviours, helper classes, etc.
* [Windows&reg; API Code Pack for Microsoft&reg; .NET Framework](http://archive.msdn.microsoft.com/WindowsAPICodePack) - [License: EULA](http://archive.msdn.microsoft.com/WindowsAPICodePack/Project/License.aspx) provides a source code library that can be used to access some features of Windows 7 and Windows Vista from managed code.
