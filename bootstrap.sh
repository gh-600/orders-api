#!/usr/bin/env bash
# Crea la solución .NET mínima del repo orders-api.
# Ejecútalo en la raíz del repo clonado (necesita el SDK de .NET 9 o superior).
set -euo pipefail

dotnet new sln -n Orders --format sln
dotnet new classlib -n Orders.Domain         -o src/Orders.Domain         -f net9.0
dotnet new classlib -n Orders.Application    -o src/Orders.Application    -f net9.0
dotnet new classlib -n Orders.Infrastructure -o src/Orders.Infrastructure -f net9.0
dotnet new web      -n Orders.Api            -o src/Orders.Api            -f net9.0
dotnet new xunit    -n Orders.Tests          -o tests/Orders.Tests        -f net9.0

dotnet sln Orders.sln add \
  src/Orders.Domain src/Orders.Application src/Orders.Infrastructure \
  src/Orders.Api tests/Orders.Tests

# Dependencias entre capas (hacia dentro, hacia el dominio)
dotnet add src/Orders.Application    reference src/Orders.Domain
dotnet add src/Orders.Infrastructure reference src/Orders.Application
dotnet add src/Orders.Api            reference src/Orders.Application src/Orders.Infrastructure
dotnet add tests/Orders.Tests        reference src/Orders.Domain src/Orders.Application

rm -f src/*/Class1.cs tests/Orders.Tests/UnitTest1.cs

# Agregado mínimo. La invariante de importe no negativo falta a propósito:
# es una de las tareas para el agente.
cat > src/Orders.Domain/Order.cs <<'EOF'
namespace Orders.Domain;

public sealed class Order
{
    public Guid Id { get; } = Guid.NewGuid();
    public decimal Amount { get; }

    public Order(decimal amount) => Amount = amount;
}
EOF

cat > tests/Orders.Tests/OrderTests.cs <<'EOF'
using Orders.Domain;

namespace Orders.Tests;

public class OrderTests
{
    [Fact]
    public void New_order_keeps_amount()
    {
        var order = new Order(42m);
        Assert.Equal(42m, order.Amount);
    }
}
EOF

mkdir -p docs infra security
[ -f docs/README.md ]  || echo "# Orders API docs" > docs/README.md
[ -f infra/main.bicep ] || echo "// placeholder" > infra/main.bicep
[ -f security/README.md ] || echo "# Security notes" > security/README.md

dotnet build Orders.sln
dotnet test Orders.sln
