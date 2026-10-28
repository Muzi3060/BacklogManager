using System;
using System.Collections.Generic;

namespace BacklogManager.Models;

public sealed record ColecaoPersonalizada(Guid Id, string Nome, string Descricao, IReadOnlyList<Guid> Obras);
