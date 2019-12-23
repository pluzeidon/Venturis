using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Venturis.Model;

namespace Venturis.Interfaces
{
    public interface IMyAPI
    {
        [Post("/insComprobantes")]
        Task<PostContent> SumitPost([Body] PostContent post);
    }
}
