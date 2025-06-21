# CRM Sistema de Gráfica - Fullstack

## Descrição

Sistema CRM completo desenvolvido para empresas gráficas, utilizando tecnologias modernas como .NET 8, Blazor Server, Entity Framework Core e com IA em ML.NET para previsão de vendas.

## Tecnologias Utilizadas

### Backend (.NET 8 API)
- **ASP.NET Core Web API** - Framework principal da API
- **Entity Framework Core** - ORM para acesso ao banco de dados
- **SQLite** - Banco de dados (configurável para SQL Server)
- **ClosedXML** - Biblioteca para exportação de arquivos Excel

### Frontend (Blazor Server)
- **Blazor Server** - Framework para interface web interativa
- **Bootstrap 5** - Framework CSS para design responsivo
- **JavaScript** - Para funcionalidades específicas como download de arquivos

### IA e Machine Learning
- **ML.NET** - Framework da Microsoft para machine learning no ambiente .NET
- **Microsoft.ML.Data** - Biblioteca para manipulação e definição dos dados usados no treinamento e predição
- **System.Text.Json** - Para serialização e desserialização dos dados entre frontend e backend

## Funcionalidades Principais

### 1. Gerenciamento de Clientes
- ✅ Cadastro, edição e exclusão de clientes
- ✅ Classificação por tipo (Novo, Recorrente, Inativo)
- ✅ Controle de frequência de compras
- ✅ Interface web intuitiva

### 2. Gerenciamento de Pedidos
- ✅ Criação e acompanhamento de pedidos
- ✅ Controle de status (Orçamento, Aprovado, Produção, Finalizado, Entregue, Cancelado)
- ✅ Associação com clientes
- ✅ Cálculo de valores e datas

### 3. Sistema de IA para Previsão de Vendas
- ✅ Algoritmo de regressão linear para previsão
- ✅ Análise de histórico de vendas
- ✅ Previsão baseada em dados mensais
- ✅ Interface web para visualização

### 4. Relatórios e Exportação
- ✅ Relatório de clientes mais valiosos
- ✅ Histórico de receita mensal
- ✅ Exportação para Excel (ClosedXML)
- ✅ Download automático de arquivos

### 5. Dashboard Executivo
- ✅ Visão geral do negócio
- ✅ Métricas em tempo real
- ✅ Últimos clientes e pedidos
- ✅ Indicadores de performance

## Estrutura do Projeto

```
CRMGrafica/
├── CRMGraficaAPI/              # Backend API
│   ├── Controllers/            # Controllers da API
│   ├── Services/              # Lógica de negócio
│   ├── Models/                # Modelos de dados
│   ├── Dtos/                  # Data Transfer Objects
│   ├── Data/                  # Contexto do banco de dados
├── CRMGraficaBlazor/          # Frontend Blazor
│   ├── Components/            # Componentes Blazor
│   ├── Services/              # Serviços do frontend
│   ├── Models/                # Modelos do frontend
│   └── wwwroot/               # Arquivos estáticos
└── CRMGraficaSolution.sln     # Solução Visual Studio
```

## Como Executar

### Pré-requisitos
- .NET 8 SDK

### Executando a API
```bash
cd CRMGraficaAPI
dotnet restore
dotnet ef database update
dotnet run --urls="http://0.0.0.0:5000"
```

### Executando o Frontend Blazor
```bash
cd CRMGraficaBlazor
dotnet restore
dotnet run --urls="http://0.0.0.0:5001"
```

## Endpoints da API

### Clientes
- `GET /api/Clientes` - Lista todos os clientes
- `POST /api/Clientes` - Cria um novo cliente
- `GET /api/Clientes/{id}` - Busca cliente por ID
- `PUT /api/Clientes/{id}` - Atualiza cliente
- `DELETE /api/Clientes/{id}` - Remove cliente

### Pedidos
- `GET /api/Pedidos` - Lista todos os pedidos
- `POST /api/Pedidos` - Cria um novo pedido
- `GET /api/Pedidos/{id}` - Busca pedido por ID
- `PATCH /api/Pedidos/{id}/Status` - Atualiza status do pedido
- `DELETE /api/Pedidos/{id}` - Remove pedido

### IA
- `POST /api/IA/PreverVendas` - Gera previsão de vendas

### Relatórios
- `GET /api/Reports/ClientesMaisValiosos` - Relatório de clientes valiosos
- `GET /api/Reports/ReceitaMensalHistorica` - Histórico de receita mensal
- `GET /api/Reports/ExportarClientesMaisValiosos` - Exporta Excel

## Acesso ao Sistema

- **API**: http://localhost:5000
- **Swagger**: http://localhost:5000/swagger
- **Frontend**: http://localhost:5001

## Características Técnicas

### Segurança
- Validação de dados nos DTOs
- Tratamento de erros centralizado
- Logs estruturados

### Performance
- Entity Framework com queries otimizadas
- Paginação em endpoints de listagem
- Cache de dados no frontend

### Escalabilidade
- Arquitetura em camadas
- Separação de responsabilidades
- Injeção de dependência


## Desenvolvido por Vitor Durães

Sistema desenvolvido como demonstração de um CRM completo para empresas gráficas, integrando tecnologias modernas de desenvolvimento web e inteligência artificial.

