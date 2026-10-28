using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BacklogManager.Models;

namespace BacklogManager.Contracts;

public interface IPesquisaCatalogo
{
    Task<IReadOnlyList<ObraCatalogo>> BuscarAsync(string termo, CancellationToken cancellationToken = default);
}
