using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeGit.Models
{
    public interface IRepositoryGraphBuilder
    {
        RepositoryGraph Graph();
    }
}
