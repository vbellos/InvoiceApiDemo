using System.Collections.Generic;
using System.Linq;

namespace Common.Models;

public class Company
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public IEnumerable<User> Users { get; set; } = Enumerable.Empty<User>();
}