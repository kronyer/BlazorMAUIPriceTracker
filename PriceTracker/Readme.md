# PriceTracker

O **PriceTracker** é um aplicativo multiplataforma (.NET MAUI) para acompanhamento e comparação de preços de produtos em diferentes estabelecimentos. Com ele, você pode registrar valores, consultar históricos de preços, analisar variações e identificar as melhores oportunidades de compra. O app permite o cadastro rápido de produtos por código de barras, além de oferecer uma interface intuitiva e recursos de busca avançada.

## Funcionalidades

- Cadastro e consulta de produtos por código de barras
- Histórico de preços e variações
- Busca por nome, marca e estabelecimento
- Cadastro de marcas e mercados
- Interface responsiva e amigável
- Integração com leitor de código de barras (ZXing.Net.MAUI)
- Suporte a múltiplas plataformas (.NET 9 MAUI: Android, iOS, Windows, MacCatalyst)

## Tecnologias

- [.NET 9 MAUI](https://learn.microsoft.com/dotnet/maui/)
- MudBlazor
- ZXing.Net.MAUI
- SQLite
- SweetAlert2
- C#

## Como rodar

1. **Pré-requisitos:**  
   - .NET 9 SDK  
   - Visual Studio 2022 (com workload MAUI)

2. **Clone o repositório:**
1. 
3. **Restaure os pacotes:**
 ```dotnet restore```
4. **Compile e execute:**
- No Visual Studio, selecione a plataforma desejada (Android, Windows, etc) e clique em "Run".
- Ou via terminal:
  ```sh
  dotnet build
  dotnet run --project PriceTracker
  ```

## Estrutura do Projeto

- `PriceTracker/Components/Pages/` — Páginas Blazor do app
- `PriceTracker/Data/Repositories/` — Repositórios de dados
- `PriceTracker/Services/` — Serviços de integração e utilitários
- `PriceTracker/Models/` — Modelos de dados
- `PriceTracker/wwwroot/js/` — Scripts JavaScript auxiliares

